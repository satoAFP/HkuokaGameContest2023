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


    [SerializeField, Header("�J�����R���C�_�[")]
    private GameObject CColl;

    public CinemachineConfiner camera;


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


        if (ManagerAccessor.Instance.systemManager.BossHP > 0)
        {
            
        }
    }
}
