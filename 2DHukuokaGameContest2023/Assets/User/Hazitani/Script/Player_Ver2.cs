using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player_Ver2 : BaseStatusClass
{
	/// <summary>
	/// Player�I�u�W�F�N�g�̎q�I�u�W�F�N�g�̏���
	/// 
	/// Player
	///		PlayerSprite
	///			rainbow_aura
	///			aura
	///		Arrow
	///		
	/// ���̏��Ԃ���Ȃ���GetChild�֐����o�O��܂�
	/// </summary>

	[SerializeField, Header("�W�����v��"), Range(0, 100)]
	private float JumpPower;

	[SerializeField, Header("�ړ����x"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("�ō����x"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("�d��"), Range(0, 100)]
	private float Gravity;

	[SerializeField, Header("�����ō����x"), Range(0, 100)]
	private float FallSpeed;

	[SerializeField, Header("�U�����͂�����"), Range(0, 10)]
	private float AttackDistance;

	[SerializeField, Header("�U���̃N�[���^�C��")]
	private int AttackCoolTime;

	[SerializeField, Header("�U���𓖂Ă����̈ڑ��ʒu")]
	private Vector3 AttackMovePos;

	[SerializeField, Header("�U���𓖂Ă����̈ڑ����x")]
	private float AttackMoveSpeed;

	[SerializeField, Header("�G�ɓ����������Ƃ̔�ԗ�")]
	private Vector2 SubjugationKnockback;

	[SerializeField, Header("�q�b�g�X�g�b�v�̃t���[����"), Range(0, 100)]
	private int HitStopFrame;

	[SerializeField, Header("���̉�]�A�j���[�V����")]
	private Animator RotationAnimator;

	[SerializeField, Header("�J�[�\���̕\��")]
	private bool AttackCursor;

	//[SerializeField, Header("�t�B�[�o�[�^�C���̃e�L�X�g")]
	//private Text textFeverTime;

	[SerializeField, Header("�I�����W�I�[���o���܂ł̃R���{��"), Range(0, 100)]
	private int OrangeCombo;

	[SerializeField, Header("�t�B�[�o�[�^�C���܂ł̃R���{��"), Range(0, 100)]
	private int FeverCombo;

	[SerializeField, Header("�t�B�[�o�[�^�C���̎���"), Range(0, 100)]
	private int FeverTime;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private enum PrefabChild
    {
		PlayerSprite = 0,
			Rainbow_Aura = 0,
			Aura = 1,
		Arrow = 1,
			ArrowImage = 0,
	}


	//��l���֘A
	private Rigidbody2D rb2D;				//��l���̃��W�b�g�{�f�B
	private int jump_count = 0;             //�W�����v��
	private bool jump_key_flag = false;     //�W�����v�L�[�A�����萧��p
	private bool ground_on = false;			//�n�ʂɗ����Ă��邩
	private int now_move = 0;				//��:-1�E��~:0�E�E:1
	private bool player_frip = false;		//�v���C���[�̌���true�Efalse��
	private bool move_stop = false;         //�������~�߂����Ƃ��g�p
	private Ray2D cursor_ray;               //��΂����C
	private RaycastHit2D cursor_hit;        //���C�������ɓ����������̏��
	private Vector3 cursor_rayPosition;     //���C�𔭎˂���ʒu


	//�V�X�e���֘A
	private int combo_fever_count = 0;      //�t�B�[�o�[�^�C���ɓ��邽�߂ɕK�v�ȃR���{��
	[System.NonSerialized]
	public int time_fever = 0;              //�t�B�[�o�[�^�C���̎��Ԃ𐔂���p
	[System.NonSerialized]
	public int score_add = 0;               //���ꂩ����Z�����X�R�A
	[System.NonSerialized]
	public bool combo_reset = false;		//�R���{����������true


	//�U���֘A
	private Vector3 mousePos;				//�}�E�X�̈ʒu�擾�p
	private Vector3 target;					//�U���ʒu�����p
	private Quaternion atkQuaternion;		//�U���p�x
	private bool attack_ok = true;			//�U���o���邩�ǂ����o����Ƃ�true
	private bool dont_move = false;         //�G�̌�����1����p
	private Ray2D attack_ray;				//��΂����C
	private RaycastHit2D attack_hit;		//���C�������ɓ����������̏��
	private Vector3 attack_rayPosition;		//���C�𔭎˂���ʒu
	private int attack_cooltime = 0;        //�U���N�[���^�C��
	private bool hitstop_on = false;        //�q�b�g�X�g�b�v��true
	private int hitstop_frame = 0;			//�q�b�g�X�g�b�v�t���[���J�E���g�p
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

		//�e�L�X�g�̏�����
		//textFeverTime.text = "";
	}

	void Update()
    {
		if (!ManagerAccessor.Instance.systemManager.GameEnd)
		{
			//�p�x�ݒ�
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target = Vector3.Scale((mousePos - transform.position), new Vector3(0, 0, 0)).normalized;
			atkQuaternion = Quaternion.AngleAxis(GetAim(transform.position, mousePos), Vector3.forward);

			//�J�[�\���̐F�ύX
			transform.GetChild((int)PrefabChild.Arrow).GetChild((int)PrefabChild.ArrowImage).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 50);

			//�J�[�\���̃��C
			//���C�𔭎˂���ʒu�̒���
			cursor_rayPosition = transform.position;

			//���C���΂�
			cursor_ray = new Ray2D(cursor_rayPosition, mousePos - cursor_rayPosition);

			//Enemy�Ƃ����Փ˂���
			int attack_layerMask = LayerMask.GetMask(new string[] { "Enemy" });
			cursor_hit = Physics2D.Raycast(cursor_ray.origin, cursor_ray.direction, AttackDistance, attack_layerMask);

			//�R���C�_�[�ƃ��C���ڐG
			if (cursor_hit.collider)
			{
				if (cursor_hit.collider.tag == "Enemy")
				{
					//�J�[�\���̐F�ύX
					transform.GetChild((int)PrefabChild.Arrow).GetChild((int)PrefabChild.ArrowImage).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
				}
			}

			//�U��
			if (Input.GetMouseButtonDown(0))
			{
				if (attack_ok)
				{
					attack_ok = false;

					//���C�𔭎˂���ʒu�̒���
					attack_rayPosition = transform.position;

					//���C���΂�
					attack_ray = new Ray2D(attack_rayPosition, mousePos - attack_rayPosition);

					//Enemy�Ƃ����Փ˂���
					attack_hit = Physics2D.Raycast(attack_ray.origin, attack_ray.direction, AttackDistance, attack_layerMask);

					//���C�����F�ŕ\��������
					Debug.DrawRay(attack_ray.origin, attack_ray.direction * AttackDistance, Color.yellow);

					//�R���C�_�[�ƃ��C���ڐG
					if (attack_hit.collider)
					{
						if (attack_hit.collider.tag == "Enemy")
						{
							enemyObj = attack_hit.collider.gameObject.GetComponent<BaseEnemyFly>();
							hit_enemy_pos = enemyObj.transform.position;
							hit_enemy = true;
							hitstop_frame = 0;
						}
					}
				}
			}
		}
	}

	void FixedUpdate()
	{
		if (!ManagerAccessor.Instance.systemManager.GameEnd)
		{
			//�����ō����x�𒴂��Ȃ��悤�ɂ���
			if (rb2D.velocity.y < -FallSpeed)
			{
				Physics2D.gravity = new Vector3(0, -rb2D.velocity.y, 0);
			}
			else
			{
				//�d�͐ݒ�
				Physics2D.gravity = new Vector3(0, -Gravity, 0);
			}

			//�ڒn����
			//���C�𔭎˂���ʒu�̒���
			ground_rayPosition = transform.position + new Vector3(0.0f, -transform.localScale.y / 2, 0.0f);

			//���C�̐ڒn����
			RayGround(ground_ray, ground_hit, ground_rayPosition);

			//���̉�]
			if (hit_enemy)
			{
				if (!hitstop_on)
					transform.GetChild((int)PrefabChild.PlayerSprite).gameObject.transform.rotation = atkQuaternion * Quaternion.Euler(0, 0, 90);
			}
			else
			{
				if (!hitstop_on)
					transform.GetChild((int)PrefabChild.PlayerSprite).gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
			}

			//�J�[�\����\�����邩�ǂ���
			if (AttackCursor)
			{
				//����\��
				transform.GetChild((int)PrefabChild.Arrow).gameObject.SetActive(true);

				//���̉�]
				transform.GetChild((int)PrefabChild.Arrow).gameObject.transform.localScale = new Vector3(1.0f, AttackDistance / 5, 1.0f);
				transform.GetChild((int)PrefabChild.Arrow).gameObject.transform.rotation = atkQuaternion * Quaternion.Euler(0, 0, 90);
			}
			else
			{
				//�����\��
				transform.GetChild((int)PrefabChild.Arrow).gameObject.SetActive(false);
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

					rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

					//�J�E���g����
					jump_count++;
				}
			}
			else
			{
				jump_key_flag = false;
			}

			//���C���G�ɓ��������ꍇ
			if (hit_enemy)
			{
				if (enemyObj != null)
				{
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
				}
				//���C�������������A�G�������Ă��܂����ꍇ
				else
				{
					if (!hitstop_on)
					{
						//�U���֘A�̃t���O���Z�b�g
						jump_count = 0;
						hitstop_on = false;
						hitstop_frame = 0;
						move_stop = false;
						hit_enemy = false;
						rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
						dont_move = false;
						attack_cooltime = 0;
						attack_ok = true;

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
				}
			}

			//�q�b�g�X�g�b�v
			if (hitstop_on)
			{
				hitstop_frame++;

				if (hitstop_frame >= HitStopFrame)
				{
					AttackFin();
				}
			}

			//�U���N�[���^�C��
			if (!attack_ok)
			{
				attack_cooltime++;

				if (attack_cooltime >= AttackCoolTime)
				{
					attack_cooltime = 0;
					attack_ok = true;
				}
			}

			//�t�B�[�o�[�^�C���̂Ƃ�
			if (ManagerAccessor.Instance.systemManager.FeverTime)
			{
				//�t�B�[�o�[�^�C�����J�E���g
				time_fever++;

				//if (time_fever % 50 == 0)
				//{
				//	textFeverTime.text = "����" + (FeverTime - (time_fever / 50)) + "�b";
				//}

				//���Ԍo�߂�����
				if (time_fever >= FeverTime * 50)
				{
					//�t�B�[�o�[�^�C���I��
					ManagerAccessor.Instance.systemManager.FeverTime = false;
					time_fever = 0;

					//�I�[���̏���
					//���F�̃I�[��������
					transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Rainbow_Aura).gameObject.SetActive(false);
					//20�R���{�ȏ�Ȃ�I�����W�I�[�����o��
					if (ManagerAccessor.Instance.systemManager.Combo >= OrangeCombo)
					{
						transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(true);
					}

					//textFeverTime.text = "";
					//textFeverTime.gameObject.SetActive(false);
				}
			}
		}
		else
        {
			rb2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		}
    }

    //�R���C�_�[�ɐG�ꂽ��
    private void OnTriggerEnter2D(Collider2D collider)
    {
		//�U���̃m�b�N�o�b�N
		if (collider.gameObject.tag == "Enemy")
		{
			if (hit_enemy)
            {
				if (!hitstop_on)
                {
					hitstop_on = true;
					hitstop_frame = 0;
					Attack();
				}
			}
		}
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
		//�n�ʂɐG�ꂽ��
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = true;
			move_stop = false;

			if(ManagerAccessor.Instance.systemManager.Combo > 0)
				ComboReset();
		}
	}

    //�R���C�_�[�ɐG��Ă����
    private void OnTriggerStay2D(Collider2D collider)
    {
		if (collider.gameObject.tag == "Enemy")
		{
			if (hit_enemy)
			{
				if (!hitstop_on)
				{
					hitstop_on = true;
					hitstop_frame = 0;
					Attack();
				}
			}
		}
	}
    private void OnCollisionStay2D(Collision2D collision)
    {
		//�n�ʂɐG�ꂽ��
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground_on = true;
			move_stop = false;

			if (ManagerAccessor.Instance.systemManager.Combo > 0)
				ComboReset();
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
				jump_count = 0;
			}
		}
	}

	//�U������
	private void AttackFin()
	{
		//�U���֘A�̃t���O�S�����Z�b�g
		jump_count = 0;
		hitstop_on = false;
		hitstop_frame = 0;
		hit_enemy = false;
		rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		dont_move = false;

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
	private int ScoreSetting(int combo)
    {
		//���Z�����X�R�A��������
		score_add = 0;

		//�R���{�ɂ���ăX�R�A���Z
		if (combo >= 1 && combo < 50)
			score_add += 1000;
		else if (combo >= 50 && combo < 100)
			score_add += 5000;
		else if (combo >= 100)
			score_add += 10000;

		/*
		�R���{�{�[�i�X
			 10�R���{���Ƃ�  5000�_
			 50�R���{���Ƃ� 10000�_
			100�R���{���Ƃ�100000�_
		*/
		if (combo % 100 == 0)
		{
			score_add += 100000;
		}
		else if (combo % 50 == 0)
		{
			score_add += 10000;
		}
		else if (combo % 10 == 0)
		{
			score_add += 5000;
		}

		Debug.Log(score_add.ToString());
		return score_add;
	}

	//�U������
	private void Attack()
	{
		//���̉�]
		StartCoroutine(StartRotato());

		//�R���{���₵�Ĕ��f
		ManagerAccessor.Instance.systemManager.Combo++;
		ManagerAccessor.Instance.systemManager.textCombo.text = ManagerAccessor.Instance.systemManager.Combo.ToString();
		ManagerAccessor.Instance.systemManager.AllCombo++;

		//�}�b�N�X�R���{�ύX
		if (ManagerAccessor.Instance.systemManager.MaxCombo < ManagerAccessor.Instance.systemManager.Combo)
		{
			ManagerAccessor.Instance.systemManager.MaxCombo = ManagerAccessor.Instance.systemManager.Combo;
			ManagerAccessor.Instance.systemManager.textMaxCombo.text = ManagerAccessor.Instance.systemManager.MaxCombo.ToString();
		}

		//�t�B�[�o�[�^�C���ł͂Ȃ��Ƃ�
		if (!ManagerAccessor.Instance.systemManager.FeverTime)
		{
			//�t�B�[�o�[�^�C���܂ł̃R���{���J�E���g
			combo_fever_count++;

			//20�R���{�ȏ�Ȃ�I�����W�I�[�����o��
			if (ManagerAccessor.Instance.systemManager.Combo >= OrangeCombo)
			{
				transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(true);
			}

			//�R���{��B�������Ƃ�
			if (combo_fever_count >= FeverCombo)
			{
				//�t�B�[�o�[�^�C���Ɉڍs
				ManagerAccessor.Instance.systemManager.FeverTime = true;

				//�t�B�[�o�[�p�̃R���{��������
				combo_fever_count = 0;

				//���F�̃I�[�����o��
				transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(false);
				transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Rainbow_Aura).gameObject.SetActive(true);

				//�c�莞�ԃe�L�X�g��\��
				//textFeverTime.gameObject.SetActive(true);
				//textFeverTime.text = "����" + (FeverTime - (time_fever / 50)) + "�b";
			}
		}

		if (enemyObj != null)
		{
			//�G��HP���炷
			enemyObj.HP -= ATK;

			//�\���p�̃X�R�A���߂�
			ManagerAccessor.Instance.systemManager.Score += ScoreSetting(ManagerAccessor.Instance.systemManager.Combo);
			ManagerAccessor.Instance.systemManager.textScore.text = ManagerAccessor.Instance.systemManager.Score.ToString();

			//HP��0�̎�
			//if (enemyObj.HP <= 0)
			//{
			//    //�q�b�g�X�g�b�v�̏���
			//}
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

	//�R���{���Z�b�g
	private void ComboReset()
    {
		//�R���{���Z�b�g���Ĕ��f
		if (!ManagerAccessor.Instance.systemManager.FeverTime)
		{
			//�R���{���Z�b�g�J�n
			combo_reset = true;

			//�R���{���Z�b�g���Ĕ��f
			ManagerAccessor.Instance.systemManager.Combo = 0;
			ManagerAccessor.Instance.systemManager.textCombo.text = ManagerAccessor.Instance.systemManager.Combo.ToString();

			//�t�B�[�o�[�^�C���ɕK�v�ȃR���{��������
			combo_fever_count = 0;

			//�I�����W�I�[��������
			transform.GetChild((int)PrefabChild.PlayerSprite).GetChild((int)PrefabChild.Aura).gameObject.SetActive(false);
		}
	}

	//��]�A�j���[�V����
	private IEnumerator StartRotato()
	{
		RotationAnimator.SetTrigger("Attack");
		yield return null;//1�t���[���҂�

		while (!RotationAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idel"))
		{
			//���o���͑ҋ@
			yield return null;
		}
	}
}

/* ��邱��
 * �J�[�\���킩��₷��
 * �G�ɍU����������ʉ��u�Y�V���A�I�I�v
*/

/* �o�O
 * �����W�����v����
*/