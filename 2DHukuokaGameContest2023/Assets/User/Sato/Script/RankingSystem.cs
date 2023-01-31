using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    [SerializeField, Header("ランキング表示用")]
    private Text[] RankingText;

    [SerializeField, Header("ランキング表示時のアクティブにするオブジェクト")]
    private GameObject[] ActiveObj;

    [SerializeField, Header("クリック時のES")] private AudioClip SE;

    [System.NonSerialized]
    public int[] Score;         //計算用スコア

    private void Start()
    {
        ManagerAccessor.Instance.rankingSystem = this;
    }


    public void OnRanking()
    {
        //SE鳴らす
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        //オブジェクト表示
        for (int i = 0; i < ActiveObj.Length; i++)
            ActiveObj[i].SetActive(true);

        //ランキング表示
        Init();
        WriteScore();
        DisplayRanking();
    }

    public void OfRanking()
    {
        //SE鳴らす
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        //オブジェクト表示
        for (int i = 0; i < ActiveObj.Length; i++)
            ActiveObj[i].SetActive(false);
    }


    //ランキングの表示
    public void DisplayRanking()
    {
        //十位まで表示
        for (int i = 0; i < RankingText.Length; i++)
        {
            if (i + 1 < 10)
                RankingText[i].text = " " + (i + 1) + "位　" + Score[i].ToString();
            else
                RankingText[i].text = (i + 1) + "位　" + Score[i].ToString();
        }
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

    //ランキングデータ消去用
    public void DeleteScore()
    {
        //SE鳴らす
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        PlayerPrefs.DeleteAll();
    }

}
