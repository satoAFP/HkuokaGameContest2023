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

    [SerializeField, Header("カメラコライダー")]
    public GameObject CameraColl;

    [SerializeField, Header("主人公の代わり")]
    public GameObject DecoyColl;



    private Vector3 FirstPos;       //初期位置記憶用
    private Vector3 MovePos;        //移動量入力用
    private int MoveCount = 0;      //動く回数カウント用
    private int FrameCount = 0;     //フレームカウント用

    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        FirstPos = gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        MovePos = gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //主人公の代わりにいるオブジェクトに主人公のY座標のみ更新
        DecoyColl.transform.position = new Vector3(0, ManagerAccessor.Instance.player.gameObject.transform.position.y, 0);

        //ボス撃破時
        if (ManagerAccessor.Instance.systemManager.MoveCamera)
            BossCamera();

        //弱点攻撃時
        if (ManagerAccessor.Instance.systemManager.WeakCamera)
            WeakCamera();
    }


    public void BossCamera()
    {
        FrameCount++;

        if (ManagerAccessor.Instance.systemManager.BossDethTime * 50 >= FrameCount)
        {
            if (FrameCount == StopFrame)
            {
                if (MoveCount < MoveNum)
                {
                    if (FirstPos.x <= gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x)
                        MovePos.x = FirstPos.x - MoveWidth;
                    else
                        MovePos.x = FirstPos.x + MoveWidth;

                    MoveCount++;
                    gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = MovePos;
                }
                else
                {
                    MoveCount = 0;
                    gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = FirstPos;
                }
                FrameCount = 0;
            }
        }
    }

    public void WeakCamera()
    {
        FrameCount++;

        if (ManagerAccessor.Instance.systemManager.WeakCamera)
        {
            if (FrameCount == StopFrame)
            {
                if (MoveCount < MoveNum)
                {
                    if (FirstPos.x <= gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x)
                        MovePos.x = FirstPos.x - MoveWidth;
                    else
                        MovePos.x = FirstPos.x + MoveWidth;


                    MoveCount++;
                    gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = MovePos;
                }
                else
                {
                    ManagerAccessor.Instance.systemManager.WeakCamera = false;
                    MoveCount = 0;
                    gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = FirstPos;
                }
                FrameCount = 0;
            }
        }
    }


}
