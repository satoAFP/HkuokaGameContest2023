using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseStatusClass
{
    [SerializeField, Header("�ړ����x"), Range(0, 100)]
    private float MoveSpeed;

    [SerializeField, Header("�ړ�����"), Range(0, 1000)]
    private float MoveFrame;

    [SerializeField, Header("�d��"), Range(0, 100)]
    private float Gravity;

    [SerializeField, Header("��l�����m�̃��C�̑��x"), Range(0.01f, 1)] 
    private float RaySpeed;

    [SerializeField, Header("���C�̋���"), Range(1, 10)]
    private float raydistance;


    private Rigidbody2D rigidbody;
    private int MoveCount = 0;
    private Vector2 RayRotato;                  //���C�̉�]�ʒu����ϐ�
    private float rotato = 0;                   //��]��
    private GameObject SearchGameObject;        //���C�ɐG�ꂽ�I�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //�d�͐ݒ�
        //Physics2D.gravity = new Vector3(0, -Gravity, 0);

        //�ړ�����
        MoveCount++;
        if (MoveCount <= (MoveFrame / 2)) 
        {
            rigidbody.velocity = new Vector3(-MoveSpeed, -5, 0);
        }
        if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame) 
        {
            rigidbody.velocity = new Vector3(MoveSpeed, -5, 0);
        }
        if (MoveCount > MoveFrame) 
        {
            MoveCount = 0;
        }


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
                
            }
        }
    }
}
