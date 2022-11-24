using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    [SerializeField, Header("初期スポーンまでのフレーム")]
    private int FirstSpownFrame;

    [SerializeField, Header("リスポーンまでのフレーム")]
    private int RespownFrame;

    [SerializeField, Header("フィーバータイム")]
    private bool FeverTimeCheck;

    [SerializeField, Header("出現させるオブジェクト")]
    private GameObject SpownEnemy;


    private int FrameCount = 0;     //フレームカウント用
    private GameObject clone;       //クローンしたオブジェクトを入れるよう
    private bool FeverTime = false; //フィーバーモード取得用
    private int MemRespownFrame = 0;//リスポーンまでの時間記憶用


    private void Start()
    {
        //初期スポーンだけタイミングずらすため
        MemRespownFrame = RespownFrame;
        RespownFrame = FirstSpownFrame;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!FeverTimeCheck)//通常モード
        {
            EnemySpawn();
        }
        else//フィーバーモード
        {
            FeverTime = ManagerAccessor.Instance.systemManager.FeverTime;
            if (FeverTime)
            {
                EnemySpawn();
            }
            else
            {
                if (clone != null)
                    Destroy(clone);
            }
        }
    }


    /// <summary>
    /// 敵召喚用関数
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
