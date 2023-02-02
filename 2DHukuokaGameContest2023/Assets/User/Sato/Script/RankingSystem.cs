using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    [SerializeField, Header("ランキング表示用")]
    private Text[] ScoreText;

    [SerializeField, Header("ランキング表示用")]
    private Text[] ComboText;

    [SerializeField, Header("ランキング表示用")]
    private Text[] TimeText;

    [SerializeField, Header("ランキング表示時のアクティブにするオブジェクト")]
    private GameObject[] ActiveObj;

    [SerializeField, Header("クリック時のES")] private AudioClip SE;

    [System.NonSerialized]
    public int[] Score;         //計算用スコア

    [System.NonSerialized]
    public int[] Combo;         //計算用スコア

    [System.NonSerialized]
    public int[] Time;         //計算用スコア

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
        for (int i = 0; i < ScoreText.Length; i++)
        {
            if (i + 1 < 10)
            {
                ScoreText[i].text = " " + (i + 1) + "位　" + Score[i].ToString();
                ComboText[i].text = " " + (i + 1) + "位　" + Combo[i].ToString();
                TimeText[i].text = " " + (i + 1) + "位　" + Time[i].ToString();
            }
            else
            {
                ScoreText[i].text = (i + 1) + "位　" + Score[i].ToString();
                ComboText[i].text = (i + 1) + "位　" + Combo[i].ToString();
                TimeText[i].text = (i + 1) + "位　" + Time[i].ToString();
            }
        }
    }

    /// <summary>
    /// ソート処理
    /// </summary>
    public void Sort()
    {
        int max = 0;
        int max_pos = 0;
        //スコアのソート
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

        max = 0;
        max_pos = 0;
        //コンボのソート
        for (int i = 0; i < Combo.Length; i++)
        {
            max = Combo[i];
            max_pos = i;
            for (int j = i + 1; j < Combo.Length; j++)
            {
                if (Combo[i] < Combo[j])
                {
                    if (max < Combo[j])
                    {
                        max = Combo[j];
                        max_pos = j;
                    }
                }
            }
            Combo[max_pos] = Combo[i];
            Combo[i] = max;
        }

        max = 0;
        max_pos = 0;
        //タイムのソート
        for (int i = 0; i < Time.Length; i++)
        {
            max = Time[i];
            max_pos = i;
            for (int j = i + 1; j < Time.Length; j++)
            {
                if (Time[i] < Time[j])
                {
                    if (max < Time[j])
                    {
                        max = Time[j];
                        max_pos = j;
                    }
                }
            }
            Time[max_pos] = Time[i];
            Time[i] = max;
        }
    }

    //初期化
    public void Init()
    {
        Score = new int[ScoreText.Length + 1];
        Combo = new int[ComboText.Length + 1];
        Time = new int[TimeText.Length + 1];
        for (int i = 0; i < Score.Length; i++)
        {
            Score[i] = 0;
            Combo[i] = 0;
            Time[i] = 0;
        }
    }


    //スコア記憶用関数
    public void MemScore()
    {
        for (int i = 0; i < Score.Length; i++)
        {
            PlayerPrefs.SetInt("SCORE" + i, Score[i]);
            PlayerPrefs.SetInt("COMBO" + i, Combo[i]);
            PlayerPrefs.SetInt("TIME" + i, Time[i]);
        }
        PlayerPrefs.Save();
    }

    //スコア読み込み用関数
    public void WriteScore()
    {
        for (int i = 0; i < Score.Length; i++)
        {
            Score[i] = PlayerPrefs.GetInt("SCORE" + i, 0);
            Combo[i] = PlayerPrefs.GetInt("COMBO" + i, 0);
            Time[i] = PlayerPrefs.GetInt("TIME" + i, 0);
        }
    }

    //ランキングデータ消去用
    public void DeleteScore()
    {
        //SE鳴らす
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        PlayerPrefs.DeleteAll();
    }

}
