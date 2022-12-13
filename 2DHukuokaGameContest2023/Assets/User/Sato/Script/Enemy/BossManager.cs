using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossManager : MonoBehaviour
{
    [SerializeField, Header("エフェクトとオブジェクトの出現時間のずれ")]
    private int Lag;

    [SerializeField, Header("弱点")]
    private GameObject WeakPoint;

    [SerializeField, Header("出現エフェクト")]
    private GameObject eff;

    [SerializeField, Header("弱点の位置")]
    private GameObject[] WeakPos;

    [SerializeField, Header("ボス倒した後消えるオブジェクト")]
    private SpriteRenderer[] DeleteObj;


    private GameObject clone = null;        //クローンのオブジェクト
    private int count = 0;                  //フレームカウント用
    private bool first = true;              //一回しかしない処理
    private int random = 0;                 //出現位置らんだむ

    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.bossManager = this;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int bosshp = ManagerAccessor.Instance.systemManager.BossHP;
        if (clone == null) 
        {
            if (bosshp > 0)
            {
                count++;

                //ボスの弱点出現
                if (first)
                {
                    random = Random.Range(0, WeakPos.Length);
                    Instantiate(eff, WeakPos[random].transform.position, Quaternion.identity);
                }

                first = false;
                if (count > Lag)
                {
                    clone = Instantiate(WeakPoint, WeakPos[random].transform.position, Quaternion.identity);
                    clone.GetComponent<SpriteRenderer>().color = new Color(1, (float)bosshp / 10, (float)bosshp / 10, 1);
                    count = 0;
                    first = true;
                }
            }
        }

        //ボスが死んだときボスをだんだん薄くする
        if (bosshp <= 0)
        {
            for (int i = 0; i < DeleteObj.Length; i++)
                DeleteObj[i].color -= new Color(0, 0, 0, 1f / (ManagerAccessor.Instance.systemManager.BossDethTime * 50));
        }
    }
}
