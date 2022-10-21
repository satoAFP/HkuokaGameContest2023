using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
	private Rigidbody2D rigidbody2d;

	void Start()
	{
		rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		//�d�͐ݒ�
		Physics2D.gravity = new Vector3(0, -Gravity, 0);
		

		//�ړ�����
		if (Input.GetKey(KeyCode.A))
		{
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
			if (rigidbody2d.velocity.x > -LimitSpeed)
			{
				rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
			if (rigidbody2d.velocity.x < LimitSpeed)
			{
				rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
			}
		}



		//�W�����v����
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


	//��_�Ԃ̊p�x�����߂�֐�
	//����1�@���_�ƂȂ�I�u�W�F�N�g���W
	//����2�@�p�x�����߂����I�u�W�F�N�g���W
	public float GetAim(Vector3 p1, Vector3 p2)
	{
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dy, dx);
		return rad * Mathf.Rad2Deg;
	}
}
