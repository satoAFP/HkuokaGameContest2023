using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    [SerializeField, Header("�����X�|�[���܂ł̃t���[��")]
    private int FirstSpownFrame;

    [SerializeField, Header("���X�|�[���܂ł̃t���[��")]
    private int RespownFrame;

    [SerializeField, Header("�t�B�[�o�[�^�C��")]
    private bool FeverTimeCheck;

    [SerializeField, Header("�o��������I�u�W�F�N�g")]
    private GameObject SpownEnemy;


    private int FrameCount = 0;     //�t���[���J�E���g�p
    private GameObject clone;       //�N���[�������I�u�W�F�N�g������悤
    private bool FeverTime = false; //�t�B�[�o�[���[�h�擾�p
    private int MemRespownFrame = 0;//���X�|�[���܂ł̎��ԋL���p


    private void Start()
    {
        //�����X�|�[�������^�C�~���O���炷����
        MemRespownFrame = RespownFrame;
        RespownFrame = FirstSpownFrame;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!FeverTimeCheck)//�ʏ탂�[�h
        {
            EnemySpawn();
        }
        else//�t�B�[�o�[���[�h
        {
            FeverTime = ManagerAccessor.Instance.systemManager.FeverTime;
            if (FeverTime)
            {
                EnemySpawn();
            }
        }
    }


    /// <summary>
    /// �G�����p�֐�
    /// </summary>
    private void EnemySpawn()
    {
        if (clone == null)
        {
            if (RespownFrame <= FrameCount)
            {
                clone = Instantiate(SpownEnemy, transform.position, Quaternion.identity);
                clone.transform.parent = transform;
                RespownFrame = MemRespownFrame;
                FrameCount = 0;
            }
            FrameCount++;
        }
    }
}
