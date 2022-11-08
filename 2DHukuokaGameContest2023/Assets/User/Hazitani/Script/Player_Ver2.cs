using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

	[SerializeField, Header("�U���𓖂Ă����̕��V����")]
	private int AttackingTime;

	[SerializeField, Header("�G��|�������Ƃ̔�ԗ�")]
	private Vector2 SubjugationKnockback;

	[SerializeField, Header("�R���{�e�L�X�g")]
	private Text Combo;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private Rigidbody2D rb2D;			//��l���̃��W�b�g�{�f�B
	private int jump_count = 0;			//�W�����v��
	private bool ground_hit = false;	//�n�ʂɗ����Ă��邩
	private int now_move = 0;			//��:-1�E��~:0�E�E:1
	private bool player_frip = false;   //�v���C���[�̌���true�Efalse��
	private bool move_stop = false;     //�������~�߂����Ƃ��g�p
	private bool avoiding = false;      //��𒆂��ǂ���
	private float avoid_time = 0;       //�������
	private int combo_count = 0;		//�R���{��


	//�U���֘A
	private Vector3 mousePos;				//�}�E�X�̈ʒu�擾�p
	private Vector3 target;					//�U���ʒu�����p
	private Quaternion atkQuaternion;		//�U���p�x
	private bool attacking = false;			//�U�������ǂ���
	private int attacking_time = 0;         //�U�����ɕ��V�ł��鎞��
	private bool dont_move = false;
	private GameObject attack = null;		//�U���I�u�W�F�N�g
	[System.NonSerialized]
	public Vector3 hit_enemy_pos;			//�U�������������G�̈ʒu
	[System.NonSerialized]
	public bool hit_enemy = false;			//�U�����G�ɓ����������ǂ���
	private bool hit_enemy_frip = false;	//�U�������G�̕���true�Efalse��
	[System.NonSerialized]
	public bool enemy_alive = true;			//�U�������G�������Ă��邩
	private Ray2D ray_attack;				//�U�����ɔ�΂����C
	private float ray_attack_distance = 10;	//�U�����C�̋���
	private RaycastHit2D hit_attack;		//�U�����C�������ɓ����������̏��


	//�ڒn�֘A
	private Ray2D ray_left, ray_right;			//��΂����C
	private float distance = 2.0f;				//���C���΂�����
	private RaycastHit2D hit_left,hit_right;	//���C�������ɓ����������̏��
	private Vector3 rayPosition1, rayPosition2;	//���C�𔭎˂���ʒu
	private GameObject SearchGameObject = null; //���C�ɐG�ꂽ�I�u�W�F�N�g�擾�p


	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
		Combo.text = combo_count.ToString();
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
			//�p�x�ݒ�
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target = Vector3.Scale((mousePos - transform.position), new Vector3(1, 1, 0)).normalized;
			atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

			////���C���}�E�X�����ɔ�΂�
			//ray_attack = new Ray2D(transform.position, target);

			////Enemy�Ƃ����Փ˂���
			//int layerMask_attack = LayerMask.GetMask(new string[] { "Enemy" });
			//hit_attack = Physics2D.Raycast(ray_attack.origin, ray_attack.direction, ray_attack_distance, layerMask_attack);

			////���C��F�ŕ\��������
			//Debug.DrawRay(ray_attack.origin, ray_attack.direction * ray_attack_distance, Color.blue);

			////�R���C�_�[�ƃ��C���ڐG
			//if (hit_attack.collider)
			//{
			//	SearchGameObject = hit_attack.collider.gameObject;

			//	if (SearchGameObject.tag == "Enemy")
			//	{
			//		Debug.Log(SearchGameObject.name.ToString());
			//	}
			//}

			//�R���C�_�[�𐶐�
			PlayerAttack(attack, AttackCollider, attackpos);
		}

		//�U�����G�ɓ��������ꍇ
		if (hit_enemy)
        {
			//�ړ��L�[�󂯂Ȃ�
			move_stop = true;

			rb2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

			if (transform.position.x < hit_enemy_pos.x)
			{
				if (!dont_move)
				{
					dont_move = true;
					hit_enemy_frip = true;
				}
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos - AttackMovePos, AttackMoveSpeed);
			}
			else
			{
				if (!dont_move)
				{
					dont_move = true;
					hit_enemy_frip = false;
				}
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos + AttackMovePos, AttackMoveSpeed);
			}

			if (transform.position == hit_enemy_pos + AttackMovePos || transform.position == hit_enemy_pos - AttackMovePos)
			{
				attacking = true;
			}
		}

		//���
		//if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl))
  //      {
		//	//��𒆂ł͂Ȃ�
		//	if (!avoiding)
		//	{
		//		avoiding = true;
		//		if (Input.GetKey(KeyCode.A))
		//		{
		//			Debug.Log("�����");
		//			rb2D.velocity = -transform.right * AvoidDis;
		//		}
		//		else if (Input.GetKey(KeyCode.D))
		//		{
		//			Debug.Log("�E���");
		//			rb2D.velocity = transform.right * AvoidDis;
		//		}
		//		else
		//		{
		//			Debug.Log("���̏���");
		//		}
		//	}
  //      }
	}

	void FixedUpdate()
	{
		if (!attacking)
		{
			//�d�͐ݒ�
			Physics2D.gravity = new Vector3(0, -Gravity, 0);
		}

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

		//�U����
		if (attacking)
		{
			attacking_time++;

			if (attacking_time >= AttackingTime)
			{
				attacking_time = 0;
				attacking = false;
				hit_enemy = false;
				rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			}

			//�G������
			if (!enemy_alive)
			{
				move_stop = false;
				attacking_time = 0;
				attacking = false;
				hit_enemy = false;
				rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
				if (hit_enemy_frip)
				{
					rb2D.AddForce(new Vector2(-SubjugationKnockback.x, SubjugationKnockback.y), ForceMode2D.Force);
				}
				else
				{
					rb2D.AddForce(SubjugationKnockback, ForceMode2D.Force);
				}
				dont_move = false;
			}
		}
	}

	//�R���C�_�[�ɐG�ꂽ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
			ground_hit = true;
			move_stop = false;
			combo_count = 0;
			Combo.text = combo_count.ToString();
		}

		//��l���ƏՓˎ��̃m�b�N�o�b�N
		if (collision.gameObject.tag == "Enemy")
		{
			jump_count = 0;

			combo_count++;
			Combo.text = combo_count.ToString();

			//if (player_frip)
			//{
			//	rb2D.AddForce(new Vector2(-KnockbackPow.x, KnockbackPow.y), ForceMode2D.Force);
			//}
			//else
			//{
			//	rb2D.AddForce(KnockbackPow, ForceMode2D.Force);
			//}
			//move_stop = true;
		}
	}

	//�R���C�_�[���痣�ꂽ��
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_hit = false;
		}

		//��l���ƏՓˎ��̃m�b�N�o�b�N
		if (collision.gameObject.tag == "Enemy")
		{
			if (collision.gameObject.GetComponent<BaseEnemyFly>().deth)
			{
				enemy_alive = false;
			}
			else
			{
				enemy_alive = true;
			}
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