using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyFly : BaseStatusClass
{
    [SerializeField, Header("�ō����x"), Range(0, 1)]
    private float MaxSpeed;

    [SerializeField, Header("��������"), Range(0, 0.1f)]
    private float Deceleration;

    [SerializeField, Header("�꒼���̎��̈ړ����x"), Range(0, 100)]
    private float MoveSpeed;

    [SerializeField, Header("�ō����x"), Range(0, 20)]
    private float LimitSpeed;

    [SerializeField, Header("��~�܂ł̃t���[��"), Range(0, 500)]
    public int StopCount;



    [SerializeField, Header("��l�����m�̃��C�̑��x"), Range(0.01f, 1), Space(50)]
    private float RaySpeed;

    [SerializeField, Header("���C�̋���"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("��l���ɋ߂Â��Ď~�܂鋗��"), Range(0, 5), Space(50)]
    private float StopDistance;


    [SerializeField, Header("��l���ƏՓ˂������̃m�b�N�o�b�N"), Space(50)]
    private Vector2 KnockbackPow;


    [SerializeField, Header("����ł��������܂ł̃t���[��"), Range(0, 100), Space(50)]
    public int DethFrame;

    [SerializeField, Header("�A�C�e���h���b�v��"), Range(0, 100)]
    private int dropRate;


    [SerializeField, Header("���E�ړ�"), Space(50)]
    private bool RLMoveFrag;
    [SerializeField, Header("�E�ړ�")]
    private bool RMoveFrag;
    [SerializeField, Header("���ړ�")]
    private bool LMoveFrag;
    [SerializeField, Header("�ǔ��ړ�")]
    private bool TrackingFrag;



    [SerializeField, Header("�ҋ@���̉摜"), Space(50)]
    private Sprite StandImage;

    [SerializeField, Header("�U�����̉摜")]
    private Sprite AttackImage;

    [SerializeField, Header("�U�����ꂽ�Ƃ��̃G�t�F�N�g")]
    private GameObject RecEffct;

    [SerializeField, Header("���ʂƂ��̃G�t�F�N�g")]
    private GameObject DethEffct;




    private Rigidbody2D rigidbody2d;            //���W�b�g�{�f�B2D�擾
    private float movespeed;                    //�ړ����x�v�Z�p
    private int MoveCount = 0;                  //���E�ړ��؂�ւ��̃^�C�~���O�擾�p
    private bool MoveStop = false;              //�������~�߂����Ƃ��g�p
    private Vector2 RayRotato;                  //���C�̉�]�ʒu����ϐ�
    private float rotato = 0;                   //��]��
    private GameObject SearchGameObject;        //���C�ɐG�ꂽ�I�u�W�F�N�g
    private bool AttckMode = false;             //��l�������������̍U�����[�h
    private DropItemList dropItemList;          //�h���b�v�A�C�e���Ǘ��p
    private int AttckDirection = 0;             //1:�E�ɍU���@2:���ɍU��
    private int DethFrameCount = 0;

    private bool first1 = true;



    [System.NonSerialized]
    public bool deth = false;                   //��l���󂯓n���p���S����
    [System.NonSerialized]
    public int stopCount = 0;                   //�ړ���~�܂ł̃J�E���g



    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        dropItemList = GameObject.Find("DropItemList").GetComponent<DropItemList>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //���S����
        Deth();

        //��l���T�[�`
        if (TrackingFrag)
        {
            RayPlayerCheck();
        }

        //�s������>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (!MoveStop)
        {
            //�U�����[�h�ɓ����Ă��Ȃ��Ƃ��̍s��---------------------------------------------------
            if (!AttckMode)
            {
                if (StopCount > stopCount)
                {
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
                }
                else
                    rigidbody2d.velocity = Vector3.zero;


                stopCount++;
            }
            //------------------------------------------------------------------


            //�U�����[�h�̍s��--------------------------------------------------
            else
            {
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
            }
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        //��l���ƏՓˎ��̃m�b�N�o�b�N
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(RecEffct, transform.position, Quaternion.identity);

            HP -= collision.gameObject.GetComponent<Player_Ver2>().ATK;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��l���̍U���ɓ���������
        if (collision.tag == "PlayerAttack")
        {
            HP -= collision.gameObject.transform.root.gameObject.GetComponent<Player_Ver2>().ATK;


        }

    }



    /// <summary>
    /// ���S����
    /// </summary>
    private void Deth()
    {
        if (HP <= 0)
        {

            //���S����󂯓n���p
            deth = true;

            DethFrameCount++;
            gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;


            if (DethFrame == DethFrameCount)
            {
                //�A�C�e���h���b�v����
                if (dropRate >= Random.Range(0, 100))
                {
                    Instantiate(dropItemList.DropItem[Random.Range(0, dropItemList.DropItem.Length)], transform.localPosition, Quaternion.identity);
                }


                //���S���̃G�t�F�N�g
                Instantiate(DethEffct, transform.position, Quaternion.identity);

                //�폜
                Destroy(gameObject);
            }
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
        //�~�܂�t���[���܂œ���
        //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
        if (rigidbody2d.velocity.x > -LimitSpeed)
        {
            rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
        }
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }



    /// <summary>
    /// �ړ����̉��������̐܂�Ԃ��n�_�v�Z�p
    /// </summary>
    /// <param name="near"></param>
    /// <returns></returns>
    private bool Near(float near)
    {
        if (((MaxSpeed / 2) - Deceleration) < near && ((MaxSpeed / 2) + Deceleration) > near)
            return true;
        else
            return false;
    }
}
