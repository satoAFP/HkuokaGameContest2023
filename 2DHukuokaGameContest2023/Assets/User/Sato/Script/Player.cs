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
	private Rigidbody2D rigidbody2d;

	void Start()
	{
		rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);
		

		//移動処理
		if (Input.GetKey(KeyCode.A))
		{
			//最高速度になるとそれ以上加速しない
			if (rigidbody2d.velocity.x > -LimitSpeed)
			{
				rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			//最高速度になるとそれ以上加速しない
			if (rigidbody2d.velocity.x < LimitSpeed)
			{
				rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
			}
		}



		//ジャンプ処理
		if (Input.GetKey(KeyCode.Space))
		{
			if (first)
			{
				rigidbody2d.AddForce(transform.up * JumpPower);
				rigidbody2d.velocity = transform.up * JumpPower;
			}
			first = false;
		}
		else
			first = true;
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
