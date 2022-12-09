using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HitStop : MonoBehaviour
{
    [SerializeField, Header("動く幅"), Range(0, 1f)]
    private float MoveWidth;

    [SerializeField, Header("動く回数"), Range(0, 20)]
    private int MoveNum;

    [SerializeField, Header("待機フレーム"), Range(0, 20)]
    private int StopFrame;

    [SerializeField, Header("ヒットストップ発動")]
    public bool OnHitStop;


    private Vector3 FirstPos;       //初期位置記憶用
    private Vector3 MovePos;        //移動量入力用
    private int MoveCount = 0;      //動く回数カウント用
    private int FrameCount = 0;     //フレームカウント用

    // Start is called before the first frame update
    void Start()
    {
        FirstPos = gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        MovePos = gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
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
                if (FirstPos.x <= gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x)
                    MovePos.x = FirstPos.x - MoveWidth;
                else
                    MovePos.x = FirstPos.x + MoveWidth;

                Debug.Log(FirstPos);

                MoveCount++;
                gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = MovePos;
            }
            else
            {
                OnHitStop = false;
                MoveCount = 0;
                gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = FirstPos;
            }
            FrameCount = 0;
        }

    }


}
