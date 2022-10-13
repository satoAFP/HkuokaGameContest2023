using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseStatusClass
{
    [SerializeField, Header("�ړ����x"), Range(0, 100)]
    private float MoveSpeed;

    [SerializeField, Header("�ō����x"), Range(0, 20)]
    private float LimitSpeed;

    [SerializeField, Header("�ړ�����"), Range(0, 1000)]
    private float MoveFrame;

    [SerializeField, Header("�d��"), Range(0, 100)]
    private float Gravity;

    [SerializeField, Header("��l�����m�̃��C�̑��x"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("���C�̋���"), Range(1, 10)]
    private float raydistance;

    [SerializeField, Header("��l���ɋ߂Â��Ď~�܂鋗��"), Range(0, 5)]
    private float StopDistance;

    [SerializeField, Header("���g����ǂ̈ʒu�ɍU�������ݒ肷�邩")]
    private Vector3 AttackPos;

    [SerializeField, Header("�U���p�x"), Range(0, 500)]
    private int AttackFrequency;

    [SerializeField, Header("�U�����[�V�����̃t���[��"), Range(0, 50)]
    private int AttackMotionFrame;

    [SerializeField, Header("��l���ƏՓ˂������̃m�b�N�o�b�N")]
    private Vector2 KnockbackPow;

    [SerializeField, Header("����グ���̃m�b�N�o�b�N")]
    private Vector2 KnockbackKickPow;


    [SerializeField, Header("�����蔻��I�u�W�F�N�g")]
    private GameObject AttackCollision;

    [SerializeField, Header("�ҋ@���̉摜")]
    private Sprite StandImage;

    [SerializeField, Header("�U�����̉摜")]
    private Sprite AttackImage;




    private Rigidbody2D rigidbody2d;            //���W�b�g�{�f�B2D�擾
    private int MoveCount = 0;                  //���E�ړ��؂�ւ��̃^�C�~���O�擾�p
    private bool MoveStop = false;              //�������~�߂����Ƃ��g�p
    private Vector2 RayRotato;                  //���C�̉�]�ʒu����ϐ�
    private float rotato = 0;                   //��]��
    private GameObject SearchGameObject;        //���C�ɐG�ꂽ�I�u�W�F�N�g
    private bool AttckMode = false;             //��l�������������̍U�����[�h
    private int AttckDirection = 0;             //1:�E�ɍU���@2:���ɍU��
    private int AttackFreCount = 0;             //�U���p�x�v�Z���t���[���J�E���g�p
    private int AttackMotCount = 0;             //�U�����[�V�����v�Z���t���[���J�E���g�p
    private bool AttackMotCheck = false;        //�U�����[�V������true�ɂȂ�
    private bool Attck1 = false;                //�U��1

    private bool first1 = true;
    private bool first2 = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�d�͐ݒ�>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Physics2D.gravity = new Vector3(0, -Gravity, 0);

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        //���S����>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (HP <= 0) 
        {
            //�A�C�e���h���b�v����

            //�폜
            Destroy(gameObject);
        }

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        //��l���T�[�`>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //�I�u�W�F�N�g����E����Ray��L�΂�
        Ray2D ray = new Ray2D(transform.position, transform.right);
        RaycastHit2D hit;

        //Corgi�AShiba���C���[�Ƃ����Փ˂���
        int layerMask = LayerMask.GetMask(new string[] { "Player" });

        //�����̍ŏ��ɏ�����
        if (first1)
        {
            hit = Physics2D.Raycast(ray.origin, new Vector2(0, 0) * raydistance, raydistance, layerMask);
            first1 = false;
        }

        //���C�̉�]����
        if (!AttckMode)
        {
            rotato += RaySpeed;
            RayRotato = new Vector2(Mathf.Cos(rotato), Mathf.Sin(rotato));

            //���C���΂�
            hit = Physics2D.Raycast(ray.origin + RayRotato, RayRotato * raydistance, raydistance, layerMask);
            Debug.DrawRay(ray.origin + RayRotato, RayRotato * raydistance, Color.green);
        }
        else
        {

            //���C���΂�
            hit = Physics2D.Raycast(ray.origin, new Vector2(SearchGameObject.transform.localPosition.x, SearchGameObject.transform.localPosition.y) - ray.origin, raydistance, layerMask);
            Debug.DrawRay(ray.origin, new Vector2(SearchGameObject.transform.localPosition.x, SearchGameObject.transform.localPosition.y) - ray.origin, Color.green);
        }

        //���C�Ŏ�l������������
        if (hit.collider)
        {
            SearchGameObject = hit.collider.gameObject;

            if (SearchGameObject.tag == "Player")
            {
                //�U�����[�h�Ɉڍs
                AttckMode = true;
            }
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<




        //�s������>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (!MoveStop)
        {
            //�U�����[�h�ɓ����Ă��Ȃ��Ƃ��̍s��---------------------------------------------------
            if (!AttckMode)
            {
                MoveCount++;
                if (MoveCount <= (MoveFrame / 2))
                {
                    //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
                    if (rigidbody2d.velocity.x > -LimitSpeed)
                    {
                        rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
                    }
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame)
                {
                    //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
                    if (rigidbody2d.velocity.x < LimitSpeed)
                    {
                        rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
                    }
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                if (MoveCount > MoveFrame)
                {
                    //�J�E���g���Z�b�g
                    MoveCount = 0;
                }

                //�U�����[�h�łȂ����͍U���܂ł̃t���[���J�E���g�����Z�b�g
                AttackFreCount = 0;
            }
            //------------------------------------------------------------------


            //�U�����[�h�̍s��--------------------------------------------------
            else
            {
                //�U���܂ł̃t���[���J�E���g
                AttackFreCount++;

                //�E���鎞
                if (SearchGameObject.transform.localPosition.x < transform.localPosition.x)
                {
                    if ((SearchGameObject.transform.localPosition.x - transform.localPosition.x) <= -StopDistance)
                    {
                        //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
                        if (rigidbody2d.velocity.x > -LimitSpeed)
                        {
                            //�ړ�����
                            rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
                        }
                    }
                    //���ɍU��
                    AttckDirection = 2;
                    //������
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                //�����鎞
                if (SearchGameObject.transform.localPosition.x > transform.localPosition.x)
                {
                    if ((SearchGameObject.transform.localPosition.x - transform.localPosition.x) >= StopDistance)
                    {
                        //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
                        if (rigidbody2d.velocity.x < LimitSpeed)
                        {
                            rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
                        }
                    }
                    //�E�ɍU��
                    AttckDirection = 1;
                    //�E����
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }


                //�U������
                Vector3 pos = transform.position;//�U���ʒu�̍��W�X�V�p
                                                 //�U���܂Ń^�C�~���O
                if (AttackFrequency == AttackFreCount)
                {
                    Attck1 = true;
                    AttackMotCheck = true;
                    AttackFreCount = 0;
                    gameObject.GetComponent<SpriteRenderer>().sprite = AttackImage;
                }

                //�U������Ƃ��̌���
                if (AttckDirection == 1)
                {
                    pos += AttackPos;//�E
                }
                if (AttckDirection == 2)
                {
                    pos -= AttackPos;//��
                }

                //�U�����[�V����
                if (AttackMotCheck)
                {
                    AttackMotCount++;
                    //���[�V�����I������
                    if (AttackMotionFrame == AttackMotCount)
                    {
                        AttackMotCount = 0;
                        AttackMotCheck = false;
                        gameObject.GetComponent<SpriteRenderer>().sprite = StandImage;
                    }
                }

                //�U��
                if (Attck1)
                {
                    GameObject obj = Instantiate(AttackCollision, pos, Quaternion.identity);
                    obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttckCollision>().Damage = ATK;
                    Attck1 = false;
                }
                //-------------------------------------------------------------------------
            }
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�n�ʂɒ��n�ňړ���~�I��
        if (collision.gameObject.tag == "Ground")
        {
            MoveStop = false;
        }


        //��l���ƏՓˎ��̃m�b�N�o�b�N
        if (collision.gameObject.tag=="Player")
        {
            if (AttckDirection == 1)
            {
                rigidbody2d.AddForce(new Vector2(-KnockbackPow.x, KnockbackPow.y), ForceMode2D.Force);
            }
            if (AttckDirection == 2)
            {
                rigidbody2d.AddForce(KnockbackPow, ForceMode2D.Force);
            }
            MoveStop = true;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��l���̍U���ɓ���������
        if (collision.tag == "PlayerAttack") 
        {
            //HP���炷����

        }

        //�R��グ��ꂽ���̏���
        if (collision.tag == "Finish") 
        {
            if (AttckDirection == 1)
            {
                rigidbody2d.AddForce(new Vector2(-KnockbackKickPow.x, KnockbackKickPow.y), ForceMode2D.Force);
            }
            if (AttckDirection == 2)
            {
                rigidbody2d.AddForce(KnockbackKickPow, ForceMode2D.Force);
            }
            MoveStop = true;
        }
    }
}
