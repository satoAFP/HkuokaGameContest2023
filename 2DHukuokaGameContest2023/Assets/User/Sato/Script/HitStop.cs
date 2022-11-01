using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [SerializeField, Header("動く幅"), Range(0, 1f)]
    private float MoveWidth;

    [SerializeField, Header("動く回数"), Range(0, 20)]
    private int MoveNum;

    [SerializeField, Header("待機フレーム"), Range(0, 20)]
    private int StopFrame;

    [SerializeField, Header("ヒットストップ発動")]
    private bool OnHitStop;


    private Vector3 FirstPos;       //初期位置記憶用
    private Vector3 MovePos;        //移動量入力用
    private int MoveCount = 0;      //動く回数カウント用
    private int FrameCount = 0;     //フレームカウント用

    // Start is called before the first frame update
    void Start()
    {
        FirstPos = transform.localPosition;
        MovePos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (OnHitStop)
            HitStopCamera();
    }


    private void HitStopCamera()
    {
        FrameCount++;
        if (FrameCount == StopFrame)
        {
            if (MoveCount < MoveNum)
            {
                if (FirstPos.y <= transform.localPosition.y)
                    MovePos.y = FirstPos.y - MoveWidth;
                else
                    MovePos.y = FirstPos.y + MoveWidth;
                

                MoveCount++;
                transform.localPosition = MovePos;
            }
            else
            {
                OnHitStop = false;
                MoveCount = 0;
                transform.localPosition = FirstPos;
            }
            FrameCount = 0;
        }

    }


}
