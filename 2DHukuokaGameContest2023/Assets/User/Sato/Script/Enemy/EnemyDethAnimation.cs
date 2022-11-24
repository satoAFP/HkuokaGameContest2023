using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDethAnimation : MonoBehaviour
{
    [SerializeField, Header("X軸の飛ばされる力"),Range(0,100)]
    private float ReceivePowX;

    [SerializeField, Header("Y軸の飛ばされる最大値"), Range(50, 200)]
    private float ReceiveMaxPowY;

    [SerializeField, Header("Y軸の飛ばされる最低値"), Range(0, 50)]
    private float ReceiveMinPowY;

    [SerializeField, Header("消えるまでのフレーム"), Range(0, 200)]
    private int DestroyFrame;

    private Rigidbody2D rb2d;       //リジットボディ
    private int FrameCount = 0;     //消えるまでのフレームカウント用


    private void Start()
    {
        //リジットボディの初期化
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
