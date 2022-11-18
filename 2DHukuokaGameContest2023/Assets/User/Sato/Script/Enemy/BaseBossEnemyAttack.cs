using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossEnemyAttack : BaseStatusClass
{
    [SerializeField, Header("���ł�������")]
    private Vector3 MoveDirection;

    [SerializeField, Header("�o��G�t�F�N�g�̐�"), Range(0, 10), Space(50)]
    public int EffectNum;

    [SerializeField, Header("�G�t�F�N�g���A���ŏo��Ƃ��̃t���[��"), Range(0, 20)]
    public int EffectInterval;



    [SerializeField, Header("��l�����m�̃��C�̑��x"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("���C�̋���"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("��l���ɋ߂Â��Ď~�܂鋗��"), Range(0, 5)]
    private float StopDistance;


    [SerializeField, Header("��l���ƏՓ˂������̃m�b�N�o�b�N")]
    private Vector2 KnockbackPow;


    [SerializeField, Header("����ł��������܂ł̃t���[��"), Range(0, 100)]
    public int DethFrame;

    [SerializeField,Header("���C�̎g�p")]
    private bool TrackingFrag;



    [SerializeField, Header("�U�����ꂽ�Ƃ��̃G�t�F�N�g")]
    private GameObject RecEffct;

    [SerializeField, Header("���ʂƂ��̃G�t�F�N�g")]
    private GameObject DethEffct;



    private Vector2 RayRotato;                  //���C�̉�]�ʒu����ϐ�
    private float rotato = 0;                   //��]��
    private GameObject SearchGameObject;        //���C�ɐG�ꂽ�I�u�W�F�N�g
    private bool AttckMode = false;             //��l�������������̍U�����[�h
    private int DethFrameCount = 0;             //���ʂ܂ł̃J�E���g
    private bool OnDamageEffect = false;        //�_���[�W�󂯂����̃G�t�F�N�g
    private int EffectIntervalCount = 0;        //�G�t�F�N�g�̃C���^�[�o���̃J�E���g
    private int EffectCount = 0;                //�G�t�F�N�g�̉񐔃J�E���g
    private Vector3 MoveDirPow;                 //�ړ��ʉ��Z�p

    private bool first1 = true;



    [System.NonSerialized]
    public bool deth = false;                   //��l���󂯓n���p���S����
    [System.NonSerialized]
    public int stopCount = 0;                   //�ړ���~�܂ł̃J�E���g


    private void Start()
    {
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
        transform.position += MoveDirection;



    }






    /// <summary>
    /// �_���[�W���󂯂����̃G�t�F�N�g����
    /// </summary>
    private void DamageEffect()
    {
        //�G�t�F�N�g�Đ��J�n
        if (OnDamageEffect)
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

            //���S����󂯓n���p
            deth = true;

            OnDamageEffect = true;
            DethFrameCount++;
            gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;


            if (DethFrame == DethFrameCount)
            {
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


}
