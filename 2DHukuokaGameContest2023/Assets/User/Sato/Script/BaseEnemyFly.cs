using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyFly : BaseStatusClass
{
    [SerializeField, Header("�ō����x"), Range(0, 1), Space(50)]
    private float MaxSpeed;

    [SerializeField, Header("��������"), Range(0, 0.1f)]
    private float Deceleration;



    [SerializeField, Header("��l�����m�̃��C�̑��x"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("���C�̋���"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("��l���ɋ߂Â��Ď~�܂鋗��"), Range(0, 5), Space(50)]
    private float StopDistance;


    [SerializeField, Header("��l���ƏՓ˂������̃m�b�N�o�b�N"), Space(50)]
    private Vector2 KnockbackPow;


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



    [SerializeField, Header("�ҋ@���̉摜"), Space(50)]
    private Sprite StandImage;

    [SerializeField, Header("�U�����̉摜")]
    private Sprite AttackImage;




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
    private bool MoveChange = true;             //�ړ������ύX�p
    private bool AccChange = true;              //�ړ��ʕύX�p

    private bool first1 = true;

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
                //���E�ړ�
                if (RLMoveFrag)
                {
                    if (MoveChange)
                    {
                        LeftMove();
                        AttckDirection = 2;
                    }
                    else
                    {
                        RightMove();
                        AttckDirection = 1;
                    }
                    if (movespeed <= 0)
                    {
                        MoveChange = !MoveChange;
                        movespeed = 0;
                        AccChange = !AccChange;
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
            //if (AttckDirection == 1)
            //{
            //    rigidbody2d.AddForce(new Vector2(-KnockbackPow.x, KnockbackPow.y), ForceMode2D.Force);
            //}
            //if (AttckDirection == 2)
            //{
            //    rigidbody2d.AddForce(KnockbackPow, ForceMode2D.Force);
            //}
            //MoveStop = true;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��l���̍U���ɓ���������
        if (collision.tag == "PlayerAttack")
        {
            HP -= collision.gameObject.transform.parent.gameObject.GetComponent<Player_Ver2>().ATK;


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



    private void RightMove()
    {
        if (AccChange)
            movespeed += Deceleration;
        else
            movespeed -= Deceleration;

        if (Near(movespeed))
            AccChange = !AccChange;

        transform.position += new Vector3(movespeed, 0, 0);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }


    private void LeftMove()
    {
        if (AccChange)
            movespeed += Deceleration;
        else
            movespeed -= Deceleration;

        if (Near(movespeed))
            AccChange = !AccChange;

        transform.position -= new Vector3(movespeed, 0, 0);
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
        {
            Debug.Log("aaa");
            return true;
        }
        else
            return false;
    }
}
