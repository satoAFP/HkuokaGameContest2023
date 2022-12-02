using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player_Ver2 : BaseStatusClass
{
	/// <summary>
	/// Playerオブジェクトの子オブジェクトの順番
	/// 
	/// Player
	///		PlayerSprite
	///			rainbow_aura
	///			aura
	///		Arrow
	///		
	/// この順番じゃないとGetChild関数がバグります
	/// </summary>

	[SerializeField, Header("ジャンプ力"), Range(0, 100)]
	private float JumpPower;

	[SerializeField, Header("移動速度"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("最高速度"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("重力"), Range(0, 100)]
	private float Gravity;

	[SerializeField, Header("落下最高速度"), Range(0, 100)]
	private float FallSpeed;

	[SerializeField, Header("攻撃が届く距離"), Range(0, 10)]
	private float AttackDistance;

	[SerializeField, Header("攻撃のクールタイム")]
	private int AttackCoolTime;

	[SerializeField, Header("攻撃を当てた時の移送位置")]
	private Vector3 AttackMovePos;

	[SerializeField, Header("攻撃を当てた時の移送速度")]
	private float AttackMoveSpeed;

	[SerializeField, Header("敵に当たったあとの飛ぶ力")]
	private Vector2 SubjugationKnockback;

	[SerializeField, Header("ヒットストップのフレーム数"), Range(0, 100)]
	private int HitStopFrame;

	[SerializeField, Header("剣の回転アニメーション")]
	private Animator RotationAnimator;

	[SerializeField, Header("カーソルの表示")]
	private bool AttackCursor;

	//[SerializeField, Header("フィーバータイムのテキスト")]
	//private Text textFeverTime;

	[SerializeField, Header("オレンジオーラ出すまでのコンボ数"), Range(0, 100)]
	private int OrangeCombo;

	[SerializeField, Header("フィーバータイムまでのコンボ数"), Range(0, 100)]
	private int FeverCombo;

	[SerializeField, Header("フィーバータイムの時間"), Range(0, 100)]
	private int FeverTime;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private enum PrefabChild
    {
		PlayerSprite = 0,
			Rainbow_Aura = 0,
			Aura = 1,
		Arrow = 1,
			ArrowImage = 0,
	}


	//主人公関連
	private Rigidbody2D rb2D;				//主人公のリジットボディ
	private int jump_count = 0;             //ジャンプ回数
	private bool jump_key_flag = false;     //ジャンプキー連続判定制御用
	private bool ground_on = false;			//地面に立っているか
	private int now_move = 0;				//左:-1・停止:0・右:1
	private bool player_frip = false;		//プレイヤーの向きtrue右false左
	private bool move_stop = false;         //動きを止めたいとき使用
	private Ray2D cursor_ray;               //飛ばすレイ
	private RaycastHit2D cursor_hit;        //レイが何かに当たった時の情報
	private Vector3 cursor_rayPosition;     //レイを発射する位置


	//システム関連
	private int combo_fever_count = 0;      //フィーバータイムに入るために必要なコンボ数
	[System.NonSerialized]
	public int time_fever = 0;              //フィーバータイムの時間を数える用
	[System.NonSerialized]
	public int score_add = 0;				//これから加算されるスコア


	//攻撃関連
	private Vector3 mousePos;				//マウスの位置取得用
	private Vector3 target;					//攻撃位置調整用
	private Quaternion atkQuaternion;		//攻撃角度
	private bool attack_ok = true;			//攻撃出来るかどうか出来るときtrue
	private bool dont_move = false;         //敵の向きを1回取る用
	private Ray2D attack_ray;				//飛ばすレイ
	private RaycastHit2D attack_hit;		//レイが何かに当たった時の情報
	private Vector3 attack_rayPosition;		//レイを発射する位置
	private int attack_cooltime = 0;        //攻撃クールタイム
	private bool hitstop_on = false;        //ヒットストップ中true
	private int hitstop_frame = 0;			//ヒットストップフレームカウント用
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

		//テキストの初期化
		//textFeverTime.text = "";
	}

	void Update()
    {
		//角度設定
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target = Vector3.Scale((mousePos - transform.position), new Vector3(0, 0, 0)).normalized;
		atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

		//カーソルの色変更
		transform.GetChild((int)PrefabChild.Arrow).GetChild((int)PrefabChild.ArrowImage).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);

		//カーソルのレイ
		//レイを発射する位置の調整
		cursor_rayPosition = transform.position;

		//レイを飛ばす
		cursor_ray = new Ray2D(cursor_rayPosition, mousePos - cursor_rayPosition);

		//Enemyとだけ衝突する
		int attack_layerMask = LayerMask.GetMask(new string[] { "Enemy" });
		cursor_hit = Physics2D.Raycast(cursor_ray.origin, cursor_ray.direction, AttackDistance, attack_layerMask);

		//コライダーとレイが接触
		if (cursor_hit.collider)
		{
			if (cursor_hit.collider.tag == "Enemy")
			{
				//カーソルの色変更
				transform.GetChild((int)PrefabChild.Arrow).GetChild((int)PrefabChild.ArrowImage).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
			}
		}

		//攻撃
		if (Input.GetMouseButtonDown(0))
		{
			if (attack_ok)
			{
				attack_ok = false;

				//レイを発射する位置の調整
				attack_rayPosition = transform.position;

				//レイを飛ばす
				attack_ray = new Ray2D(attack_rayPosition, mousePos - attack_rayPosition);

				//Enemyとだけ衝突する
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
						hitstop_frame = 0;
					}
                }
            }
		}
	}

	void FixedUpdate()
	{
		//落下最高速度を超えないようにする
		if (rb2D.velocity.y < -FallSpeed)
		{
			Physics2D.gravity = Vector3.zero;
		}
		else
        {
			//重力設定
			Physics2D.gravity = new Vector3(0, -Gravity, 0);
		}

		//接地判定
		//レイを発射する位置の調整
		ground_rayPosition = transform.position + new Vector3(0.0f, -transform.localScale.y / 2, 0.0f);

		//レイの接地判定
		RayGround(ground_ray, ground_hit, ground_rayPosition);

		//剣の回転
		if (hit_enemy)
        {
			if (!hitstop_on)
				transform.GetChild((int)PrefabChild.PlayerSprite).gameObject.transform.rotation = atkQuaternion * Quaternion.Euler(0, 0, 90);
		}
		else
        {
			if (!hitstop_on)
				transform.GetChild((int)PrefabChild.PlayerSprite).gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		//カーソルを表示するかどうか
		if (AttackCursor)
		{
			//矢印を表示
			transform.GetChild((int)PrefabChild.Arrow).gameObject.SetActive(true);
			
			//矢印の回転
			transform.GetChild((int)PrefabChild.Arrow).gameObject.transform.localScale = new Vector3(1.0f, AttackDistance / 5, 1.0f);
			transform.GetChild((int)PrefabChild.Arrow).gameObject.transform.rotation = atkQuaternion * Quaternion.Euler(0, 0, 90);
		}
		else
        {
			//矢印を非表示
			transform.GetChild((int)PrefabChild.Arrow).gameObject.SetActive(false);
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

				rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

				//カウント増加
				jump_count++;
			}
		}
		else
		{
			jump_key_flag = false;
		}

		//レイが敵に当たった場合
		if (hit_enemy)
		{
			if (enemyObj != null)
			{
				move_stop = true;
				rb2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

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
			}
			//レイが当たったが、敵が消えてしまった場合
			else
            {
				if (!hitstop_on)
				{
					//攻撃関連のフラグリセット
					jump_count = 0;
					hitstop_on = false;
					hitstop_frame = 0;
					move_stop = false;
					hit_enemy = false;
					rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
					dont_move = false;
					attack_cooltime = 0;
					attack_ok = true;

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
			}
		}

		//ヒットストップ
		if(hitstop_on)
        {
			hitstop_frame++;

			if(hitstop_frame >= HitStopFrame)
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

			//if (time_fever % 50 == 0)
			//{
			//	textFeverTime.text = "あと" + (FeverTime - (time_fever / 50)) + "秒";
			//}

			//時間経過したら
			if (time_fever >= FeverTime * 50)
			{
				//フィーバータイム終了
				ManagerAccessor.Instance.systemManager.FeverTime = false;
				time_fever = 0;

				//オーラの処理
				//虹色のオーラを消す
				transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Rainbow_Aura).gameObject.SetActive(false);
				//20コンボ以上ならオレンジオーラを出す
				if (ManagerAccessor.Instance.systemManager.Combo >= OrangeCombo)
				{
					transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(true);
				}

				//textFeverTime.text = "";
				//textFeverTime.gameObject.SetActive(false);
			}
		}
	}

    //コライダーに触れた時
    private void OnTriggerEnter2D(Collider2D collider)
    {
		//攻撃のノックバック
		if (collider.gameObject.tag == "Enemy")
		{
			if (hit_enemy)
            {
				if (!hitstop_on)
                {
					hitstop_on = true;
					hitstop_frame = 0;
					Attack();
				}
			}
		}
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
		//地面に触れた時
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = true;
			move_stop = false;

			ComboReset();
		}
	}

    //コライダーに触れている間
    private void OnTriggerStay2D(Collider2D collider)
    {
		if (collider.gameObject.tag == "Enemy")
		{
			if (hit_enemy)
			{
				if (!hitstop_on)
				{
					hitstop_on = true;
					hitstop_frame = 0;
					Attack();
				}
			}
		}
	}
    private void OnCollisionStay2D(Collision2D collision)
    {
		//地面に触れた時
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = true;
			move_stop = false;

			ComboReset();
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
				jump_count = 0;
			}
		}
	}

	//攻撃解除
	private void AttackFin()
	{
		//攻撃関連のフラグ全部リセット
		jump_count = 0;
		hitstop_on = false;
		hitstop_frame = 0;
		hit_enemy = false;
		rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		dont_move = false;

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
	private int ScoreSetting(int combo)
    {
		//加算されるスコアを初期化
		score_add = 0;

		//コンボによってスコア加算
		if (combo >= 1 && combo < 50)
			score_add += 1000;
		else if (combo >= 50 && combo < 100)
			score_add += 5000;
		else if (combo >= 100)
			score_add += 10000;

		/*
		コンボボーナス
			 10コンボごとに  5000点
			 50コンボごとに 10000点
			100コンボごとに100000点
		*/
		if (combo % 100 == 0)
		{
			score_add += 100000;
		}
		else if (combo % 50 == 0)
		{
			score_add += 10000;
		}
		else if (combo % 10 == 0)
		{
			score_add += 5000;
		}

		return score_add;
	}

	//攻撃処理
	private void Attack()
	{
		//剣の回転
		StartCoroutine(StartRotato());

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

			//20コンボ以上ならオレンジオーラを出す
			if (ManagerAccessor.Instance.systemManager.Combo >= OrangeCombo)
			{
				transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(true);
			}

			//コンボを達成したとき
			if (combo_fever_count >= FeverCombo)
			{
				//フィーバータイムに移行
				ManagerAccessor.Instance.systemManager.FeverTime = true;

				//フィーバー用のコンボを初期化
				combo_fever_count = 0;

				//虹色のオーラを出す
				transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(false);
				transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Rainbow_Aura).gameObject.SetActive(true);

				//残り時間テキストを表示
				//textFeverTime.gameObject.SetActive(true);
				//textFeverTime.text = "あと" + (FeverTime - (time_fever / 50)) + "秒";
			}
		}

		if (enemyObj != null)
		{
			//敵のHP減らす
			enemyObj.HP -= ATK;

			//表示用のスコア決める
			ManagerAccessor.Instance.systemManager.Score += ScoreSetting(ManagerAccessor.Instance.systemManager.Combo);
			ManagerAccessor.Instance.systemManager.textScore.text = ManagerAccessor.Instance.systemManager.Score.ToString();

			//HPが0の時
			//if (enemyObj.HP <= 0)
			//{
			//    //ヒットストップの処理
			//}
		}
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

	//コンボリセット
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

			//オレンジオーラも消す
			transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(false);
		}
	}

	//回転アニメーション
	private IEnumerator StartRotato()
	{
		RotationAnimator.SetTrigger("Attack");
		yield return null;//1フレーム待つ

		while (!RotationAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idel"))
		{
			//演出中は待機
			yield return null;
		}
	}
}

/* やること
 * カーソルわかりやすく
 * 敵に攻撃したら効果音「ズシャア！！」
*/

/* バグ
 * 高くジャンプする
*/