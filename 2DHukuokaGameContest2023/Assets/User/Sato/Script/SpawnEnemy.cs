using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //[SerializeField, Header("出現頻度"),Range(0,500)]
    //private int FrequencyAppearance;

    //[SerializeField, Header("最大出現数")]
    //private int spawnMax;

    //[SerializeField, Header("スポーンする下ライン")]
    //public float spawnUnderLine;

    //[SerializeField, Header("スポーンする上ライン")]
    //public float spawnOverLine;

    //[SerializeField, Header("スポーンするX座標")]
    //public float spawnXpos;

    //[SerializeField,Header("出したい左移動敵の種類")]
    //public GameObject[] spawnLeftEnemy;

    //[SerializeField, Header("出したい右移動敵の種類")]
    //public GameObject[] spawnRightEnemy;

    [Header("現在のWave")]
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
    }


    [SerializeField, Header("敵出現フロー作成用")]
    private GameFlow[] GF;

    private int FrequencyCount = 0;     //出現頻度カウント用
    private int spawnCount = 0;         //スポーン数カウント用
    private int FrameCount = 0;         //フレームカウント用
    private int NextSpawnNum = 0;       //次敵が出現するフレーム
    private int NowArrangement = 0;     //現在の配列番号

    // Update is called once per frame
    void FixedUpdate()
    {
        FluctuationSpawn();

        


    }

    /// <summary>
    /// 敵設定手動関数
    /// </summary>
    private void FluctuationSpawn()
    {
        //設定しているフレーム毎に召喚
        if (GF[Wave].GFbase[NowArrangement].SpawnFrame == FrameCount)
        {
            //敵生成
            Vector3 MyPos = transform.localPosition;
            GameObject clone = Instantiate(GF[Wave].GFbase[NowArrangement].EnemySet, MyPos + GF[Wave].GFbase[NowArrangement].spawnPos, Quaternion.identity);
            clone.gameObject.GetComponent<BaseEnemyFly>().stopCount = GF[Wave].GFbase[NowArrangement].StopFrame;

            //使用している配列分加算される
            if (GF.Length - 1 > NowArrangement)
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
