using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    [SerializeField, Header("初期スポーンまでのフレーム")]
    private int FirstSpownFrame;

    [SerializeField, Header("リスポーンまでのフレーム")]
    private int RespownFrame;

    [SerializeField, Header("エフェクトをずらして表示するためのフレーム")]
    private int EffectFrame;

    [SerializeField, Header("フィーバータイム")]
    private bool FeverTimeCheck;

    [SerializeField, Header("点滅までのフレーム")]
    private int TikatikaFrame;

    [SerializeField, Header("点滅の間隔")]
    private int TikatikaInterval;


    [SerializeField, Header("出現させるオブジェクト")]
    private GameObject SpownEnemy;

    [SerializeField, Header("出現エフェクト")]
    private GameObject SpownEffect;


    private int FrameCount = 0;     //フレームカウント用
    private GameObject clone;       //クローンしたオブジェクトを入れるよう
    private bool FeverTime = false; //フィーバーモード取得用
    private int MemRespownFrame = 0;//リスポーンまでの時間記憶用
    private Color ObjColor;         //クローンしたオブジェクトの色情報
    private bool AlphInversion = false;//アルファ変更用
    private int AlphFrameCount = 0; //アルファ変更用フレームカウント

    private void Start()
    {
        //初期スポーンだけタイミングずらすため
        MemRespownFrame = RespownFrame;
        RespownFrame = FirstSpownFrame;

        ObjColor = new Color(1, 1, 1, 0);
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

                        //アルファ切り替え
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
                //初期スポーンだけタイミングずらすため
                RespownFrame = FirstSpownFrame;
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

    //アルファ変更用
    private void AlphChange(float a)
    {
        ObjColor.a = a;
        clone.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = ObjColor;
        clone.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = ObjColor;
        clone.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = ObjColor;
    }
}
