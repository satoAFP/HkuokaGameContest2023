using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [SerializeField, Header("������"), Range(0, 1f)]
    private float MoveWidth;

    [SerializeField, Header("������"), Range(0, 20)]
    private int MoveNum;

    [SerializeField, Header("�ҋ@�t���[��"), Range(0, 20)]
    private int StopFrame;

    [SerializeField, Header("�q�b�g�X�g�b�v����")]
    private bool OnHitStop;


    private Vector3 FirstPos;       //�����ʒu�L���p
    private Vector3 MovePos;        //�ړ��ʓ��͗p
    private int MoveCount = 0;      //�����񐔃J�E���g�p
    private int FrameCount = 0;     //�t���[���J�E���g�p

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
