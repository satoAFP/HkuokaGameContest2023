using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField, Header("弱点")]
    private GameObject WeakPoint;

    [SerializeField, Header("弱点の位置")]
    private GameObject[] WeakPos;


    private int WeakPosNum = 0;
    private GameObject clone = null;

    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.bossManager = this;

        WeakPosNum = WeakPos.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (clone == null) 
        {
            int random = Random.Range(0, WeakPosNum);

            //ボスの弱点出現
            clone = Instantiate(WeakPoint, WeakPos[random].transform.position, Quaternion.identity);
        }
    }
}
