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


    private Rigidbody2D rigidbody;              //���W�b�g�{�f�B2D�擾
    private int MoveCount = 0;                  //���E�ړ��؂�ւ��̃^�C�~���O�擾�p
    private Vector2 RayRotato;                  //���C�̉�]�ʒu����ϐ�
    private float rotato = 0;                   //��]��
    private GameObject SearchGameObject;        //���C�ɐG�ꂽ�I�u�W�F�N�g
    private bool AttckMode = false;             //��l�������������̍U�����[�h

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�d�͐ݒ�-----------------------------------------------------------------------------------------------------------------
        Physics2D.gravity = new Vector3(0, -Gravity, 0);




        //��l���T�[�`-------------------------------------------------------------------------------------------------------------
        //�I�u�W�F�N�g����E����Ray��L�΂�
        Ray2D ray = new Ray2D(transform.position, transform.right);
        SearchGameObject = null;
        RaycastHit2D hit;

        //Corgi�AShiba���C���[�Ƃ����Փ˂���
        int layerMask = LayerMask.GetMask(new string[] { "Player" });

        //���C�̉�]�̏�����
        rotato += RaySpeed;
        RayRotato = new Vector2(Mathf.Cos(rotato), Mathf.Sin(rotato));
        //���C���΂�
        hit = Physics2D.Raycast(ray.origin + RayRotato, RayRotato * raydistance, raydistance, layerMask);
        Debug.DrawRay(ray.origin + RayRotato, RayRotato * raydistance, Color.green);

        if (hit.collider)
        {
            SearchGameObject = hit.collider.gameObject;

            if (SearchGameObject.tag == "Player")
            {
                AttckMode = true;
            }
        }



        //�ړ�����----------------------------------------------------------------------------------------------------------------
        //�U�����[�h�ɓ����Ă��Ȃ��Ƃ��̍s��
        if (!AttckMode)
        {
            MoveCount++;
            if (MoveCount <= (MoveFrame / 2))
            {
                //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
                if (rigidbody.velocity.x > -LimitSpeed)
                {
                    rigidbody.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
                }
            }
            if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame)
            {
                //�ō����x�ɂȂ�Ƃ���ȏ�������Ȃ�
                if (rigidbody.velocity.x < LimitSpeed)
                {
                    rigidbody.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
                }
            }
            if (MoveCount > MoveFrame)
            {
                //�J�E���g���Z�b�g
                MoveCount = 0;
            }
        }
        //�U�����[�h�̍s��
        else
        {
            //���������Ă�
        }




        
        

        

    }
}
