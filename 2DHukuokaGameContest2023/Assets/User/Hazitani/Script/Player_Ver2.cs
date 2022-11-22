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

	[SerializeField, Header("�G�ɓ����������Ƃ̔�ԗ�")]
	private Vector2 SubjugationKnockback;

	[SerializeField, Header("�U�����Ɍ�����]�����鑬��")]
	private int AttackRotationSpeed;

	[SerializeField, Header("�R���{�e�L�X�g")]
	private Text Combo;

	[SerializeField, Header("�ő�R���{�e�L�X�g")]
	private Text ComboMax;

	[SerializeField, Header("�X�R�A�e�L�X�g")]
	private Text Score;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}


	//��l���֘A
	private Rigidbody2D rb2D;				//��l���̃��W�b�g�{�f�B
	private int jump_count = 0;             //�W�����v��
	private bool jump_key_flag = false;     //�W�����v�L�[�A�����萧��p
	private bool ground_on = false;			//�n�ʂɗ����Ă��邩
	private int now_move = 0;				//��:-1�E��~:0�E�E:1
	private bool player_frip = false;		//�v���C���[�̌���true�Efalse��
	private bool move_stop = false;         //�������~�߂����Ƃ��g�p


	//�V�X�e���֘A
	private int combo_count = 0;            //�R���{��
	private int combo_max = 0;              //�ő�R���{��
	private int score = 0;                  //�X�R�A
	private int score_add = 0;              //���ꂩ����Z�����X�R�A
	private SpawnEnemy spawn_enemy;         //�X�|�[���G�l�~�[�擾�p


	//�U���֘A
	private Vector3 mousePos;				//�}�E�X�̈ʒu�擾�p
	private Vector3 target;					//�U���ʒu�����p
	private Quaternion atkQuaternion;		//�U���p�x
	private bool attack_ok = true;			//�U���o���邩�ǂ����o����Ƃ�true
	private bool attacking = false;			//�U����true
	private bool dont_move = false;         //�G�̌�����1����p
	private Ray2D attack_ray;				//��΂����C
	private float attack_distance = 10.0f;  //���C���΂�����
	private RaycastHit2D attack_hit;		//���C�������ɓ����������̏��
	private Vector3 attack_rayPosition;		//���C�𔭎˂���ʒu
	private GameObject attack = null;       //�U���I�u�W�F�N�g
	private int attack_cooltime = 0;        //�U���N�[���^�C��
	private int attack_rotation = 0;        //�U�����̌���]
	[System.NonSerialized]
	public bool attack_col = false;			//�A�^�b�N�R���C�_�[�o����true
	[System.NonSerialized]
	public Vector3 hit_enemy_pos;			//�U�������������G�̈ʒu
	[System.NonSerialized]
	public bool hit_enemy = false;			//�U�����G�ɓ����������ǂ���
	private bool hit_enemy_frip = false;    //�U�������G�̕���true�Efalse��
	[System.NonSerialized]
	public BaseEnemyFly enemyObj = null;	//�U���ɓ��������G�̃I�u�W�F�N�g


	//�ڒn�֘A
	private Ray2D ground_ray;					//��΂����C
	private float distance = 2.0f;				//���C���΂�����
	private RaycastHit2D ground_hit;			//���C�������ɓ����������̏��
	private Vector3 ground_rayPosition;			//���C�𔭎˂���ʒu
	private GameObject SearchGameObject = null; //���C�ɐG�ꂽ�I�u�W�F�N�g�擾�p


	void Start()
	{
		//���W�b�g�{�f�B�o�^
		rb2D = GetComponent<Rigidbody2D>();

		//�}�l�[�W���[�ɓo�^
		ManagerAccessor.Instance.player = this;

		//�e�L�X�g������
		Combo.text = combo_count.ToString();
		ComboMax.text = combo_max.ToString();
		Score.text = score.ToString();

		//�X�|�[���G�l�~�[���������ēo�^
		spawn_enemy = GameObject.Find("SpawnEnemy").GetComponent<SpawnEnemy>();
	}

	void Update()
    {
		//�U���̌����ݒ�
		Vector3 attackpos = transform.position;//�U���ʒu�̍��W�X�V�p

		//�p�x�ݒ�
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target = Vector3.Scale((mousePos - transform.position), new Vector3(0, 0, 0)).normalized;
		atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

		//�U��
		if (Input.GetMouseButtonDown(0))
		{
			if (attack_ok)
			{
				attack_ok = false;
				attacking = true;

				//�R���C�_�[�𐶐�
				PlayerAttack(attack, AttackCollider, attackpos);

				////���C�𔭎˂���ʒu�̒���
				//attack_rayPosition = transform.position;

				////���C���΂�
				//attack_ray = new Ray2D(attack_rayPosition, atkQuaternion.eulerAngles);
				//Debug.Log(atkQuaternion.eulerAngles.normalized);

				////Enemy�Ƃ����Փ˂���
				//int attack_layerMask = LayerMask.GetMask(new string[] { "Enemy" });
				//attack_hit = Physics2D.Raycast(attack_ray.origin, attack_ray.direction, attack_distance, attack_layerMask);

				////���C�����F�ŕ\��������
				//Debug.DrawRay(attack_ray.origin, attack_ray.direction * attack_distance, Color.yellow);

				////�R���C�_�[�ƃ��C���ڐG
				//if (attack_hit.collider)
				//{
				//	if (attack_hit.collider.tag == "Enemy")
				//	{
				//		enemyObj = attack_hit.collider.gameObject.GetComponent<BaseEnemyFly>();
				//		hit_enemy_pos = enemyObj.transform.position;
				//		hit_enemy = true;
				//	}
				//}
			}
		}
	}

	void FixedUpdate()
	{
		//�d�͐ݒ�
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//���C�𔭎˂���ʒu�̒���
		ground_rayPosition = transform.position + new Vector3(0.0f, -transform.localScale.y / 2, 0.0f);

		//���C�̐ڒn����
		RayGround(ground_ray, ground_hit, ground_rayPosition);

		if(attack_col)
        {
			//���̉�]
			transform.GetChild(0).gameObject.transform.rotation = atkQuaternion * Quaternion.Euler(0, 0, 90);
		}
		else
        {
			transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

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
			if (!jump_key_flag)
			{
				jump_key_flag = true;
				move_stop = false;
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
			move_stop = true;
			rb2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

			if (enemyObj != null)
			{
				hit_enemy_pos = enemyObj.transform.position;
			}

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

			if(transform.position == hit_enemy_pos)
            {
				AttackFin();
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
			ground_on = true;
			move_stop = false;

			//�R���{���Z�b�g���Ĕ��f
			if (spawn_enemy.NowWave	!= 1)
			{
				combo_count = 0;
				Combo.text = combo_count.ToString();
			}
		}

		//�U���̃m�b�N�o�b�N
		if (collision.gameObject.tag == "Enemy")
		{
			if (attacking)
				Attack();
		}
	}

    //�R���C�_�[�ɐG��Ă����
    private void OnCollisionStay2D(Collision2D collision)
    {
		//�n�ʂɐG�ꂽ��
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = true;
			move_stop = false;

			//�R���{���Z�b�g���Ĕ��f
			combo_count = 0;
			Combo.text = combo_count.ToString();
		}

		if (collision.gameObject.tag == "Enemy")
		{
			if(attacking)
				Attack();
		}
	}

    //�R���C�_�[���痣�ꂽ��
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = false;
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

			if (SearchGameObject.tag == "Ground" && ground_on == true && jump_count > 0)
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
		//�W�����v�񐔃��Z�b�g
		jump_count = 0;

		hit_enemy = false;
		rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		dont_move = false;
		attacking = false;

		//�U���㒵�˕Ԃ�
		if (hit_enemy_frip)
		{
			rb2D.AddForce(new Vector2(-SubjugationKnockback.x, SubjugationKnockback.y), ForceMode2D.Impulse);
		}
		else
		{
			rb2D.AddForce(SubjugationKnockback, ForceMode2D.Impulse);
		}
	}

	//�X�R�A���Z
	private int ScoreSetting(int score, int combo)
    {
		//�R���{�ɂ���ăX�R�A���Z
		if (combo >= 1 && combo < 50)
			score += 1000;
		else if (combo >= 50 && combo < 100)
			score += 5000;
		else if (combo >= 100)
			score += 10000;

		/*
		�R���{�{�[�i�X
			 10�R���{���Ƃ�  5000�_
			 50�R���{���Ƃ� 10000�_
			100�R���{���Ƃ�100000�_
		*/
		if (combo % 100 == 0)
		{
			score += 100000;
		}
		else if (combo % 50 == 0)
		{
			score += 10000;
		}
		else if (combo % 10 == 0)
		{
			score += 5000;
		}

		return score;
	}

	private void Attack()
    {
		//�R���{���₵�Ĕ��f
		combo_count++;
		Combo.text = combo_count.ToString();

		//�}�b�N�X�R���{�ύX
		if (combo_max < combo_count)
		{
			combo_max = combo_count;
			ComboMax.text = combo_max.ToString();
		}

		//���݂̃E�F�[�u���ʏ�E�F�[�u�̂Ƃ�
		if (spawn_enemy.NowWave == 0)
		{
			spawn_enemy.WaveCombo++;
			//���ꂩ����Z�����X�R�A�����߂�
			score_add = ScoreSetting(score_add, combo_count);
			spawn_enemy.WaveScore += score_add;
		}

		if (enemyObj != null)
		{
			//�G��HP���炷
			enemyObj.HP -= ATK;

			//�\���p�̃X�R�A���߂�
			score = ScoreSetting(score, combo_count);
			Score.text = score.ToString();

			//HP��0�̎�
			//if (enemyObj.HP <= 0)
			//{
			//    //�q�b�g�X�g�b�v�̏���
			//}
		}

		score_add = 0;
		AttackFin();
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

/* ��邱��
 * ��l���̍U���I�u�W�F�N�g�����C�ɕϊ�
 * ��l������J�[�\����\��
 * �G�ɍU����������ʉ��u�Y�V���A�I�I�v
*/

/* �o�O
 * ��̓G������
 * �����W�����v����
 * �U�����X�J���Ă���G�ɓ�����ƃR���{������
 * Wave�؂�ւ��^�C�~���O�ōU������Ǝ~�܂�
*/