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

    [SerializeField, Header("�J�����R���C�_�[")]
    public GameObject CameraColl;

    [SerializeField, Header("��l���̑���")]
    public GameObject DecoyColl;



    private Vector3 FirstPos;       //�����ʒu�L���p
    private Vector3 MovePos;        //�ړ��ʓ��͗p
    private int MoveCount = 0;      //�����񐔃J�E���g�p
    private int FrameCount = 0;     //�t���[���J�E���g�p

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
        //��l���̑���ɂ���I�u�W�F�N�g�Ɏ�l����Y���W�̂ݍX�V
        DecoyColl.transform.position = new Vector3(0, ManagerAccessor.Instance.player.gameObject.transform.position.y, 0);

        //�{�X���j��
        if (ManagerAccessor.Instance.systemManager.MoveCamera)
            BossCamera();

        //��_�U����
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
