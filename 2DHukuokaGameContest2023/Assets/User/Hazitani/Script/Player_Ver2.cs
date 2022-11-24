using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player_Ver2 : BaseStatusClass
{
	[SerializeField, Header("ジャンプ力"), Range(0, 100)]
	private float JumpPower;

	[SerializeField, Header("移動速度"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("最高速度"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("重力"), Range(0, 100)]
	private float Gravity;

	[SerializeField, Header("回避距離"), Range(0, 100)]
	private float AvoidDis;

	[SerializeField, Header("回避時間"), Range(0, 100)]
	private int AvoidTime;

	[SerializeField, Header("攻撃当たり判定")]
	private GameObject AttackCollider;

	[SerializeField, Header("攻撃が届く距離"), Range(0, 10)]
	private int AttackDistance;

	[SerializeField, Header("攻撃のクールタイム")]
	private int AttackCoolTime;

	[SerializeField, Header("攻撃を当てた時の移送位置")]
	private Vector3 AttackMovePos;

	[SerializeField, Header("攻撃を当てた時の移送速度")]
	private float AttackMoveSpeed;

	[SerializeField, Header("敵に当たったあとの飛ぶ力")]
	private Vector2 SubjugationKnockback;

	[SerializeField, Header("攻撃中に剣を回転させる速さ")]
	private int AttackRotationSpeed;


	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}


	//主人公関連
	private Rigidbody2D rb2D;				//主人公のリジットボディ
	private int jump_count = 0;             //ジャンプ回数
	private bool jump_key_flag = false;     //ジャンプキー連続判定制御用
	private bool ground_on = false;			//地面に立っているか
	private int now_move = 0;				//左:-1・停止:0・右:1
	private bool player_frip = false;		//プレイヤーの向きtrue右false左
	private bool move_stop = false;         //動きを止めたいとき使用


	//システム関連
	private int combo_fever_count = 0;		//フィーバータイムに入るために必要なコンボ数
	private int time_fever = 0;				//フィーバータイムの時間を数える用


	//攻撃関連
	private Vector3 mousePos;				//マウスの位置取得用
	private Vector3 target;					//攻撃位置調整用
	private Quaternion atkQuaternion;		//攻撃角度
	private bool attack_ok = true;			//攻撃出来るかどうか出来るときtrue
	private bool attacking = false;			//攻撃中true
	private bool dont_move = false;         //敵の向きを1回取る用
	private Ray2D attack_ray;				//飛ばすレイ
	private RaycastHit2D attack_hit;		//レイが何かに当たった時の情報
	private Vector3 attack_rayPosition;		//レイを発射する位置
	private GameObject attack = null;       //攻撃オブジェクト
	private int attack_cooltime = 0;        //攻撃クールタイム
	private int attack_rotation = 0;        //攻撃中の剣回転
	[System.NonSerialized]
	public bool attack_col = false;			//アタックコライダー出現中true
	[System.NonSerialized]
	public Vector3 hit_enemy_pos;			//攻撃が当たった敵の位置
	[System.NonSerialized]
	public bool hit_enemy = false;			//攻撃が敵に当たったかどうか
	private bool hit_enemy_frip = false;    //攻撃した敵の方向true右false左
	[System.NonSerialized]
	public BaseEnemyFly enemyObj = null;	//攻撃に当たった敵のオブジェクト


	//接地関連
	private Ray2D ground_ray;					//飛ばすレイ
	private float distance = 2.0f;				//レイを飛ばす距離
	private RaycastHit2D ground_hit;			//レイが何かに当たった時の情報
	private Vector3 ground_rayPosition;			//レイを発射する位置
	private GameObject SearchGameObject = null; //レイに触れたオブジェクト取得用


	void Start()
	{
		//リジットボディ登録
		rb2D = GetComponent<Rigidbody2D>();

		//マネージャーに登録
		ManagerAccessor.Instance.player = this;
	}

	void Update()
    {
		//攻撃の向き設定
		Vector3 attackpos = transform.position;//攻撃位置の座標更新用

		//角度設定
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target = Vector3.Scale((mousePos - transform.position), new Vector3(0, 0, 0)).normalized;
		atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

		//攻撃
		if (Input.GetMouseButtonDown(0))
		{
			if (attack_ok)
			{
				attack_ok = false;
				attacking = true;

				//コライダーを生成
				//PlayerAttack(attack, AttackCollider, attackpos);

				//レイを発射する位置の調整
				attack_rayPosition = transform.position;

				//レイを飛ばす
				attack_ray = new Ray2D(attack_rayPosition, mousePos - attack_rayPosition);

				//Enemyとだけ衝突する
				int attack_layerMask = LayerMask.GetMask(new string[] { "Enemy" });
				attack_hit = Physics2D.Raycast(attack_ray.origin, attack_ray.direction, AttackDistance, attack_layerMask);

				//レイを黄色で表示させる
				Debug.DrawRay(attack_ray.origin, attack_ray.direction * AttackDistance, Color.yellow);

				//コライダーとレイが接触
				if (attack_hit.collider)
                {
                    if (attack_hit.collider.tag == "Enemy")
                    {
                        enemyObj = attack_hit.collider.gameObject.GetComponent<BaseEnemyFly>();
                        hit_enemy_pos = enemyObj.transform.position;
                        hit_enemy = true;
                    }
                }
            }
		}
	}

	void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//接地判定
		//レイを発射する位置の調整
		ground_rayPosition = transform.position + new Vector3(0.0f, -transform.localScale.y / 2, 0.0f);

		//レイの接地判定
		RayGround(ground_ray, ground_hit, ground_rayPosition);

		//剣の回転
		if (hit_enemy)
        {
			transform.GetChild(0).gameObject.transform.rotation = atkQuaternion * Quaternion.Euler(0, 0, 90);
		}
		else
        {
			transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		//移動処理
		if (!move_stop)
		{
			if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				now_move = (int)Direction.LEFT;
				player_frip = false;//左向き
				
				//最高速度になるとそれ以上加速しない
				if (rb2D.velocity.x > -LimitSpeed)
				{
					rb2D.AddForce(-transform.right * (MoveSpeed), ForceMode2D.Force);
				}
			}
			if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
			{
				now_move = (int)Direction.RIGHT;
				player_frip = true;//右向き
				
				//最高速度になるとそれ以上加速しない
				if (rb2D.velocity.x < LimitSpeed)
				{
					rb2D.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
				}
			}
			if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				now_move = (int)Direction.STOP;
			}
		}

		//ジャンプ処理
		if (Input.GetKey(KeyCode.Space) && jump_count < 1)
		{
			if (!jump_key_flag)
			{
				jump_key_flag = true;
				move_stop = false;
				Debug.Log("ジャンプ入力された");

				rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

				//カウント増加
				jump_count++;
			}
		}
		else
		{
			jump_key_flag = false;
		}

		//攻撃が敵に当たった場合
		if (hit_enemy)
		{
			move_stop = true;
			rb2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

			if (enemyObj != null)
			{
				hit_enemy_pos = enemyObj.transform.position;
			}

			if (transform.position.x < hit_enemy_pos.x)
			{
				if (!dont_move)
				{
					dont_move = true;
					hit_enemy_frip = true;
				}
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos - AttackMovePos, AttackMoveSpeed);
			}
			else
			{
				if (!dont_move)
				{
					dont_move = true;
					hit_enemy_frip = false;
				}
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos + AttackMovePos, AttackMoveSpeed);
			}

			if(transform.position == hit_enemy_pos)
            {
				AttackFin();
            }
		}

		//攻撃クールタイム
		if (!attack_ok)
        {
			attack_cooltime++;

			if(attack_cooltime >= AttackCoolTime)
            {
				attack_cooltime = 0;
				attack_ok = true;
            }
        }

		//フィーバータイムのとき
		if (ManagerAccessor.Instance.systemManager.FeverTime)
        {
			//フィーバータイムをカウント
			time_fever++;
			Debug.Log(ManagerAccessor.Instance.systemManager.FeverTime);

			//時間経過したら
			if (time_fever >= 500)
			{
				//フィーバータイム終了
				ManagerAccessor.Instance.systemManager.FeverTime = false;
				time_fever = 0;
			}
		}
	}

	//コライダーに触れた時
    private void OnCollisionEnter2D(Collision2D collision)
    {
		//地面に触れた時
        if (collision.gameObject.CompareTag("Ground"))
        {
			ground_on = true;
			move_stop = false;

			ComboReset();
		}

		//攻撃のノックバック
		if (collision.gameObject.tag == "Enemy")
		{
			if (attacking)
				Attack();
		}
	}

    //コライダーに触れている間
    private void OnCollisionStay2D(Collision2D collision)
    {
		//地面に触れた時
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = true;
			move_stop = false;

			ComboReset();
		}

		if (collision.gameObject.tag == "Enemy")
		{
			if(attacking)
				Attack();
		}
	}

    //コライダーから離れた時
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = false;
		}
	}

	//レイの接地判定
	private void RayGround(Ray2D ray, RaycastHit2D hit, Vector3 vec)
	{
		//レイを下に飛ばす
		ray = new Ray2D(vec, -transform.up);

		//Groundとだけ衝突する
		int layerMask = LayerMask.GetMask(new string[] { "Ground" });
		hit = Physics2D.Raycast(ray.origin, ray.direction, distance, layerMask);

		//レイを赤色で表示させる
		Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

		//コライダーとレイが接触
		if (hit.collider)
		{
			SearchGameObject = hit.collider.gameObject;

			if (SearchGameObject.tag == "Ground" && ground_on == true && jump_count > 0)
			{
				Debug.Log("着地してる！");
				jump_count = 0;
			}
		}
	}

	//攻撃オブジェクト生成
	private void PlayerAttack(GameObject attack, GameObject prefab, Vector3 attackpos)
    {
		attack = Instantiate(prefab, attackpos += target, atkQuaternion);
		attack.transform.parent = gameObject.transform;
		attack.GetComponentInChildren<AttckCollision>().Damage = ATK;
	}

	//攻撃解除
	private void AttackFin()
	{
		//ジャンプ回数リセット
		jump_count = 0;

		hit_enemy = false;
		rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		dont_move = false;
		attacking = false;

		//攻撃後跳ね返り
		if (hit_enemy_frip)
		{
			rb2D.AddForce(new Vector2(-SubjugationKnockback.x, SubjugationKnockback.y), ForceMode2D.Impulse);
		}
		else
		{
			rb2D.AddForce(SubjugationKnockback, ForceMode2D.Impulse);
		}
	}

	//スコア加算
	private int ScoreSetting(int score, int combo)
    {
		//コンボによってスコア加算
		if (combo >= 1 && combo < 50)
			score += 1000;
		else if (combo >= 50 && combo < 100)
			score += 5000;
		else if (combo >= 100)
			score += 10000;

		/*
		コンボボーナス
			 10コンボごとに  5000点
			 50コンボごとに 10000点
			100コンボごとに100000点
		*/
		if (combo % 100 == 0)
		{
			score += 100000;
		}
		else if (combo % 50 == 0)
		{
			score += 10000;
		}
		else if (combo % 10 == 0)
		{
			score += 5000;
		}

		return score;
	}

	//攻撃処理
	private void Attack()
    {
		//コンボ増やして反映
		ManagerAccessor.Instance.systemManager.Combo++;
		ManagerAccessor.Instance.systemManager.textCombo.text = ManagerAccessor.Instance.systemManager.Combo.ToString();

		//マックスコンボ変更
		if (ManagerAccessor.Instance.systemManager.MaxCombo < ManagerAccessor.Instance.systemManager.Combo)
		{
			ManagerAccessor.Instance.systemManager.MaxCombo = ManagerAccessor.Instance.systemManager.Combo;
			ManagerAccessor.Instance.systemManager.textMaxCombo.text = ManagerAccessor.Instance.systemManager.MaxCombo.ToString();
		}

		//フィーバータイムではないとき
		if (!ManagerAccessor.Instance.systemManager.FeverTime)
		{
			//フィーバータイムまでのコンボをカウント
			combo_fever_count++;
			Debug.Log(ManagerAccessor.Instance.systemManager.FeverTime);

			//コンボを達成したとき
			if (combo_fever_count >= 10)
            {
				//フィーバータイムに移行
				ManagerAccessor.Instance.systemManager.FeverTime = true;

				//フィーバー用のコンボを初期化
				combo_fever_count = 0;
			}
		}

		if (enemyObj != null)
		{
			//敵のHP減らす
			enemyObj.HP -= ATK;

			//表示用のスコア決める
			ManagerAccessor.Instance.systemManager.Score = ScoreSetting(ManagerAccessor.Instance.systemManager.Score, ManagerAccessor.Instance.systemManager.Combo);
			ManagerAccessor.Instance.systemManager.textScore.text = ManagerAccessor.Instance.systemManager.Score.ToString();

			//HPが0の時
			//if (enemyObj.HP <= 0)
			//{
			//    //ヒットストップの処理
			//}
		}

		AttackFin();
	}

	//二点間の角度を求める関数
	//引数1　原点となるオブジェクト座標
	//引数2　角度を求めたいオブジェクト座標
	public float GetAim(Vector3 p1, Vector3 p2)
	{
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dy, dx);
		return rad * Mathf.Rad2Deg;
	}

	private void ComboReset()
    {
		//コンボリセットして反映
		if (!ManagerAccessor.Instance.systemManager.FeverTime)
		{
			//コンボリセットして反映
			ManagerAccessor.Instance.systemManager.Combo = 0;
			ManagerAccessor.Instance.systemManager.textCombo.text = ManagerAccessor.Instance.systemManager.Combo.ToString();

			//フィーバータイムに必要なコンボも初期化
			combo_fever_count = 0;
		}
	}
}

/* やること
 * 主人公からカーソルを表示
 * 敵に攻撃したら効果音「ズシャア！！」
*/

/* バグ
 * 高くジャンプする
 * 攻撃をスカしてから敵に当たるとコンボ増える
 * Wave切り替えタイミングで攻撃すると止まる
*/