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

	[SerializeField, Header("コンボテキスト")]
	private Text Combo;

	[SerializeField, Header("最大コンボテキスト")]
	private Text ComboMax;

	[SerializeField, Header("スコアテキスト")]
	private Text Score;

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
	private int combo_count = 0;            //コンボ数
	private int combo_max = 0;              //最大コンボ数
	private int score = 0;                  //スコア
	private int score_add = 0;              //これから加算されるスコア
	private SpawnEnemy spawn_enemy;         //スポーンエネミー取得用


	//攻撃関連
	private Vector3 mousePos;				//マウスの位置取得用
	private Vector3 target;					//攻撃位置調整用
	private Quaternion atkQuaternion;		//攻撃角度
	private bool attack_ok = true;			//攻撃出来るかどうか出来るときtrue
	private bool attacking = false;			//攻撃中true
	private bool dont_move = false;         //敵の向きを1回取る用
	private Ray2D attack_ray;				//飛ばすレイ
	private float attack_distance = 10.0f;  //レイを飛ばす距離
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
		rb2D = GetComponent<Rigidbody2D>();
		Combo.text = combo_count.ToString();
		ComboMax.text = combo_max.ToString();
		Score.text = score.ToString();

		spawn_enemy = GameObject.Find("SpawnEnemy").GetComponent<SpawnEnemy>();
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
				attack_ray = new Ray2D(attack_rayPosition, atkQuaternion.eulerAngles);
				Debug.Log(atkQuaternion.eulerAngles.normalized);

				//Enemyとだけ衝突する
				int attack_layerMask = LayerMask.GetMask(new string[] { "Enemy" });
				attack_hit = Physics2D.Raycast(attack_ray.origin, attack_ray.direction, attack_distance, attack_layerMask);

				//レイを黄色で表示させる
				Debug.DrawRay(attack_ray.origin, attack_ray.direction * attack_distance, Color.yellow);

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
		//レイを発射する位置の調整
		ground_rayPosition = transform.position + new Vector3(0.0f, -transform.localScale.y / 2, 0.0f);
		//rayPosition2 = transform.position + new Vector3(0.5f, -transform.localScale.y / 2, 0.0f);

		//レイの接地判定
		RayGround(ground_ray, ground_hit, ground_rayPosition);

		if(attack_col)
        {
			//剣の回転
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
	}

	//コライダーに触れた時
    private void OnCollisionEnter2D(Collision2D collision)
    {
		//地面に触れた時
        if (collision.gameObject.CompareTag("Ground"))
        {
			ground_on = true;
			move_stop = false;

			//コンボリセットして反映
			if (spawn_enemy.NowWave	!= 1)
			{
				combo_count = 0;
				Combo.text = combo_count.ToString();
			}
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

			//コンボリセットして反映
			combo_count = 0;
			Combo.text = combo_count.ToString();
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

	private void Attack()
    {
		//コンボ増やして反映
		combo_count++;
		Combo.text = combo_count.ToString();

		//マックスコンボ変更
		if (combo_max < combo_count)
		{
			combo_max = combo_count;
			ComboMax.text = combo_max.ToString();
		}

		//現在のウェーブが通常ウェーブのとき
		if (spawn_enemy.NowWave == 0)
		{
			spawn_enemy.WaveCombo++;
			//これから加算されるスコアを決める
			score_add = ScoreSetting(score_add, combo_count);
			spawn_enemy.WaveScore += score_add;
		}

		if (enemyObj != null)
		{
			//敵のHP減らす
			enemyObj.HP -= ATK;

			//表示用のスコア決める
			score = ScoreSetting(score, combo_count);
			Score.text = score.ToString();

			//HPが0の時
			//if (enemyObj.HP <= 0)
			//{
			//    //ヒットストップの処理
			//}
		}

		score_add = 0;
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
}

/* やること
 * 主人公の攻撃オブジェクトをレイに変換
 * 主人公からカーソルを表示
 * 敵に攻撃したら効果音「ズシャア！！」
*/

/* バグ
 * 先の敵が死ぬ
 * 高くジャンプする
 * 攻撃をスカしてから敵に当たるとコンボ増える
 * Wave切り替えタイミングで攻撃すると止まる
*/