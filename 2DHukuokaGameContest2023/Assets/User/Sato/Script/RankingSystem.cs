using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    [SerializeField, Header("�����L���O�\���p")]
    private Text[] RankingText;

    [System.NonSerialized]
    public int[] Score;         //�v�Z�p�X�R�A


    // Update is called once per frame
    void Update()
    {
        //�\�ʂ܂ŕ\��
        for (int i = 0; i < RankingText.Length; i++)
            RankingText[i].text = (i + 1) + "�ʁ@" + Score[i].ToString();
    }

    /// <summary>
    /// �\�[�g����
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

    //������
    public void Init()
    {
        Score = new int[RankingText.Length + 1];
        for (int i = 0; i < Score.Length; i++)
            Score[i] = 0;
    }


    //�X�R�A�L���p�֐�
    public void MemScore()
    {
        for (int i = 0; i < Score.Length; i++)
            PlayerPrefs.SetInt("SCORE" + i, Score[i]);
        PlayerPrefs.Save();
    }

    //�X�R�A�ǂݍ��ݗp�֐�
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
