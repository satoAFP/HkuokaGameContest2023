using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyFly : BaseStatusClass
{
    [SerializeField, Header("�{�X�̂Ƃ�")]
    private bool BossMode;

    [SerializeField, Header("�G�̍U�����̔��ł�������")]
    private Vector3 MoveDirection;


    [SerializeField, Header("�o��G�t�F�N�g�̐�"), Range(0, 10)]
    public int EffectNum;

    [SerializeField, Header("�G�t�F�N�g���A���ŏo��Ƃ��̃t���[��"), Range(0, 20)]
    public int EffectInterval;



    [SerializeField, Header("��l�����m�̃��C�̑��x"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("���C�̋���"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("��l���ɋ߂Â��Ď~�܂鋗��"), Range(0, 5), Space(50)]
    private float StopDistance;

    [SerializeField, Header("����ł��������܂ł̃t���[��"), Range(0, 100)]
    public int DethFrame;


    [SerializeField, Header("�����ړ�")]
    private bool TrackingFrag;
    [SerializeField, Header("�����ړ�")]
    private bool ConstantFrag;


    [SerializeField, Header("�U�����ꂽ�Ƃ��̃G�t�F�N�g")]
    private GameObject RecEffct;

    [SerializeField, Header("���ʂƂ��̃G�t�F�N�g")]
    private GameObject DethEffct;

    [SerializeField, Header("���ʂƂ��̃A�j���[�V����")]
    private GameObject DethAni;

    [SerializeField, Header("���ʂƂ���SE")]
    private AudioClip DethSound;

    [SerializeField, Header("�{�X�̎�_�����ʂƂ���SE")]
    private AudioClip BossDethSound;



    private Vector2 RayRotato;                  //���C�̉�]�ʒu����ϐ�
    private float rotato = 0;                   //��]��
    private GameObject SearchGameObject;        //���C�ɐG�ꂽ�I�u�W�F�N�g
    private bool AttckMode = false;             //��l�������������̍U�����[�h
    private int DethFrameCount = 0;             //���ʂ܂ł̃J�E���g
    private bool OnDamageEffect = false;        //�_���[�W�󂯂����̃G�t�F�N�g
    private int EffectIntervalCount = 0;        //�G�t�F�N�g�̃C���^�[�o���̃J�E���g
    private int EffectCount = 0;                //�G�t�F�N�g�̉񐔃J�E���g
    private int NowHP = 0;                      //���݂�HP
    private AudioSource audioSource;

    private bool first1 = true;


    [System.NonSerialized]
    public int stopCount = 0;                   //�ړ���~�܂ł̃J�E���g


    private void Start()
    {
        NowHP = HP;
        //audioSource = ManagerAccessor.Instance.soundManager.GetComponent<AudioSource>();
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

        //�G�t�F�N�g�Ăяo��
        DamageEffect();

        //�s������
        if(ConstantFrag)
        {
            transform.position += MoveDirection;
        }

        //HP�����������G�t�F�N�g�\��
        if (NowHP > HP)
        {
            OnDamageEffect = true;
            NowHP = HP;
        }

    }




    

    /// <summary>
    /// �_���[�W���󂯂����̃G�t�F�N�g����
    /// </summary>
    private void DamageEffect()
    {
        //�G�t�F�N�g�Đ��J�n
        if(OnDamageEffect)
        {
            //�G�t�F�N�g�̐�
            if (EffectCount < EffectNum) 
            {
                //���G�t�F�N�g���o���܂ł̃t���[��
                if (EffectIntervalCount % EffectInterval == 0) 
                {
                    //�����������_���ȕ���������
                    GameObject clone = Instantiate(RecEffct, transform.position, Quaternion.identity);
                    clone.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 180));
                    EffectCount++;
                }
            }
            else 
            {
                //�G�t�F�N�g�I��
                EffectCount = 0;
                OnDamageEffect = false;
            }
            EffectIntervalCount++;
        }
    }




    /// <summary>
    /// ���S����
    /// </summary>
    private void Deth()
    {
        if (HP <= 0)
        {
            //�G�t�F�N�g��\�����ď���
            //OnDamageEffect = true;
            DethFrameCount++;
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());

            //SE�Đ�
            //if (!BossMode)
            //    audioSource.PlayOneShot(DethSound);
            //else
            //    audioSource.PlayOneShot(BossDethSound);


            if (DethFrame == DethFrameCount)
            {
                //���S���̃G�t�F�N�g
                Instantiate(DethEffct, transform.position, Quaternion.identity);

                //���S���̃A�j���[�V����
                Instantiate(DethAni, transform.position, Quaternion.identity);

                //�폜
                if (!BossMode)
                    Destroy(transform.parent.gameObject);
                else
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



    //private void RightMove()
    //{
    //    //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
    //    if (rigidbody2d.velocity.x < LimitSpeed)
    //    {
    //        rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
    //    }
    //    transform.localScale = new Vector3(1f, 1f, 1f);
    //}


    //private void LeftMove()
    //{
    //    //�~�܂�t���[���܂œ���
    //    //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
    //    if (rigidbody2d.velocity.x > -LimitSpeed)
    //    {
    //        rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
    //    }
    //    transform.localScale = new Vector3(-1f, 1f, 1f);
    //}


}
