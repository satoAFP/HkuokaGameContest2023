using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreDisplay : MonoBehaviour
{
    [SerializeField, Header("�ړ���")]
    private float MovePower;

    [SerializeField, Header("�\�����������e�L�X�g")]
    private GameObject ScoreText;


    private int MemScore = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�X�R�A����������
        if (MemScore < ManagerAccessor.Instance.systemManager.Score) 
        {
            MemScore = ManagerAccessor.Instance.systemManager.Score;
        }
    }
}
