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
	private bool ground_hit = false;
	private int now_move = 0;//��:-1�E��~:0�E�E:1

	private Ray2D ray;				//��΂����C
	private float distance = 2.0f;	//���C���΂�����
	private RaycastHit2D hit;		//���C�������ɓ����������̏��
	private Vector3 rayPosition;    //���C�𔭎˂���ʒu
	private GameObject SearchGameObject = null;//���C�ɐG�ꂽ�I�u�W�F�N�g�擾�p

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//���C�𔭎˂���ʒu�̒���
		rayPosition = this.transform.position + new Vector3(0.0f, -0.5f, 0.0f); ;
		//���C�����ɔ�΂�
		ray = new Ray2D(rayPosition, -transform.up);

		//Ground�Ƃ����Փ˂���
		int layerMask = LayerMask.GetMask(new string[] { "Ground" });
		hit = Physics2D.Raycast(ray.origin, ray.direction, distance, layerMask);
		
		//���C��ԐF�ŕ\��������
		Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

		if (hit.collider)
		{
			SearchGameObject = hit.collider.gameObject;

			if (SearchGameObject.tag == "Ground" && ground_hit == true && jump_count > 0)
			{
				Debug.Log("���n���Ă�I");
				jump_count = 0;
			}
		}

        //�W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && jump_count < 1)
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
			ground_hit = true;
		}
    }

    private void OnCollisionExit2D(Collision2D other)
    {
		if (other.gameObject.CompareTag("Ground"))
		{
			ground_hit = false;
		}
	}
}