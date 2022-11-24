using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDethAnimation : MonoBehaviour
{
    [SerializeField, Header("X���̔�΂�����"),Range(0,100)]
    private float ReceivePowX;

    [SerializeField, Header("Y���̔�΂����ő�l"), Range(50, 200)]
    private float ReceiveMaxPowY;

    [SerializeField, Header("Y���̔�΂����Œ�l"), Range(0, 50)]
    private float ReceiveMinPowY;

    [SerializeField, Header("������܂ł̃t���[��"), Range(0, 200)]
    private int DestroyFrame;

    private Rigidbody2D rb2d;       //���W�b�g�{�f�B
    private int FrameCount = 0;     //������܂ł̃t���[���J�E���g�p


    private void Start()
    {
        //���W�b�g�{�f�B�̏�����
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        rb2d.AddForce(new Vector2(Random.Range(-ReceivePowX, ReceivePowX), Random.Range(ReceiveMinPowY, ReceiveMaxPowY)), ForceMode2D.Force);

    }

    // Update is called once per frame
    void Update()
    {
        if (FrameCount >= DestroyFrame)
        {
            Destroy(transform.parent.gameObject);
        }

        FrameCount++;
    }
}
