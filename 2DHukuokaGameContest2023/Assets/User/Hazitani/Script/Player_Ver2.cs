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

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private Rigidbody2D rb2D;
	private int jump_count = 0;
	private int now_move = 0;//��:-1�E��~:0�E�E:1

	private Ray2D ray;				//��΂����C
	private float distance = 0.1f;	//���C���΂�����
	private RaycastHit2D hit;		//���C�������ɓ����������̏��
	private Vector3 rayPosition;	//���C�𔭎˂���ʒu

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//���C�𔭎˂���ʒu�̒���
		rayPosition = transform.position + new Vector3(0.0f, -0.5f, 0.0f);
		//���C�����ɔ�΂�
		ray = new Ray2D(rayPosition, transform.up * -1);
		//���C��ԐF�ŕ\��������
		Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

		////���C�����������Ƃ�
		//if (Physics2D.Raycast())
		//{
		//	if (hit.collider.tag == "Ground")
		//	{
		//		Debug.Log("���C�������Ă܂�");
		//		jump_count = 0;
		//	}
		//}

		//�W�����v����
		if (Input.GetKeyDown(KeyCode.Space) && jump_count < 2)
		{
			Debug.Log("�W�����v���͂��ꂽ");
			rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

			//�J�E���g����
			jump_count++;
		}

		//��U��
		if (Input.GetMouseButtonDown(0))
		{
			switch(now_move)
            {
				case (int)Direction.LEFT:
					Debug.Log("����L�b�N�I");
					break;
				case (int)Direction.STOP:
					Debug.Log("��p���`�I");
					break;
				case (int)Direction.RIGHT:
					Debug.Log("�E��L�b�N�I");
					break;
			}
		}

		//���U��
		if (Input.GetMouseButtonDown(1))
		{
			switch (now_move)
			{
				case (int)Direction.LEFT:
					Debug.Log("�����L�b�N�I");
					break;
				case (int)Direction.STOP:
					Debug.Log("���p���`�I");
					break;
				case (int)Direction.RIGHT:
					Debug.Log("�E���L�b�N�I");
					break;
			}
		}
	}

    void FixedUpdate()
	{
		//�d�͐ݒ�
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//�ړ�����
		if (Input.GetKey(KeyCode.A))
		{
			now_move = (int)Direction.LEFT;
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
			if (rb2D.velocity.x > -LimitSpeed)
			{
				rb2D.AddForce(-transform.right * (MoveSpeed), ForceMode2D.Force);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			now_move = (int)Direction.RIGHT;
			//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
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
			Debug.Log("���n�����I");
			jump_count = 0;
		}
	}
}