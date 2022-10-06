using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ver2 : MonoBehaviour
{
	[SerializeField, Header("ジャンプ力"), Range(0, 100)]
	private float JumpPower;

	[SerializeField, Header("移動速度"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("最高速度"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("重力"), Range(0, 100)]
	private float Gravity;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private Rigidbody2D rb2D;
	private int jump_count = 0;
	private int now_move = 0;//左:-1・停止:0・右:1

	private Ray2D ray;				//飛ばすレイ
	private float distance = 0.1f;	//レイを飛ばす距離
	private RaycastHit2D hit;		//レイが何かに当たった時の情報
	private Vector3 rayPosition;	//レイを発射する位置

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//レイを発射する位置の調整
		rayPosition = transform.position + new Vector3(0.0f, -0.5f, 0.0f);
		//レイを下に飛ばす
		ray = new Ray2D(rayPosition, transform.up * -1);
		//レイを赤色で表示させる
		Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

		////レイが当たったとき
		//if (Physics2D.Raycast())
		//{
		//	if (hit.collider.tag == "Ground")
		//	{
		//		Debug.Log("レイ当たってます");
		//		jump_count = 0;
		//	}
		//}

		//ジャンプ処理
		if (Input.GetKeyDown(KeyCode.Space) && jump_count < 2)
		{
			Debug.Log("ジャンプ入力された");
			rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

			//カウント増加
			jump_count++;
		}

		//弱攻撃
		if (Input.GetMouseButtonDown(0))
		{
			switch(now_move)
            {
				case (int)Direction.LEFT:
					Debug.Log("左弱キック！");
					break;
				case (int)Direction.STOP:
					Debug.Log("弱パンチ！");
					break;
				case (int)Direction.RIGHT:
					Debug.Log("右弱キック！");
					break;
			}
		}

		//強攻撃
		if (Input.GetMouseButtonDown(1))
		{
			switch (now_move)
			{
				case (int)Direction.LEFT:
					Debug.Log("左強キック！");
					break;
				case (int)Direction.STOP:
					Debug.Log("強パンチ！");
					break;
				case (int)Direction.RIGHT:
					Debug.Log("右強キック！");
					break;
			}
		}
	}

    void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//移動処理
		if (Input.GetKey(KeyCode.A))
		{
			now_move = (int)Direction.LEFT;
			//最高速度になるとそれ以上加速しない
			if (rb2D.velocity.x > -LimitSpeed)
			{
				rb2D.AddForce(-transform.right * (MoveSpeed), ForceMode2D.Force);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			now_move = (int)Direction.RIGHT;
			//最高速度になるとそれ以上加速しない
			if (rb2D.velocity.x < LimitSpeed)
			{
				rb2D.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
			}
		}
		if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
			now_move = (int)Direction.STOP;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			Debug.Log("着地した！");
			jump_count = 0;
		}
	}
}