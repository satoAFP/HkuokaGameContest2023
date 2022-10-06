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


	private Rigidbody2D rb2D;
	private bool first = true;
	private int jump_count = 0;

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		//�d�͐ݒ�
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//�ړ�����
		if (Input.GetKey(KeyCode.A))
		{
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
			if (rb2D.velocity.x > -LimitSpeed)
			{
				rb2D.AddForce(-transform.right * (MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(-MoveSpeed, 0, 0);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
			if (rb2D.velocity.x < LimitSpeed)
			{
				rb2D.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
				//rigidbody.velocity = new Vector3(MoveSpeed, 0, 0);
			}
		}


		//�W�����v����
		if (Input.GetKey(KeyCode.Space) && jump_count < 2)
		{
			//1��̂�
			if (first)
			{
				Debug.Log("�W�����v���͂��ꂽ");
				rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

				//�J�E���g����
				jump_count++;
				first = false;
			}
		}
		else
			first = true;
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
		//Ground�^�O�ɐG�ꂽ�ꍇ
        if(other.gameObject.CompareTag("Ground"))
        {
			Debug.Log("���n���܂���");
			//�J�E���g���Z�b�g
			jump_count = 0;
        }
    }
}