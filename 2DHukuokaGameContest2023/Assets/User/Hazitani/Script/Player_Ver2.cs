using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ver2 : BaseStatusClass
{
	[SerializeField, Header("�W�����v��"), Range(0, 100)]
	private float JumpPower;

	[SerializeField, Header("�ړ����x"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("�ō����x"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("�d��"), Range(0, 100)]
	private float Gravity;

	[SerializeField, Header("�������"), Range(0, 100)]
	private float AvoidDis;

	[SerializeField, Header("�������"), Range(0, 100)]
	private int AvoidTime;

	[SerializeField, Header("�G�ƏՓ˂������̃m�b�N�o�b�N")]
	private Vector2 KnockbackPow;

	[SerializeField, Header("���g����ǂ̈ʒu�ɍU�������ݒ肷�邩")]
	private Vector3 AttackPos;

	[SerializeField, Header("�����蔻���p���`")]
	private GameObject AtkColWeekPunch;

	[SerializeField, Header("�����蔻���L�b�N")]
	private GameObject AtkColWeekKick;

	[SerializeField, Header("�����蔻�苭�p���`")]
	private GameObject AtkColStrongPunch;

	[SerializeField, Header("�����蔻�苭�L�b�N")]
	private GameObject AtkColStrongKick;


	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private enum LastAttack
    {
		NONE,	//�������Ă��Ȃ�
		WEAK,	//��U��
		STRONG,	//���U��
		STRONG2,//���U��2���
	}

	private Rigidbody2D rb2D;
	private int jump_count = 0;			//�W�����v��
	private bool ground_hit = false;	//�n�ʂɗ����Ă��邩
	private int now_move = 0;			//��:-1�E��~:0�E�E:1
	private bool player_frip = false;	//�v���C���[�̌���true�Efalse��
	private int last_attack = 0;        //�Ō�̍U���i�R���{���Ȃ��邩�m�F�p�j
	private float gap_time = 0;			//�U����̌㌄�̎���
	private bool avoiding = false;      //��𒆂��ǂ���
	private float avoid_time = 0;		//�������

	private Ray2D ray_left, ray_right;			//��΂����C
	private float distance = 2.0f;				//���C���΂�����
	private RaycastHit2D hit_left,hit_right;	//���C�������ɓ����������̏��
	private Vector3 rayPosition1, rayPosition2;	//���C�𔭎˂���ʒu
	private GameObject SearchGameObject = null; //���C�ɐG�ꂽ�I�u�W�F�N�g�擾�p
	
	//���C�̐ڒn����
	private void RayGround(Ray2D ray, RaycastHit2D hit, Vector3 vec)
    {
		//���C�����ɔ�΂�
		ray = new Ray2D(vec, -transform.up);

		//Ground�Ƃ����Փ˂���
		int layerMask = LayerMask.GetMask(new string[] { "Ground" });
		hit = Physics2D.Raycast(ray.origin, ray.direction, distance, layerMask);

		//���C��ԐF�ŕ\��������
		Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

		//�R���C�_�[�ƃ��C���ڐG
		if (hit.collider)
		{
			SearchGameObject = hit.collider.gameObject;

			if (SearchGameObject.tag == "Ground" && ground_hit == true && jump_count > 0)
			{
				Debug.Log("���n���Ă�I");
				jump_count = 0;
			}
		}
	}

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//���C�𔭎˂���ʒu�̒���
		rayPosition1 = this.transform.position + new Vector3(-0.5f, -transform.localScale.y / 2, 0.0f);
		rayPosition2 = this.transform.position + new Vector3( 0.5f, -transform.localScale.y / 2, 0.0f);

		//���C�̐ڒn����
		RayGround(ray_left, hit_left, rayPosition1);
		RayGround(ray_right, hit_right, rayPosition2);

        //�W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && jump_count < 1)
		{
			Debug.Log("�W�����v���͂��ꂽ");
			rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

			//�J�E���g����
			jump_count++;
		}

		//�U���̌����ݒ�
		Vector3 attackfrip = transform.position;//�U���ʒu�̍��W�X�V�p
		if (player_frip)
			attackfrip += AttackPos;//�E
		else
			attackfrip -= AttackPos;//��

		//��U��
		if (Input.GetMouseButtonDown(0))
		{
			switch(now_move)
            {
				case (int)Direction.LEFT:
					Debug.Log("����L�b�N�I");
					GameObject week_left_kick = Instantiate(AtkColWeekKick, attackfrip, Quaternion.identity);
					week_left_kick.transform.parent = gameObject.transform;
					week_left_kick.GetComponent<AttckCollision>().Damage = ATK;
					break;
				case (int)Direction.STOP:
					Debug.Log("��p���`�I");
					GameObject week_punch = Instantiate(AtkColWeekPunch, attackfrip, Quaternion.identity);
					week_punch.transform.parent = gameObject.transform;
					week_punch.GetComponent<AttckCollision>().Damage = ATK;
					break;
				case (int)Direction.RIGHT:
					Debug.Log("�E��L�b�N�I");
					GameObject week_right_kick = Instantiate(AtkColWeekKick, attackfrip, Quaternion.identity);
					week_right_kick.transform.parent = gameObject.transform;
					week_right_kick.GetComponent<AttckCollision>().Damage = ATK;
					break;
			}
			last_attack = (int)LastAttack.WEAK;
			gap_time = 20;
		}

		//���U��
		if (Input.GetMouseButtonDown(1))
		{
			if (last_attack != (int)LastAttack.STRONG2)
			{
				switch (now_move)
				{
					case (int)Direction.LEFT:
						Debug.Log("�����L�b�N�I");
						GameObject strong_left_kick = Instantiate(AtkColStrongKick, attackfrip, Quaternion.identity);
						strong_left_kick.transform.parent = gameObject.transform;
						strong_left_kick.GetComponent<AttckCollision>().Damage = ATK;
						break;
					case (int)Direction.STOP:
						Debug.Log("���p���`�I");
						GameObject strong_punch = Instantiate(AtkColStrongPunch, attackfrip, Quaternion.identity);
						strong_punch.transform.parent = gameObject.transform;
						strong_punch.GetComponent<AttckCollision>().Damage = ATK;
						break;
					case (int)Direction.RIGHT:
						Debug.Log("�E���L�b�N�I");
						GameObject strong_right_kick = Instantiate(AtkColStrongKick, attackfrip, Quaternion.identity);
						strong_right_kick.transform.parent = gameObject.transform;
						strong_right_kick.GetComponent<AttckCollision>().Damage = ATK;
						break;
				}

				//�Ō�̍U�����u�������Ă��Ȃ��v���u��U���v�̂Ƃ��u���U���v�ɂ���
				if (last_attack == (int)LastAttack.NONE || last_attack == (int)LastAttack.WEAK)
				{
					last_attack = (int)LastAttack.STRONG;
					gap_time = 30;
				}
				//�u���U���v�̂Ƃ��u���U��2��ځv�ɂ���
				else if(last_attack == (int)LastAttack.STRONG)
				{
					last_attack = (int)LastAttack.STRONG2;
					gap_time = 60;
				}
			}
		}

		//���
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl))
        {
			if (!avoiding)
			{
				avoiding = true;
				if (Input.GetKey(KeyCode.A))
				{
					Debug.Log("�����");
					rb2D.velocity = -transform.right * AvoidDis;
				}
				else if (Input.GetKey(KeyCode.D))
				{
					Debug.Log("�E���");
					rb2D.velocity = transform.right * AvoidDis;
				}
				else
				{
					Debug.Log("���̏���");
				}
			}
        }
	}

    void FixedUpdate()
	{
		//�d�͐ݒ�
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//�ړ�����
		//��𒆂ł͂Ȃ��Ƃ�
		if (!avoiding)
		{
			if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				now_move = (int)Direction.LEFT;
				player_frip = false;//������
				//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
				if (rb2D.velocity.x > -LimitSpeed)
				{
					rb2D.AddForce(-transform.right * (MoveSpeed), ForceMode2D.Force);
				}
			}
			if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
			{
				now_move = (int)Direction.RIGHT;
				player_frip = true;//�E����
				//�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
				if (rb2D.velocity.x < LimitSpeed)
				{
					rb2D.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
				}
			}
			if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				now_move = (int)Direction.STOP;
			}
		}
		else
        {
			//�������
			avoid_time++;

			if(avoid_time >= AvoidTime)
            {
				avoiding = false;
				avoid_time = 0;
			}
		}

		//�U��������
		if(last_attack != (int)LastAttack.NONE && gap_time >= 0)
        {
			gap_time--;

			if(gap_time <= 0)
            {
				last_attack = (int)LastAttack.NONE;
				gap_time = 0;
			}
		}
	}
	
	//�R���C�_�[�ɐG�ꂽ��
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
			ground_hit = true;
		}

		//��l���ƏՓˎ��̃m�b�N�o�b�N
		if (other.gameObject.tag == "Enemy")
		{
			if (!player_frip)
			{
				rb2D.AddForce(new Vector2(-KnockbackPow.x, KnockbackPow.y), ForceMode2D.Force);
			}
			else
			{
				rb2D.AddForce(KnockbackPow, ForceMode2D.Force);
			}
		}
	}

	//�R���C�_�[���痣�ꂽ��
    private void OnCollisionExit2D(Collision2D other)
    {
		if (other.gameObject.CompareTag("Ground"))
		{
			ground_hit = false;
		}
	}
}