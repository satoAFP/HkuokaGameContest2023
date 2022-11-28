using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    [SerializeField, Header("ランキング表示用")]
    private Text[] RankingText;

    [System.NonSerialized]
    public int[] Score;         //計算用スコア


    // Update is called once per frame
    void Update()
    {
        //十位まで表示
        for (int i = 0; i < RankingText.Length; i++)
            RankingText[i].text = (i + 1) + "位　" + Score[i].ToString();
    }

    /// <summary>
    /// ソート処理
    /// </summary>
    public void Sort()
    {
        int max = 0;
        int max_pos = 0;

        for (int i = 0; i < Score.Length; i++)
        {
            max = Score[i];
            max_pos = i;
            for (int j = i + 1; j < Score.Length; j++)
            {
                if (Score[i] < Score[j])
                {
                    if (max < Score[j])
                    {
                        max = Score[j];
                        max_pos = j;
                    }
                }
            }
            Score[max_pos] = Score[i];
            Score[i] = max;
        }
    }

    //初期化
    public void Init()
    {
        Score = new int[RankingText.Length + 1];
        for (int i = 0; i < Score.Length; i++)
            Score[i] = 0;
    }


    //スコア記憶用関数
    public void MemScore()
    {
        for (int i = 0; i < Score.Length; i++)
            PlayerPrefs.SetInt("SCORE" + i, Score[i]);
        PlayerPrefs.Save();
    }

    //スコア読み込み用関数
    public void WriteScore()
    {
        for (int i = 0; i < Score.Length; i++)
            Score[i] = PlayerPrefs.GetInt("SCORE" + i, 0);
    }

    public void DeleteScore()
    {
        PlayerPrefs.DeleteAll();
    }

}
