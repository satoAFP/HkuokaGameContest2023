using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField, Header("グラフの針")] private GameObject[] Needle;

    [SerializeField, Header("グラフのスコア最大値")] private int MaxScore;
    [SerializeField, Header("グラフのスコア限界突破値")] private int OverScore;
    [SerializeField, Header("グラフのコンボ最大値")] private int MaxCombo;
    [SerializeField, Header("グラフのコンボ限界突破値")] private int OverCombo;
    [SerializeField, Header("グラフのタイム最大値")] private int MaxTime;
    [SerializeField, Header("グラフのタイム限界突破値")] private int OverTime;

    [SerializeField, Header("限界突破時の拡大量")] private float OverNeedle;


    private int score = 0;      //計算用
    private int allcombo = 0;
    private int time = 0;

    private float hps = 0;//height/MaxScore
    private float hpc = 0;//height/MaxCombo
    private float hpt = 0;//height/MaxTime


    // Start is called before the first frame update
    void Start()
    {
        //それぞれの情報取得
        score = ManagerAccessor.Instance.systemManager.Score;
        allcombo = ManagerAccessor.Instance.systemManager.AllCombo;
        time = ManagerAccessor.Instance.systemManager.Time;

        //1％当たりの数値
        hps = Needle[0].GetComponent<RectTransform>().sizeDelta.y / MaxScore;
        hpc = Needle[1].GetComponent<RectTransform>().sizeDelta.y / MaxCombo;
        hpt = Needle[2].GetComponent<RectTransform>().sizeDelta.y / MaxTime;

        

        if (score <= MaxScore)
            Needle[0].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hps * score);
        else if (score > MaxScore && score < OverScore)
            Needle[0].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hps * MaxScore);
        else
            Needle[0].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hps * MaxScore * OverNeedle);


        if (allcombo <= MaxCombo)
            Needle[1].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpc * allcombo);
        else if (allcombo > MaxCombo && allcombo < OverCombo)
            Needle[1].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpc * MaxCombo);
        else
            Needle[1].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpc * MaxCombo * OverNeedle);


        if (time <= MaxTime)
            Needle[2].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpt * time);
        else if (time > MaxTime && time < OverTime)
            Needle[2].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpt * MaxTime);
        else
            Needle[2].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpt * MaxTime * OverNeedle);
    }

}
