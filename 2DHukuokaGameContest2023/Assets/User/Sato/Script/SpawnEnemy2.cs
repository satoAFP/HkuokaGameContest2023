using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    [SerializeField, Header("���X�|�[���܂ł̃t���[��")]
    private int RespownFrame;

    [SerializeField, Header("�t�B�[�o�[�^�C��")]
    private bool FeverTimeCheck;

    [SerializeField, Header("�o��������I�u�W�F�N�g")]
    private GameObject SpownEnemy;


    private int FrameCount = 0; //�t���[���J�E���g�p
    private GameObject clone;
    private bool FeverTime = false;



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!FeverTime)
        {
            if (clone == null)
            {
                if (RespownFrame <= FrameCount)
                {
                    clone = Instantiate(SpownEnemy, transform.position, Quaternion.identity);
                    clone.transform.parent = transform;
                    FrameCount = 0;
                }
                FrameCount++;
            }
        }
        else
        {
            FeverTime = ManagerAccessor.Instance.systemManager.FeverTime;
            if (FeverTime)
            {
                if (clone == null)
                {
                    if (RespownFrame <= FrameCount)
                    {
                        clone = Instantiate(SpownEnemy, transform.position, Quaternion.identity);
                        clone.transform.parent = transform;
                        FrameCount = 0;
                    }
                    FrameCount++;
                }
            }
        }
    }
}
