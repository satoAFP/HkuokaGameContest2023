using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ver2 : MonoBehaviour
{
	[SerializeField, Header("ジャンプ力"), Range(0, 1000)]
	private float JumpPower;

	[SerializeField, Header("移動速度"), Range(0, 100)]
	private float MoveSpeed;

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
			//rigidbody.AddForce(new Vector3(-MoveSpeed, 0, 0));
			rigidbody.velocity = new Vector3(-MoveSpeed, 0, 0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			//rigidbody.AddForce(new Vector3(MoveSpeed, 0, 0));
			rigidbody.velocity = new Vector3(MoveSpeed, 0, 0);
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
