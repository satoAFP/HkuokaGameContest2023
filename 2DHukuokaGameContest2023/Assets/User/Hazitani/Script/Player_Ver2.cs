using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[SerializeField, Header("敵と衝突した時のノックバック")]
	private Vector2 KnockbackPow;

	[SerializeField, Header("攻撃当たり判定")]
	private GameObject AttackkCollider;


	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private Rigidbody2D rb2D;
	private int jump_count = 0;			//ジャンプ回数
	private bool ground_hit = false;	//地面に立っているか
	private int now_move = 0;			//左:-1・停止:0・右:1
	private bool player_frip = false;   //プレイヤーの向きtrue右false左
	private bool move_stop = false;     //動きを止めたいとき使用
	private bool avoiding = false;      //回避中かどうか
	private float avoid_time = 0;       //回避時間

	//攻撃関連
	private Vector3 mousePos;           //マウスの位置取得用
	private Vector3 target;             //攻撃位置調整用
	private Quaternion atkQuaternion;   //攻撃角度
	private GameObject attack = null;   //攻撃オブジェクト
	[System.NonSerialized]//これ付けたらパブリックでもインスペクターに出てこん！！！！！
	public Vector3 hit_enemy_pos;       //攻撃が当たった敵の位置
	[System.NonSerialized]
	public bool hit_enemy = false;		//攻撃が敵に当たったかどうか

	//レイ関連
	private Ray2D ray_left, ray_right;			//飛ばすレイ
	private float distance = 2.0f;				//レイを飛ばす距離
	private RaycastHit2D hit_left,hit_right;	//レイが何かに当たった時の情報
	private Vector3 rayPosition1, rayPosition2;	//レイを発射する位置
	private GameObject SearchGameObject = null; //レイに触れたオブジェクト取得用

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//レイを発射する位置の調整
		rayPosition1 = transform.position + new Vector3(-0.5f, -transform.localScale.y / 2, 0.0f);
		rayPosition2 = transform.position + new Vector3( 0.5f, -transform.localScale.y / 2, 0.0f);

		//レイの接地判定
		RayGround(ray_left, hit_left, rayPosition1);
		RayGround(ray_right, hit_right, rayPosition2);

        //ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && jump_count < 1)
		{
			Debug.Log("ジャンプ入力された");
			rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

			//カウント増加
			jump_count++;
		}

		//攻撃の向き設定
		Vector3 attackpos = transform.position;//攻撃位置の座標更新用
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target = Vector3.Scale((mousePos - transform.position), new Vector3(1, 1, 0)).normalized;

		//攻撃の位置調整したいけどわからん

		//攻撃
		if (Input.GetMouseButtonDown(0))
		{
			//角度設定
			atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

			Debug.Log("こうげき！");
			PlayerAttack(attack, AttackkCollider, attackpos);
		}

		//攻撃が敵に当たった場合
		if(hit_enemy)
        {
			Debug.Log("攻撃当たった");
			switch(now_move)
            {
				case (int)Direction.LEFT:
					transform.position = hit_enemy_pos + new Vector3(1, 0, 0);
					break;
				case (int)Direction.RIGHT:
					transform.position = hit_enemy_pos + new Vector3(-1, 0, 0);
					break;
			}
			transform.position = hit_enemy_pos + new Vector3(1, 0, 0);
			hit_enemy = false;
		}

		//回避
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl))
        {
			if (!avoiding)
			{
				avoiding = true;
				if (Input.GetKey(KeyCode.A))
				{
					Debug.Log("左回避");
					rb2D.velocity = -transform.right * AvoidDis;
				}
				else if (Input.GetKey(KeyCode.D))
				{
					Debug.Log("右回避");
					rb2D.velocity = transform.right * AvoidDis;
				}
				else
				{
					Debug.Log("その場回避");
				}
			}
        }
	}

    void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//移動処理
		if (!move_stop)
		{
			//回避中ではないとき
			if (!avoiding)
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
			else
			{
				//回避時間
				avoid_time++;

				if (avoid_time >= AvoidTime)
				{
					avoiding = false;
					avoid_time = 0;
				}
			}
		}
	}
	
	//コライダーに触れた時
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
			ground_hit = true;
			move_stop = false;
		}

		//主人公と衝突時のノックバック
		if (other.gameObject.tag == "Enemy")
		{
			if (player_frip)
			{
				rb2D.AddForce(new Vector2(-KnockbackPow.x, KnockbackPow.y), ForceMode2D.Force);
			}
			else
			{
				rb2D.AddForce(KnockbackPow, ForceMode2D.Force);
			}
			move_stop = true;
		}
	}

	//コライダーから離れた時
    private void OnCollisionExit2D(Collision2D other)
    {
		if (other.gameObject.CompareTag("Ground"))
		{
			ground_hit = false;
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