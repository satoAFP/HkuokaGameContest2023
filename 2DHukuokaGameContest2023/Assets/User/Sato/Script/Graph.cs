using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField, Header("グラフの針")] private Text EvaluationText;


    private int score = 0;      //計算用
    private int allcombo = 0;
    private int time = 0;

    private float hps = 0;//height/MaxScore
    private float hpc = 0;//height/MaxCombo
    private float hpt = 0;//height/MaxTime

    private bool[] MaxEvaluationFrag = new bool[3];
    private bool[] OverEvaluationFrag = new bool[3];


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

        //初期化
        for(int i=0;i<3;i++)
        {
            MaxEvaluationFrag[i] = false;
            OverEvaluationFrag[i] = false;
        }


        if (score <= MaxScore)
        {
            Needle[0].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hps * score);
        }
        else if (score > MaxScore && score < OverScore)
        {
            Needle[0].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hps * MaxScore);
            MaxEvaluationFrag[0] = true;
        }
        else
        {
            Needle[0].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hps * MaxScore * OverNeedle);
            OverEvaluationFrag[0] = true;
        }


        if (allcombo <= MaxCombo)
        {
            Needle[1].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpc * allcombo);
        }
        else if (allcombo > MaxCombo && allcombo < OverCombo)
        {
            Needle[1].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpc * MaxCombo);
            MaxEvaluationFrag[1] = true;
        }
        else
        {
            Needle[1].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpc * MaxCombo * OverNeedle);
            OverEvaluationFrag[1] = true;
        }


        if (time <= MaxTime)
        {
            Needle[2].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpt * time);
        }
        else if (time > MaxTime && time < OverTime)
        {
            Needle[2].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpt * MaxTime);
            MaxEvaluationFrag[2] = true;
        }
        else
        {
            Needle[2].GetComponent<RectTransform>().sizeDelta = new Vector2(100, hpt * MaxTime * OverNeedle);
            OverEvaluationFrag[2] = true;
        }


        int maxcount = 0;
        for (int i = 0; i < 3; i++)
            if (MaxEvaluationFrag[i])
                maxcount++;

        int overcount = 0;
        for (int i = 0; i < 3; i++)
            if (OverEvaluationFrag[i])
                overcount++;

        if (overcount == 2)
            EvaluationText.text = "リミットブレイカー";
        else if (overcount == 1)
        {
            if (OverEvaluationFrag[0])
                EvaluationText.text = "スコアゴッド";
            if (OverEvaluationFrag[1])
                EvaluationText.text = "アタックゴッド";
            if (OverEvaluationFrag[2])
                EvaluationText.text = "スピードゴッド";
        }
        else if (maxcount == 3)
            EvaluationText.text = "スラッシュゴッド";
        else if (maxcount == 2)
            EvaluationText.text = "スラッシュキング";
        else if (maxcount == 1)
        {
            if (MaxEvaluationFrag[0])
                EvaluationText.text = "スコアキング";
            if (MaxEvaluationFrag[1])
                EvaluationText.text = "アタックキング";
            if (MaxEvaluationFrag[2])
                EvaluationText.text = "スピードキング";
        }
        else
        {
            EvaluationText.text = "向いてない";
        }

    }

}
