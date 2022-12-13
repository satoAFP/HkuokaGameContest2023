using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossManager : MonoBehaviour
{
    [SerializeField, Header("�G�t�F�N�g�ƃI�u�W�F�N�g�̏o�����Ԃ̂���")]
    private int Lag;

    [SerializeField, Header("��_")]
    private GameObject WeakPoint;

    [SerializeField, Header("�o���G�t�F�N�g")]
    private GameObject eff;

    [SerializeField, Header("��_�̈ʒu")]
    private GameObject[] WeakPos;

    [SerializeField, Header("�{�X�|�����������I�u�W�F�N�g")]
    private SpriteRenderer[] DeleteObj;


    private GameObject clone = null;        //�N���[���̃I�u�W�F�N�g
    private int count = 0;                  //�t���[���J�E���g�p
    private bool first = true;              //��񂵂����Ȃ�����
    private int random = 0;                 //�o���ʒu��񂾂�

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

                //�{�X�̎�_�o��
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

        //�{�X�����񂾂Ƃ��{�X�����񂾂񔖂�����
        if (bosshp <= 0)
        {
            for (int i = 0; i < DeleteObj.Length; i++)
                DeleteObj[i].color -= new Color(0, 0, 0, 1f / (ManagerAccessor.Instance.systemManager.BossDethTime * 50));
        }
    }
}
