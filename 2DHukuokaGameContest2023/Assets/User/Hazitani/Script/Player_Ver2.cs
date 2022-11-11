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

	[SerializeField, Header("�U�������蔻��")]
	private GameObject AttackCollider;

	[SerializeField, Header("�U���̃N�[���^�C��")]
	private int AttackCoolTime;

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

	private Rigidbody2D rb2D;				//��l���̃��W�b�g�{�f�B
	private int jump_count = 0;				//�W�����v��
	private bool jump_key_flag = false;		//�W�����v�L�[�A�����萧��p
	private bool ground_hit = false;		//�n�ʂɗ����Ă��邩
	private int now_move = 0;				//��:-1�E��~:0�E�E:1
	private bool player_frip = false;		//�v���C���[�̌���true�Efalse��
	private bool move_stop = false;			//�������~�߂����Ƃ��g�p
	private int combo_count = 0;            //�R���{��
	private int score = 0;					//�X�R�A


	//�U���֘A
	private Vector3 mousePos;				//�}�E�X�̈ʒu�擾�p
	private Vector3 target;					//�U���ʒu�����p
	private Quaternion atkQuaternion;		//�U���p�x
	private bool attack_ok = true;			//�U���o���邩�ǂ����o����Ƃ�true
	private bool attacking = false;			//�U����true
	private bool dont_move = false;			//�G�ɂ߂荞�񂾎��ɓG�̌�����1����p
	private GameObject attack = null;       //�U���I�u�W�F�N�g
	private int attack_cooltime = 0;		//�U���N�[���^�C��
	[System.NonSerialized]
	public Vector3 hit_enemy_pos;			//�U�������������G�̈ʒu
	[System.NonSerialized]
	public bool hit_enemy = false;			//�U�����G�ɓ����������ǂ���
	private bool hit_enemy_frip = false;    //�U�������G�̕���true�Efalse��
	public BaseEnemyFly enemyObj = null;


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
		//�U���̌����ݒ�
		Vector3 attackpos = transform.position;//�U���ʒu�̍��W�X�V�p

		//�U��
		if (Input.GetMouseButtonDown(0))
		{
			if (attack_ok)
			{
				attack_ok = false;
				attacking = true;

				//�p�x�ݒ�
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				target = Vector3.Scale((mousePos - transform.position), new Vector3(0, 0, 0)).normalized;
				atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

				//�R���C�_�[�𐶐�
				PlayerAttack(attack, AttackCollider, attackpos);
			}
		}
	}

	void FixedUpdate()
	{
		//�d�͐ݒ�
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//���C�𔭎˂���ʒu�̒���
		rayPosition1 = transform.position + new Vector3(-0.5f, -transform.localScale.y / 2, 0.0f);
		rayPosition2 = transform.position + new Vector3(0.5f, -transform.localScale.y / 2, 0.0f);

		//���C�̐ڒn����
		RayGround(ray_left, hit_left, rayPosition1);
		RayGround(ray_right, hit_right, rayPosition2);

		//�ړ�����
		if (!move_stop)
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

		//�W�����v����
		if (Input.GetKey(KeyCode.Space) && jump_count < 1)
		{
			if(!jump_key_flag)
            {
				jump_key_flag = true;
				Debug.Log("�W�����v���͂��ꂽ");
				rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

				//�J�E���g����
				jump_count++;
			}
		}
		else
        {
			jump_key_flag = false;
		}

		//�U�����G�ɓ��������ꍇ
		if (hit_enemy)
		{
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
			else if (transform.position.x > hit_enemy_pos.x)
			{
				if (!dont_move)
				{
					dont_move = true;
					hit_enemy_frip = false;
				}
				transform.position = Vector3.MoveTowards(transform.position, hit_enemy_pos + AttackMovePos, AttackMoveSpeed);
			}
		}

		//�U���N�[���^�C��
		if (!attack_ok)
        {
			attack_cooltime++;

			if(attack_cooltime >= AttackCoolTime)
            {
				attack_cooltime = 0;
				attack_ok = true;
            }
        }
	}

	//�R���C�_�[�ɐG�ꂽ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
		//�n�ʂɐG�ꂽ��
        if (collision.gameObject.CompareTag("Ground"))
        {
			ground_hit = true;
			move_stop = false;

			//�R���{���Z�b�g���Ĕ��f
			combo_count = 0;
			Combo.text = combo_count.ToString();
		}

		//�U���̃m�b�N�o�b�N
		if (collision.gameObject.tag == "Enemy")
		{
			//�W�����v�񐔃��Z�b�g
			jump_count = 0;

			//�R���{���₵�Ĕ��f
			combo_count++;
			Combo.text = combo_count.ToString();

			//�G��HP���炷
			enemyObj.HP -= ATK;

			//HP��0�̎�
			if(enemyObj.HP <= 0)
            {
				//�X�R�A���Z
				score++;

				//�q�b�g�X�g�b�v�̏���
			}

			AttackFin();
		}
	}

    //�R���C�_�[�ɐG��Ă����
    private void OnCollisionStay2D(Collision2D collision)
    {
		//�n�ʂɐG�ꂽ��
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_hit = true;
			move_stop = false;

			//�R���{���Z�b�g���Ĕ��f
			combo_count = 0;
			Combo.text = combo_count.ToString();
		}

		if (collision.gameObject.tag == "Enemy")
		{
			AttackFin();
		}
	}

    //�R���C�_�[���痣�ꂽ��
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_hit = false;
		}

		if (collision.gameObject.tag == "Enemy")
		{
			AttackFin();
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

	//�U������
	private void AttackFin()
    {
		if (attacking)
		{
			hit_enemy = false;
			rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			dont_move = false;
			attacking = false;
			if (hit_enemy_frip)
			{
				rb2D.AddForce(new Vector2(-SubjugationKnockback.x, SubjugationKnockback.y), ForceMode2D.Force);
			}
			else
			{
				rb2D.AddForce(SubjugationKnockback, ForceMode2D.Force);
			}
		}
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