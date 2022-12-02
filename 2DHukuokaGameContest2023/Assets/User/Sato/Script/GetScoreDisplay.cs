using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreDisplay : MonoBehaviour
{
    [SerializeField, Header("移動量")]
    private float MovePower;

    [SerializeField, Header("表示させたいテキスト")]
    private GameObject ScoreText;


    private int MemScore = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //スコアが増えた時
        if (MemScore < ManagerAccessor.Instance.systemManager.Score) 
        {
            MemScore = ManagerAccessor.Instance.systemManager.Score;
        }
    }
}
