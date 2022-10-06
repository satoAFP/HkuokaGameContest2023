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


	private Rigidbody2D rb2D;
	private bool first = true;
	private int jump_count = 0;

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//移動処理
		if (Input.GetKey(KeyCode.A))
		{
			//最高速度になるとそれ以上加速しない
			if (rb2D.velocity.x > -LimitSpeed)
			{
				rb2D.AddForce(-transform.right * (MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(-MoveSpeed, 0, 0);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			//最高速度になるとそれ以上加速しない
			if (rb2D.velocity.x < LimitSpeed)
			{
				rb2D.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(MoveSpeed, 0, 0);
			}
		}


		//ジャンプ処理
		if (Input.GetKey(KeyCode.Space) && jump_count < 2)
		{
			//1回のみ
			if (first)
			{
				Debug.Log("ジャンプ入力された");
				rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

				//カウント増加
				jump_count++;
				first = false;
			}
		}
		else
			first = true;
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
		//Groundタグに触れた場合
        if(other.gameObject.CompareTag("Ground"))
        {
			Debug.Log("着地しました");
			//カウントリセット
			jump_count = 0;
        }
    }
}