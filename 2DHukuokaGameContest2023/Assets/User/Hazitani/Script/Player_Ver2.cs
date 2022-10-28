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

	[SerializeField, Header("�U�������蔻��")]
	private GameObject AttackCollider;

	[SerializeField, Header("�U���𓖂Ă����̈ڑ��ʒu")]
	private Vector3 AttackMovePos;

	[SerializeField, Header("�U���𓖂Ă����̈ڑ����x")]
	private float AttackMoveSpeed;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private Rigidbody2D rb2D;
	private int jump_count = 0;			//�W�����v��
	private bool ground_hit = false;	//�n�ʂɗ����Ă��邩
	private int now_move = 0;			//��:-1�E��~:0�E�E:1
	private bool player_frip = false;   //�v���C���[�̌���true�Efalse��
	private bool move_stop = false;     //�������~�߂����Ƃ��g�p
	private bool avoiding = false;      //��𒆂��ǂ���
	private float avoid_time = 0;       //�������

	//�U���֘A
	private Vector3 mousePos;           //�}�E�X�̈ʒu�擾�p
	private Vector3 target;             //�U���ʒu�����p
	private Quaternion atkQuaternion;   //�U���p�x
	private bool attacking = false;		//�U�������ǂ���
	private GameObject attack = null;   //�U���I�u�W�F�N�g
	[System.NonSerialized]//����t������p�u���b�N�ł��C���X�y�N�^�[�ɏo�Ă���I�I�I�I�I
	public Vector3 hit_enemy_pos;       //�U�������������G�̈ʒu
	[System.NonSerialized]
	public bool hit_enemy = false;		//�U�����G�ɓ����������ǂ���

	//���C�֘A
	private Ray2D ray_left, ray_right;			//��΂����C
	private float distance = 2.0f;				//���C���΂�����
	private RaycastHit2D hit_left,hit_right;	//���C�������ɓ����������̏��
	private Vector3 rayPosition1, rayPosition2;	//���C�𔭎˂���ʒu
	private GameObject SearchGameObject = null; //���C�ɐG�ꂽ�I�u�W�F�N�g�擾�p

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//���C�𔭎˂���ʒu�̒���
		rayPosition1 = transform.position + new Vector3(-0.5f, -transform.localScale.y / 2, 0.0f);
		rayPosition2 = transform.position + new Vector3( 0.5f, -transform.localScale.y / 2, 0.0f);

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
		Vector3 attackpos = transform.position;//�U���ʒu�̍��W�X�V�p

		//�U��
		if (Input.GetMouseButtonDown(0))
		{
			attacking = true;

			//�p�x�ݒ�
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target = Vector3.Scale((mousePos - transform.position), new Vector3(1, 1, 0)).normalized;
			atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

			Debug.Log("���������I");
			//�R���C�_�[�𐶐�
			PlayerAttack(attack, AttackCollider, attackpos);
		}

		//�U�����G�ɓ��������ꍇ
		if (hit_enemy)
        {
			Debug.Log("�U����������");

			if(transform.position.x < hit_enemy_pos.x)
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos - AttackMovePos, AttackMoveSpeed);
			else
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos + AttackMovePos, AttackMoveSpeed);

			if (transform.position == hit_enemy_pos + AttackMovePos || transform.position == hit_enemy_pos - AttackMovePos)
			{
				hit_enemy = false;
			}
		}

		//���
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl))
        {
			//��𒆂ł͂Ȃ�
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
		if (!move_stop)
		{
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

				if (avoid_time >= AvoidTime)
				{
					avoiding = false;
					avoid_time = 0;
				}
			}
		}

		////�U������
		//if (attacking)
		//{
		//	attacking = false;
		//	Vector3 force = new Vector3(0,1,0);//�͂̐ݒ�
		//	rb2D.AddForce(force, ForceMode2D.Impulse);
		//}�����r���I�I�I�I
	}

	//�R���C�_�[�ɐG�ꂽ��
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
			ground_hit = true;
			move_stop = false;
		}

		//��l���ƏՓˎ��̃m�b�N�o�b�N
		if (other.gameObject.tag == "Enemy")
		{
			if (player_frip)
			{
				rb2D.AddForce(new Vector2(-KnockbackPow.x, KnockbackPow.y), ForceMode2D.Force);
			}
			else
			{
				rb2D.AddForce(KnockbackPow, ForceMode2D.Force);
			}
			move_stop = true;
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

	//�U���I�u�W�F�N�g����
	private void PlayerAttack(GameObject attack, GameObject prefab, Vector3 attackpos)
    {
		attack = Instantiate(prefab, attackpos += target, atkQuaternion);
		attack.transform.parent = gameObject.transform;
		attack.GetComponentInChildren<AttckCollision>().Damage = ATK;
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