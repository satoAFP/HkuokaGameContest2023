using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseStatusClass
{
    [SerializeField, Header("�ړ����x"), Range(0, 100), Space(50)]
    private float MoveSpeed;

    [SerializeField, Header("�ō����x"), Range(0, 20)]
    private float LimitSpeed;

    [SerializeField, Header("�ړ�����"), Range(0, 1000), Space(50)]
    private float MoveFrame;


    [SerializeField, Header("�W�����v��"), Range(0, 50)]
    private float JumpPower;

    [SerializeField, Header("�W�����v����Ԋu"), Range(0, 1000), Space(50)]
    private int JumpInterval;


    [SerializeField, Header("�d��"), Range(0, 100), Space(50)]
    private float Gravity;


    [SerializeField, Header("��l�����m�̃��C�̑��x"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("���C�̋���"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("��l���ɋ߂Â��Ď~�܂鋗��"), Range(0, 5), Space(50)]
    private float StopDistance;

    [SerializeField, Header("���g����ǂ̈ʒu�Ƀp���`�����ݒ肷�邩")]
    private Vector3 AttackPunchPos;

    [SerializeField, Header("���g����ǂ̈ʒu�ɃL�b�N�����ݒ肷�邩")]
    private Vector3 AttackKickPos;

    [SerializeField, Header("�U���p�x"), Range(0, 500)]
    private int AttackFrequency;

    [SerializeField, Header("�U�����[�V�����̃t���[��"), Range(0, 50), Space(50)]
    private int AttackMotionFrame;

    
    [SerializeField, Header("��l���ƏՓ˂������̃m�b�N�o�b�N")]
    private Vector2 KnockbackPow;

    [SerializeField, Header("����グ���̃m�b�N�o�b�N"), Space(50)]
    private Vector2 KnockbackKickPow;

    
    [SerializeField, Header("�A�C�e���h���b�v��"), Range(0, 100), Space(50)]
    private int dropRate;

    
    [SerializeField, Header("���E�ړ�"), Space(50)]
    private bool RLMoveFrag;
    [SerializeField, Header("�E�ړ�")]
    private bool RMoveFrag;
    [SerializeField, Header("���ړ�")]
    private bool LMoveFrag;
    [SerializeField, Header("�ǔ��ړ�")]
    private bool TrackingFrag;
    [SerializeField, Header("�W�����v�ړ�")]
    private bool JumpFrag;
    [SerializeField, Header("�p���`")]
    private bool PunchFrag;
    [SerializeField, Header("�L�b�N")]
    private bool KickFrag;


    
    [SerializeField, Header("�p���`����I�u�W�F�N�g"), Space(50)]
    private GameObject Attack1Collision;

    [SerializeField, Header("�L�b�N����I�u�W�F�N�g")]
    private GameObject Attack2Collision;

    [SerializeField, Header("�ҋ@���̉摜")]
    private Sprite StandImage;

    [SerializeField, Header("�U�����̉摜")]
    private Sprite AttackImage;




    private Rigidbody2D rigidbody2d;            //���W�b�g�{�f�B2D�擾
    private int MoveCount = 0;                  //���E�ړ��؂�ւ��̃^�C�~���O�擾�p
    private int JumpIntCount = 0;               //�W�����v����Ԋu�ݒ�
    private bool MoveStop = false;              //�������~�߂����Ƃ��g�p
    private Vector2 RayRotato;                  //���C�̉�]�ʒu����ϐ�
    private float rotato = 0;                   //��]��
    private GameObject SearchGameObject;        //���C�ɐG�ꂽ�I�u�W�F�N�g
    private bool AttckMode = false;             //��l�������������̍U�����[�h
    private DropItemList dropItemList;          //�h���b�v�A�C�e���Ǘ��p
    private int AttckDirection = 0;             //1:�E�ɍU���@2:���ɍU��
    private int AttackFreCount = 0;             //�U���p�x�v�Z���t���[���J�E���g�p
    private int AttackMotCount = 0;             //�U�����[�V�����v�Z���t���[���J�E���g�p
    private bool AttackMotCheck = false;        //�U�����[�V������true�ɂȂ�
    private bool Attck1 = false;                //�U��1
    private bool Attck2 = false;                //�U��2

    private bool first1 = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        dropItemList = GameObject.Find("DropItemList").GetComponent<DropItemList>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�d�͐ݒ�
        Physics2D.gravity = new Vector3(0, -Gravity, 0);

        //���S����
        Deth();

        //��l���T�[�`
        if (TrackingFrag)
        {
            RayPlayerCheck();
        }

        //�W�����v����
        if (JumpFrag)
        {
            Jump();
        }


        //�s������>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (!MoveStop)
        {
            //�U�����[�h�ɓ����Ă��Ȃ��Ƃ��̍s��---------------------------------------------------
            if (!AttckMode)
            {
                //���E�ړ�
                if (RLMoveFrag)
                {
                    MoveCount++;
                    if (MoveCount <= (MoveFrame / 2))
                    {
                        LeftMove(); Debug.Log("ccc");
                    }
                    if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame)
                    {
                        RightMove();
                        Debug.Log("aaa");
                    }
                    if (MoveCount > MoveFrame)
                    {
                        //�J�E���g���Z�b�g
                        MoveCount = 0; Debug.Log("bbb");
                    }
                }

                //�E�ړ�
                if (RMoveFrag)
                {
                    RightMove();
                }
                //���ړ�
                if (LMoveFrag)
                {
                    LeftMove();
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
                        LeftMove();
                    }
                    //���ɍU��
                    AttckDirection = 2;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                //�����鎞
                if (SearchGameObject.transform.localPosition.x > transform.localPosition.x)
                {
                    if ((SearchGameObject.transform.localPosition.x - transform.localPosition.x) >= StopDistance)
                    {
                        RightMove();
                    }
                    //�E�ɍU��
                    AttckDirection = 1;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }

                
                if (PunchFrag || KickFrag)
                {
                    //�U������
                    Vector3 pos = transform.position;//�U���ʒu�̍��W�X�V�p
                                                     //�U���܂Ń^�C�~���O
                    if (AttackFrequency == AttackFreCount)
                    {
                        if (PunchFrag)
                            Attck1 = true;
                        if (KickFrag)
                            Attck2 = true;
                        AttackMotCheck = true;
                        AttackFreCount = 0;
                        gameObject.GetComponent<SpriteRenderer>().sprite = AttackImage;
                    }

                    //�U������Ƃ��̌���
                    if (AttckDirection == 1)
                    {
                        if (PunchFrag)
                            pos += AttackPunchPos;//�E
                        if (KickFrag)
                        {
                            pos.x += AttackKickPos.x;
                            pos.y += AttackKickPos.y;
                        }
                    }
                    if (AttckDirection == 2)
                    {
                        if (PunchFrag)
                            pos -= AttackPunchPos;//��
                        if (KickFrag)
                        {
                            pos.x -= AttackKickPos.x;
                            pos.y += AttackKickPos.y;
                        }
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

                    //�p���`�U��
                    if (Attck1)
                    {
                        GameObject obj = Instantiate(Attack1Collision, pos, Quaternion.identity);
                        obj.transform.parent = gameObject.transform;
                        obj.GetComponent<AttckCollision>().Damage = ATK;
                        Attck1 = false;
                    }
                    //�L�b�N�U��
                    if (Attck2)
                    {
                        GameObject obj = Instantiate(Attack2Collision, pos, Quaternion.identity);
                        obj.transform.parent = gameObject.transform;
                        obj.GetComponent<AttckCollision>().Damage = ATK;
                        Attck2 = false;
                    }
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
            if(PunchFrag)
            {
                //�v���C���[�̍U���͂ƃp���`����󂯎��
            }
            if(KickFrag)
            {
                //�v���C���[�̍U���͂ƃL�b�N����󂯎��

                //�R��グ��ꂽ���̏���
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



    /// <summary>
    /// ���S����
    /// </summary>
    private void Deth()
    {
        if (HP <= 0)
        {
            //�A�C�e���h���b�v����
            if (dropRate >= Random.Range(0, 100))
            {
                Instantiate(dropItemList.DropItem[Random.Range(0, dropItemList.DropItem.Length)], transform.localPosition, Quaternion.identity);
            }

            //�폜
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// ���C�ɂ��v���C���[���m����
    /// </summary>
    private void RayPlayerCheck()
    {
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
    }


    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump()
    {
        //�W�����v�܂ł̃t���[���J�E���g
        JumpIntCount++;
        //�W�����v���s
        if (JumpIntCount == JumpInterval)
        {
            rigidbody2d.AddForce(transform.up * JumpPower);
            rigidbody2d.velocity = transform.up * JumpPower;
            JumpIntCount = 0;
        }
    }


    private void RightMove()
    {
        //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
        if (rigidbody2d.velocity.x < LimitSpeed)
        {
            rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }


    private void LeftMove()
    {
        //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
        if (rigidbody2d.velocity.x > -LimitSpeed)
        {
            rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
        }
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}
