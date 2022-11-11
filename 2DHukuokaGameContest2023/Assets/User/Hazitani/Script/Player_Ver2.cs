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

	[SerializeField, Header("攻撃を当てた時の浮遊時間")]
	private int AttackingTime;

	[SerializeField, Header("敵を倒したあとの飛ぶ力")]
	private Vector2 SubjugationKnockback;

	[SerializeField, Header("コンボテキスト")]
	private Text Combo;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private Rigidbody2D rb2D;				//主人公のリジットボディ
	private int jump_count = 0;				//ジャンプ回数
	private bool jump_key_flag = false;		//ジャンプキー連続判定制御用
	private bool ground_hit = false;		//地面に立っているか
	private int now_move = 0;				//左:-1・停止:0・右:1
	private bool player_frip = false;		//プレイヤーの向きtrue右false左
	private bool move_stop = false;			//動きを止めたいとき使用
	private int combo_count = 0;			//コンボ数


	//攻撃関連
	private Vector3 mousePos;				//マウスの位置取得用
	private Vector3 target;					//攻撃位置調整用
	private Quaternion atkQuaternion;		//攻撃角度
	private bool attack_ok = true;			//攻撃出来るかどうか出来るときtrue
	private bool attacking = false;			//攻撃中true
	private bool dont_move = false;			//敵にめり込んだ時に敵の向きを1回取る用
	private GameObject attack = null;       //攻撃オブジェクト
	private int attack_cooltime = 0;		//攻撃クールタイム
	[System.NonSerialized]
	public Vector3 hit_enemy_pos;			//攻撃が当たった敵の位置
	[System.NonSerialized]
	public bool hit_enemy = false;			//攻撃が敵に当たったかどうか
	private bool hit_enemy_frip = false;	//攻撃した敵の方向true右false左


	//接地関連
	private Ray2D ray_left, ray_right;			//飛ばすレイ
	private float distance = 2.0f;				//レイを飛ばす距離
	private RaycastHit2D hit_left,hit_right;	//レイが何かに当たった時の情報
	private Vector3 rayPosition1, rayPosition2;	//レイを発射する位置
	private GameObject SearchGameObject = null; //レイに触れたオブジェクト取得用


	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
		Combo.text = combo_count.ToString();
	}

	void Update()
    {
		//攻撃の向き設定
		Vector3 attackpos = transform.position;//攻撃位置の座標更新用

		//攻撃
		if (Input.GetMouseButtonDown(0))
		{
			if (attack_ok)
			{
				attack_ok = false;
				attacking = true;

				//角度設定
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				target = Vector3.Scale((mousePos - transform.position), new Vector3(0, 0, 0)).normalized;
				atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

				//コライダーを生成
				PlayerAttack(attack, AttackCollider, attackpos);
			}
		}
	}

	void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//レイを発射する位置の調整
		rayPosition1 = transform.position + new Vector3(-0.5f, -transform.localScale.y / 2, 0.0f);
		rayPosition2 = transform.position + new Vector3(0.5f, -transform.localScale.y / 2, 0.0f);

		//レイの接地判定
		RayGround(ray_left, hit_left, rayPosition1);
		RayGround(ray_right, hit_right, rayPosition2);

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
			if(!jump_key_flag)
            {
				jump_key_flag = true;
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
			else if (transform.position.x > hit_enemy_pos.x)
			{
				if (!dont_move)
				{
					dont_move = true;
					hit_enemy_frip = false;
				}
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos + AttackMovePos, AttackMoveSpeed);
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
			ground_hit = true;
			move_stop = false;

			//コンボリセットして反映
			combo_count = 0;
			Combo.text = combo_count.ToString();
		}

		//攻撃のノックバック
		if (collision.gameObject.tag == "Enemy")
		{
			//ジャンプ回数リセット
			jump_count = 0;

			//コンボ増やして反映
			combo_count++;
			Combo.text = combo_count.ToString();

			AttackFin();
		}
	}

    //コライダーに触れている間
    private void OnCollisionStay2D(Collision2D collision)
    {
		//地面に触れた時
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_hit = true;
			move_stop = false;

			//コンボリセットして反映
			combo_count = 0;
			Combo.text = combo_count.ToString();
		}

		if (collision.gameObject.tag == "Enemy")
		{
			AttackFin();
		}
	}

    //コライダーから離れた時
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_hit = false;
		}

		if (collision.gameObject.tag == "Enemy")
		{
			AttackFin();
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

			if (SearchGameObject.tag == "Ground" && ground_hit == true && jump_count > 0)
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
		if (attacking)
		{
			hit_enemy = false;
			rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			dont_move = false;
			attacking = false;
			if (hit_enemy_frip)
			{
				rb2D.AddForce(new Vector2(-SubjugationKnockback.x, SubjugationKnockback.y), ForceMode2D.Force);
			}
			else
			{
				rb2D.AddForce(SubjugationKnockback, ForceMode2D.Force);
			}
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
}