using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    /*[SerializeField, Header("出現頻度"), Range(0, 500)]
    private int FrequencyAppearance;

    [SerializeField, Header("最大出現数")]
    private int spawnMax;

    [SerializeField, Header("スポーンする下ライン")]
    public float spawnUnderLine;

    [SerializeField, Header("スポーンする上ライン")]
    public float spawnOverLine;

    [SerializeField, Header("スポーンするX座標")]
    public float spawnXpos;

    [SerializeField, Header("出したい左移動敵の種類")]
    public GameObject[] spawnLeftEnemy;

    [SerializeField, Header("出したい右移動敵の種類")]
    public GameObject[] spawnRightEnemy;*/

    public enum Wave
    {
        GENERALLY,  //通常
        COMBO,      //コンボ
        SCORE,      //スコア
        BOSS,       //ボス
    }


    [SerializeField, Header("waveまでに必要なコンボ数")]
    private int NeedCombo;

    [SerializeField, Header("waveまでに必要なスコア数")]
    private int NeedScore;


    //出現時必要なデータベース
    [System.Serializable]
    public struct Base
    {
        public GameObject EnemySet;
        public Vector3 spawnPos;
        public int SpawnFrame;
        public int StopFrame;
    }

    //3次元配列にする用
    [System.Serializable]
    public struct GameFlow
    {
        public Base[] GFbase;
        [System.NonSerialized]
        public GameObject[] GFObj;
    }
    [SerializeField, Header("敵出現フロー作成用")]
    private GameFlow[] GF;


    private int FrequencyCount = 0;     //出現頻度カウント用
    private int spawnCount = 0;         //スポーン数カウント用
    private int NextSpawnNum = 0;       //次敵が出現するフレーム


    private int FrameCount = 0;         //フレームカウント用
    private int NowArrangement = 0;     //現在の配列番号
    private GameFlow[] ObjGroup;        //現在どの敵が出現しているか記憶用



    float elapsedTime = 0;

    [System.NonSerialized]
    public int WaveCombo = 0;           //waveまでのコンボ
    [System.NonSerialized]
    public int WaveScore = 0;           //waveまでのスコア
    [System.NonSerialized]
    public int NowWave = (int)Wave.GENERALLY;//現在のwave


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
    /// 敵設定手動関数
    /// </summary>
    private void FluctuationSpawn()
    {
        //設定しているフレーム毎に召喚
        if (GF[NowWave].GFbase[NowArrangement].SpawnFrame == FrameCount)
        {
            if (ObjGroup[NowWave].GFObj[NowArrangement] == null)
            {
                //敵生成
                Vector3 MyPos = transform.localPosition;
                GameObject clone = Instantiate(GF[NowWave].GFbase[NowArrangement].EnemySet, MyPos + GF[NowWave].GFbase[NowArrangement].spawnPos, Quaternion.identity);
                clone.gameObject.GetComponent<BaseEnemyFly>().stopCount = GF[NowWave].GFbase[NowArrangement].StopFrame;
                ObjGroup[NowWave].GFObj[NowArrangement] = clone;
            }

            //使用している配列分加算される
            if (GF[NowWave].GFbase.Length - 1 > NowArrangement)
                NowArrangement++;
            else
            {
                NowArrangement = 0;
                FrameCount = 0;
            }
        }

        FrameCount++;
    }


    /// <summary>
    /// wave遷移管理用関数
    /// </summary>
    private void WaveTransition()
    {
        //wave遷移条件
        if (WaveCombo >= NeedCombo)
        {
            NowWave = (int)Wave.COMBO;
            WaveCombo = 0;
        }
        else if (WaveScore >= NeedScore) 
        {
            NowWave = (int)Wave.SCORE;
            WaveScore = 0;
        }


        //NowWaveの状況管理
        switch (NowWave)
        {
            case (int)Wave.GENERALLY:
                break;

            case (int)Wave.COMBO:
                elapsedTime += Time.deltaTime;
                if (10 <= elapsedTime)
                {
                    NowWave = (int)Wave.GENERALLY;
                    elapsedTime = 0.0f;
                }
                break;

            case (int)Wave.SCORE:
                elapsedTime += Time.deltaTime;
                if (10 <= elapsedTime)
                {
                    NowWave = (int)Wave.GENERALLY;
                    elapsedTime = 0.0f;
                }
                break;

            case (int)Wave.BOSS:
                break;
        }
    }



    //private void FixedSpawn()
    //{
    //    if (spawnMax == spawnCount)
    //    {
    //        FrequencyCount++;
    //        if (FrequencyAppearance == FrequencyCount)
    //        {
    //            //左出現
    //            if (Random.Range(0, 2) == 0)
    //            {
    //                GameObject clone = Instantiate(spawnLeftEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
    //            }
    //            //右出現
    //            else
    //            {
    //                GameObject clone = Instantiate(spawnRightEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(-spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
    //            }
    //            //スポーンカウント
    //            spawnCount++;

    //            //カウントリセット
    //            FrequencyCount = 0;
    //        }
    //    }
    //}




}
