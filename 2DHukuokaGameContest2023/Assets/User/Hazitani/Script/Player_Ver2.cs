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

	[SerializeField, Header("自身からどの位置に攻撃判定を設定するか")]
	private Vector3 AttackPos;

	[SerializeField, Header("当たり判定弱パンチ")]
	private GameObject AtkColWeekPunch;

	[SerializeField, Header("当たり判定弱キック")]
	private GameObject AtkColWeekKick;

	[SerializeField, Header("当たり判定強パンチ")]
	private GameObject AtkColStrongPunch;

	[SerializeField, Header("当たり判定強キック")]
	private GameObject AtkColStrongKick;


	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private enum LastAttack
    {
		NONE,	//何もしていない
		WEAK,	//弱攻撃
		STRONG,	//強攻撃
		STRONG2,//強攻撃2回目
	}

	private Rigidbody2D rb2D;
	private int jump_count = 0;			//ジャンプ回数
	private bool ground_hit = false;	//地面に立っているか
	private int now_move = 0;			//左:-1・停止:0・右:1
	private bool player_frip = false;   //プレイヤーの向きtrue右false左
	private bool move_stop = false;     //動きを止めたいとき使用
	private int last_attack = 0;        //最後の攻撃（コンボがつながるか確認用）
	private float gap_time = 0;			//攻撃後の後隙の時間
	private bool avoiding = false;      //回避中かどうか
	private float avoid_time = 0;		//回避時間

	private Ray2D ray_left, ray_right;			//飛ばすレイ
	private float distance = 2.0f;				//レイを飛ばす距離
	private RaycastHit2D hit_left,hit_right;	//レイが何かに当たった時の情報
	private Vector3 rayPosition1, rayPosition2;	//レイを発射する位置
	private GameObject SearchGameObject = null; //レイに触れたオブジェクト取得用
	
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

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//レイを発射する位置の調整
		rayPosition1 = this.transform.position + new Vector3(-0.5f, -transform.localScale.y / 2, 0.0f);
		rayPosition2 = this.transform.position + new Vector3( 0.5f, -transform.localScale.y / 2, 0.0f);

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
		Vector3 attackfrip = transform.position;//攻撃位置の座標更新用
		if (player_frip)
			attackfrip += AttackPos;//右
		else
			attackfrip -= AttackPos;//左

		//弱攻撃
		if (Input.GetMouseButtonDown(0))
		{
			switch(now_move)
            {
				case (int)Direction.LEFT:
					Debug.Log("左弱キック！");
					GameObject week_left_kick = Instantiate(AtkColWeekKick, attackfrip, Quaternion.identity);
					week_left_kick.transform.parent = gameObject.transform;
					week_left_kick.GetComponent<AttckCollision>().Damage = ATK;
					break;
				case (int)Direction.STOP:
					Debug.Log("弱パンチ！");
					GameObject week_punch = Instantiate(AtkColWeekPunch, attackfrip, Quaternion.identity);
					week_punch.transform.parent = gameObject.transform;
					week_punch.GetComponent<AttckCollision>().Damage = ATK;
					break;
				case (int)Direction.RIGHT:
					Debug.Log("右弱キック！");
					GameObject week_right_kick = Instantiate(AtkColWeekKick, attackfrip, Quaternion.identity);
					week_right_kick.transform.parent = gameObject.transform;
					week_right_kick.GetComponent<AttckCollision>().Damage = ATK;
					break;
			}
			last_attack = (int)LastAttack.WEAK;
			gap_time = 20;
		}

		//強攻撃
		if (Input.GetMouseButtonDown(1))
		{
			if (last_attack != (int)LastAttack.STRONG2)
			{
				switch (now_move)
				{
					case (int)Direction.LEFT:
						Debug.Log("左強キック！");
						GameObject strong_left_kick = Instantiate(AtkColStrongKick, attackfrip, Quaternion.identity);
						strong_left_kick.transform.parent = gameObject.transform;
						strong_left_kick.GetComponent<AttckCollision>().Damage = ATK;
						break;
					case (int)Direction.STOP:
						Debug.Log("強パンチ！");
						GameObject strong_punch = Instantiate(AtkColStrongPunch, attackfrip, Quaternion.identity);
						strong_punch.transform.parent = gameObject.transform;
						strong_punch.GetComponent<AttckCollision>().Damage = ATK;
						break;
					case (int)Direction.RIGHT:
						Debug.Log("右強キック！");
						GameObject strong_right_kick = Instantiate(AtkColStrongKick, attackfrip, Quaternion.identity);
						strong_right_kick.transform.parent = gameObject.transform;
						strong_right_kick.GetComponent<AttckCollision>().Damage = ATK;
						break;
				}

				//最後の攻撃が「何もしていない」か「弱攻撃」のとき「強攻撃」にする
				if (last_attack == (int)LastAttack.NONE || last_attack == (int)LastAttack.WEAK)
				{
					last_attack = (int)LastAttack.STRONG;
					gap_time = 30;
				}
				//「強攻撃」のとき「強攻撃2回目」にする
				else if(last_attack == (int)LastAttack.STRONG)
				{
					last_attack = (int)LastAttack.STRONG2;
					gap_time = 60;
				}
			}
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

		//攻撃した後
		if(last_attack != (int)LastAttack.NONE && gap_time >= 0)
        {
			gap_time--;

			if(gap_time <= 0)
            {
				last_attack = (int)LastAttack.NONE;
				gap_time = 0;
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
}