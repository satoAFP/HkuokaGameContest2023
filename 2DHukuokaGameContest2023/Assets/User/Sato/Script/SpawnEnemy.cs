using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //[SerializeField, Header("�o���p�x"),Range(0,500)]
    //private int FrequencyAppearance;

    //[SerializeField, Header("�ő�o����")]
    //private int spawnMax;

    //[SerializeField, Header("�X�|�[�����鉺���C��")]
    //public float spawnUnderLine;

    //[SerializeField, Header("�X�|�[������ド�C��")]
    //public float spawnOverLine;

    //[SerializeField, Header("�X�|�[������X���W")]
    //public float spawnXpos;

    //[SerializeField,Header("�o���������ړ��G�̎��")]
    //public GameObject[] spawnLeftEnemy;

    //[SerializeField, Header("�o�������E�ړ��G�̎��")]
    //public GameObject[] spawnRightEnemy;

    [Header("���݂�Wave")]
    public int Wave;


    [System.Serializable]
    public struct Base
    {
        public GameObject EnemySet;
        public Vector3 spawnPos;
        public int SpawnFrame;
        public int StopFrame;
    }

    [System.Serializable]
    public struct GameFlow
    {
        public Base[] GFbase;
        [System.NonSerialized]
        public GameObject[] GFObj;
    }


    [SerializeField, Header("�G�o���t���[�쐬�p")]
    private GameFlow[] GF;

    private int FrequencyCount = 0;     //�o���p�x�J�E���g�p
    private int spawnCount = 0;         //�X�|�[�����J�E���g�p
    private int FrameCount = 0;         //�t���[���J�E���g�p
    private int NextSpawnNum = 0;       //���G���o������t���[��
    private int NowArrangement = 0;     //���݂̔z��ԍ�
    private GameFlow[] ObjGroup;

    private void Start()
    {
        ObjGroup = new GameFlow[GF.Length];
        for (int i = 0; i < GF.Length; i++)
        {
            ObjGroup[i].GFObj = new GameObject[GF[i].GFbase.Length];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FluctuationSpawn();

        


    }

    /// <summary>
    /// �G�ݒ�蓮�֐�
    /// </summary>
    private void FluctuationSpawn()
    {
        //�ݒ肵�Ă���t���[�����ɏ���
        if (GF[Wave].GFbase[NowArrangement].SpawnFrame == FrameCount)
        {
            if (ObjGroup[Wave].GFObj[NowArrangement] == null)
            {
                //�G����
                Vector3 MyPos = transform.localPosition;
                GameObject clone = Instantiate(GF[Wave].GFbase[NowArrangement].EnemySet, MyPos + GF[Wave].GFbase[NowArrangement].spawnPos, Quaternion.identity);
                clone.gameObject.GetComponent<BaseEnemyFly>().stopCount = GF[Wave].GFbase[NowArrangement].StopFrame;
                ObjGroup[Wave].GFObj[NowArrangement] = clone;
            }

            //�g�p���Ă���z�񕪉��Z�����
            if (GF[Wave].GFbase.Length - 1 > NowArrangement)
                NowArrangement++;
            else
            {
                NowArrangement = 0;
                FrameCount = 0;
            }
        }

        FrameCount++;
    }


    //private void FixedSpawn()
    //{
    //    if (spawnMax == spawnCount)
    //    {
    //        FrequencyCount++;
    //        if (FrequencyAppearance == FrequencyCount)
    //        {
    //            //���o��
    //            if (Random.Range(0, 2) == 0)
    //            {
    //                GameObject clone = Instantiate(spawnLeftEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
    //            }
    //            //�E�o��
    //            else
    //            {
    //                GameObject clone = Instantiate(spawnRightEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(-spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
    //            }
    //            //�X�|�[���J�E���g
    //            spawnCount++;

    //            //�J�E���g���Z�b�g
    //            FrequencyCount = 0;
    //        }
    //    }
    //}




}
