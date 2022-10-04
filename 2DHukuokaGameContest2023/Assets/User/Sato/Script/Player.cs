using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField, Header("ジャンプ力"), Range(0, 100)] 
	private float JumpPower;

	[SerializeField, Header("移動速度"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("最高速度"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("重力"), Range(0, 100)]
	private float Gravity;



	private bool first = true;
	private Rigidbody2D rigidbody;

	void Start()
	{
		rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);
		

		//移動処理
		if (Input.GetKey(KeyCode.A))
		{
			//最高速度になるとそれ以上加速しない
			if (rigidbody.velocity.x > -LimitSpeed)
			{
				rigidbody.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(-MoveSpeed, 0, 0);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			//最高速度になるとそれ以上加速しない
			if (rigidbody.velocity.x < LimitSpeed)
			{
				rigidbody.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(MoveSpeed, 0, 0);
			}
		}



		//ジャンプ処理
		if (Input.GetKey(KeyCode.Space))
		{
			if (first)
			{
				rigidbody.AddForce(transform.up * JumpPower);
				rigidbody.velocity = transform.up * JumpPower;
			}
			first = false;
		}
		else
			first = true;
	}
}
