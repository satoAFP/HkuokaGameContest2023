using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField, Header("��_")]
    private GameObject WeakPoint;

    [SerializeField, Header("��_�̈ʒu")]
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

            //�{�X�̎�_�o��
            clone = Instantiate(WeakPoint, WeakPos[random].transform.position, Quaternion.identity);
        }
    }
}
