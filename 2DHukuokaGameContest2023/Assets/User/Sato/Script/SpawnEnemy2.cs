using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    [SerializeField, Header("�����X�|�[���܂ł̃t���[��")]
    private int FirstSpownFrame;

    [SerializeField, Header("���X�|�[���܂ł̃t���[��")]
    private int RespownFrame;

    [SerializeField, Header("�G�t�F�N�g�����炵�ĕ\�����邽�߂̃t���[��")]
    private int EffectFrame;

    [SerializeField, Header("�t�B�[�o�[�^�C��")]
    private bool FeverTimeCheck;

    [SerializeField, Header("�_�ł܂ł̃t���[��")]
    private int TikatikaFrame;

    [SerializeField, Header("�_�ł̊Ԋu")]
    private int TikatikaInterval;


    [SerializeField, Header("�o��������I�u�W�F�N�g")]
    private GameObject SpownEnemy;

    [SerializeField, Header("�o���G�t�F�N�g")]
    private GameObject SpownEffect;


    private int FrameCount = 0;     //�t���[���J�E���g�p
    private GameObject clone;       //�N���[�������I�u�W�F�N�g������悤
    private bool FeverTime = false; //�t�B�[�o�[���[�h�擾�p
    private int MemRespownFrame = 0;//���X�|�[���܂ł̎��ԋL���p
    private Color ObjColor;         //�N���[�������I�u�W�F�N�g�̐F���
    private bool AlphInversion = false;//�A���t�@�ύX�p
    private int AlphFrameCount = 0; //�A���t�@�ύX�p�t���[���J�E���g

    private void Start()
    {
        //�����X�|�[�������^�C�~���O���炷����
        MemRespownFrame = RespownFrame;
        RespownFrame = FirstSpownFrame;

        ObjColor = new Color(1, 1, 1, 0);
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

                if (ManagerAccessor.Instance.player.time_fever > TikatikaFrame) 
                {
                    if (clone != null)
                    {
                        AlphFrameCount++;
                        
                        if (AlphFrameCount >= TikatikaInterval) 
                        {
                            AlphInversion = !AlphInversion;
                            AlphFrameCount = 0;
                        }

                        //�A���t�@�؂�ւ�
                        if (AlphInversion)
                        {
                            AlphChange(0); 
                        }
                        else
                        {
                            AlphChange(1);
                        }
                    }
                }
            }
            else
            {
                if (clone != null)
                    Destroy(clone);
                //�����X�|�[�������^�C�~���O���炷����
                RespownFrame = FirstSpownFrame;
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
            if (RespownFrame - EffectFrame == FrameCount)
                Instantiate(SpownEffect, transform.position, Quaternion.identity);


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

    //�A���t�@�ύX�p
    private void AlphChange(float a)
    {
        ObjColor.a = a;
        clone.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = ObjColor;
        clone.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = ObjColor;
        clone.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = ObjColor;
    }
}
