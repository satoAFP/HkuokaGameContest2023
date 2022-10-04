using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ver2 : MonoBehaviour
{
	[SerializeField, Header("�W�����v��"), Range(0, 100)]
	private float JumpPower;

	[SerializeField, Header("�ړ����x"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("�ō����x"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("�d��"), Range(0, 100)]
	private float Gravity;



	private bool first = true;
	private Rigidbody2D rigidbody;

	void Start()
	{
		rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		//�d�͐ݒ�
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//�ړ�����
		if (Input.GetKey(KeyCode.A))
		{
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
			if (rigidbody.velocity.x > -LimitSpeed)
			{
				rigidbody.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(-MoveSpeed, 0, 0);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
			if (rigidbody.velocity.x < LimitSpeed)
			{
				rigidbody.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(MoveSpeed, 0, 0);
			}
		}



		//�W�����v����
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