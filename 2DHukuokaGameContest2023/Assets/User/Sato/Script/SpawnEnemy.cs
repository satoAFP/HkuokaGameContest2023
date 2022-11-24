using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    /*[SerializeField, Header("�o���p�x"), Range(0, 500)]
    private int FrequencyAppearance;

    [SerializeField, Header("�ő�o����")]
    private int spawnMax;

    [SerializeField, Header("�X�|�[�����鉺���C��")]
    public float spawnUnderLine;

    [SerializeField, Header("�X�|�[������ド�C��")]
    public float spawnOverLine;

    [SerializeField, Header("�X�|�[������X���W")]
    public float spawnXpos;

    [SerializeField, Header("�o���������ړ��G�̎��")]
    public GameObject[] spawnLeftEnemy;

    [SerializeField, Header("�o�������E�ړ��G�̎��")]
    public GameObject[] spawnRightEnemy;*/

    public enum Wave
    {
        GENERALLY,  //�ʏ�
        COMBO,      //�R���{
        SCORE,      //�X�R�A
        BOSS,       //�{�X
    }


    [SerializeField, Header("wave�܂łɕK�v�ȃR���{��")]
    private int NeedCombo;

    [SerializeField, Header("wave�܂łɕK�v�ȃX�R�A��")]
    private int NeedScore;


    //�o�����K�v�ȃf�[�^�x�[�X
    [System.Serializable]
    public struct Base
    {
        public GameObject EnemySet;
        public Vector3 spawnPos;
        public int SpawnFrame;
        public int StopFrame;
    }

    //3�����z��ɂ���p
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
    private int NextSpawnNum = 0;       //���G���o������t���[��


    private int FrameCount = 0;         //�t���[���J�E���g�p
    private int NowArrangement = 0;     //���݂̔z��ԍ�
    private int SpawnCount = 0;         //���̃t���[���ŏo�������G�̐�
    private GameFlow[] ObjGroup;        //���݂ǂ̓G���o�����Ă��邩�L���p



    float elapsedTime = 0;

    [System.NonSerialized]
    public int WaveCombo = 0;           //wave�܂ł̃R���{
    [System.NonSerialized]
    public int WaveScore = 0;           //wave�܂ł̃X�R�A
    [System.NonSerialized]
    public int NowWave = (int)Wave.GENERALLY;//���݂�wave


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


        WaveTransition();

    }

    /// <summary>
    /// �G�ݒ�蓮�֐�
    /// </summary>
    private void FluctuationSpawn()
    {
        //�ݒ肵�Ă���t���[�����ɏ���
        if (GF[NowWave].GFbase[NowArrangement].SpawnFrame == FrameCount)
        {
            for (int i = 0; i < GF[NowWave].GFbase.Length; i++)
            {
                if (GF[NowWave].GFbase[NowArrangement].SpawnFrame == GF[NowWave].GFbase[i].SpawnFrame)
                {
                    if (ObjGroup[NowWave].GFObj[NowArrangement] == null)
                    {
                        //�G����
                        Vector3 MyPos = transform.localPosition;
                        GameObject clone = Instantiate(GF[NowWave].GFbase[i].EnemySet, MyPos + GF[NowWave].GFbase[i].spawnPos, Quaternion.identity);
                        clone.gameObject.GetComponent<BaseEnemyFly>().stopCount = GF[NowWave].GFbase[i].StopFrame;
                        ObjGroup[NowWave].GFObj[i] = clone;
                        FrameCount = GF[NowWave].GFbase[i].SpawnFrame;
                        SpawnCount++;
                    }
                }
            }


            //�g�p���Ă���z�񕪉��Z�����
            if (GF[NowWave].GFbase.Length - 1 > NowArrangement)
            {
                NowArrangement += SpawnCount;
                SpawnCount = 0;
            }
            else
            {
                NowArrangement = 0;
                FrameCount = 0;
            }

        }

        if (GF[NowWave].GFbase[GF[NowWave].GFbase.Length - 1].SpawnFrame == FrameCount)
            FrameCount = 0;

        FrameCount++;
    }


    /// <summary>
    /// wave�J�ڊǗ��p�֐�
    /// </summary>
    private void WaveTransition()
    {
        //wave�J�ڏ���
        if (WaveCombo >= NeedCombo)
        {
            NowWave = (int)Wave.COMBO;
            WaveCombo = 0;
            NowArrangement = 0;
            FrameCount = 0;
            ActiveFalse();
        }
        else if (WaveScore >= NeedScore) 
        {
            NowWave = (int)Wave.SCORE;
            WaveScore = 0;
            NowArrangement = 0;
            FrameCount = 0;
            ActiveFalse();
        }


        //NowWave�̏󋵊Ǘ�
        switch (NowWave)
        {
            case (int)Wave.GENERALLY:
                break;

            case (int)Wave.COMBO:
                elapsedTime += Time.deltaTime;
                if (10 <= elapsedTime)
                {
                    ChangeEnemy();
                    NowWave = (int)Wave.GENERALLY;
                    elapsedTime = 0.0f;
                }
                break;

            case (int)Wave.SCORE:
                elapsedTime += Time.deltaTime;
                if (10 <= elapsedTime)
                {
                    ChangeEnemy();
                    NowWave = (int)Wave.GENERALLY;
                    elapsedTime = 0.0f;
                }
                break;

            case (int)Wave.BOSS:
                break;
        }
    }

    /// <summary>
    /// �ʏ�E�F�[�u�̓G����U���܂�����
    /// </summary>
    private void ActiveFalse()
    {
        for (int i = 0; i < ObjGroup[(int)Wave.GENERALLY].GFObj.Length; i++)
        {
            if (ObjGroup[(int)Wave.GENERALLY].GFObj[i] != null)
            {
                ObjGroup[(int)Wave.GENERALLY].GFObj[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// �ʏ�E�F�[�u�ɖ߂�����
    /// </summary>
    private void ChangeEnemy()
    {
        for (int i = 0; i < ObjGroup[NowWave].GFObj.Length; i++)
        {
            if (ObjGroup[NowWave].GFObj[i] != null)
            {
                Destroy(ObjGroup[NowWave].GFObj[i]);
            }
        }

        for (int i = 0; i < ObjGroup[(int)Wave.GENERALLY].GFObj.Length; i++)
        {
            if (ObjGroup[(int)Wave.GENERALLY].GFObj[i] != null)
            {
                ObjGroup[(int)Wave.GENERALLY].GFObj[i].SetActive(true);
            }
        }
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
