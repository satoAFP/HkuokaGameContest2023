using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    private int[] Score = new int[10];

    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
            Score[i] = PlayerPrefs.GetInt("SCORE" +[i], 0);
    }

    // Update is called once per frame
    void Update()
    {
        Sort();

        if (first)
        {
            for (int i = 0; i < Score.Length; i++)
                Debug.Log(Score[i]);
            first = false;
        }
    }


    private void Sort()
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


    public void MemScore()
    {
        for (int i = 0; i < 10; i++)
            PlayerPrefs.SetInt("SCORE" + i, Score[i]);
        PlayerPrefs.Save();
    }

}
