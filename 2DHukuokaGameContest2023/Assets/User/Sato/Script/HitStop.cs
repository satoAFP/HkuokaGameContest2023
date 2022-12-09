using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HitStop : MonoBehaviour
{
    [SerializeField, Header("������"), Range(0, 1f)]
    private float MoveWidth;

    [SerializeField, Header("������"), Range(0, 20)]
    private int MoveNum;

    [SerializeField, Header("�ҋ@�t���[��"), Range(0, 20)]
    private int StopFrame;

    [SerializeField, Header("�q�b�g�X�g�b�v����")]
    public bool OnHitStop;


    private Vector3 FirstPos;       //�����ʒu�L���p
    private Vector3 MovePos;        //�ړ��ʓ��͗p
    private int MoveCount = 0;      //�����񐔃J�E���g�p
    private int FrameCount = 0;     //�t���[���J�E���g�p

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
