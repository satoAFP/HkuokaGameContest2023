using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossManager : MonoBehaviour
{
    [SerializeField, Header("弱点")]
    private GameObject WeakPoint;

    [SerializeField, Header("弱点の位置")]
    private GameObject[] WeakPos;

    [SerializeField, Header("ボス倒した後消えるオブジェクト")]
    private SpriteRenderer[] DeleteObj;



    private int WeakPosNum = 0;
    private GameObject clone = null;

    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.bossManager = this;

        WeakPosNum = WeakPos.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (clone == null) 
        {
            if (ManagerAccessor.Instance.systemManager.BossHP > 0)
            {
                int random = Random.Range(0, WeakPosNum);

                //ボスの弱点出現
                clone = Instantiate(WeakPoint, WeakPos[random].transform.position, Quaternion.identity);
            }
        }


        if (ManagerAccessor.Instance.systemManager.BossHP <= 0)
        {
            for (int i = 0; i < DeleteObj.Length; i++)
                DeleteObj[i].color -= new Color(0, 0, 0, 1f / (ManagerAccessor.Instance.systemManager.BossDethTime * 50));
        }
    }
}
