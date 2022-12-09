using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossManager : MonoBehaviour
{
    [SerializeField, Header("��_")]
    private GameObject WeakPoint;

    [SerializeField, Header("��_�̈ʒu")]
    private GameObject[] WeakPos;

    [SerializeField, Header("�{�X�|�����������I�u�W�F�N�g")]
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

                //�{�X�̎�_�o��
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
