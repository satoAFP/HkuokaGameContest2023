using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    [SerializeField, Header("リスポーンまでのフレーム")]
    private int RespownFrame;

    [SerializeField, Header("フィーバータイム")]
    private bool FeverTimeCheck;

    [SerializeField, Header("出現させるオブジェクト")]
    private GameObject SpownEnemy;


    private int FrameCount = 0; //フレームカウント用
    private GameObject clone;
    private bool FeverTime = false;




    // Update is called once per frame
    void FixedUpdate()
    {
        if (!FeverTimeCheck)
        {
            EnemySpawn();
        }
        else
        {
            FeverTime = ManagerAccessor.Instance.systemManager.FeverTime;
            Debug.Log(FeverTime);
            if (FeverTime)
            {
                EnemySpawn();
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
                FrameCount = 0;
            }
            FrameCount++;
        }
    }
}
