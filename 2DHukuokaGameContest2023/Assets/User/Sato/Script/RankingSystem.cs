using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    [SerializeField, Header("�����L���O�\���p")]
    private Text[] ScoreText;

    [SerializeField, Header("�����L���O�\���p")]
    private Text[] ComboText;

    [SerializeField, Header("�����L���O�\���p")]
    private Text[] TimeText;

    [SerializeField, Header("�����L���O�\�����̃A�N�e�B�u�ɂ���I�u�W�F�N�g")]
    private GameObject[] ActiveObj;

    [SerializeField, Header("�N���b�N����ES")] private AudioClip SE;

    [System.NonSerialized]
    public int[] Score;         //�v�Z�p�X�R�A

    [System.NonSerialized]
    public int[] Combo;         //�v�Z�p�X�R�A

    [System.NonSerialized]
    public int[] Time;         //�v�Z�p�X�R�A

    private void Start()
    {
        ManagerAccessor.Instance.rankingSystem = this;
    }


    public void OnRanking()
    {
        //SE�炷
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        //�I�u�W�F�N�g�\��
        for (int i = 0; i < ActiveObj.Length; i++)
            ActiveObj[i].SetActive(true);

        //�����L���O�\��
        Init();
        WriteScore();
        DisplayRanking();
    }

    public void OfRanking()
    {
        //SE�炷
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        //�I�u�W�F�N�g�\��
        for (int i = 0; i < ActiveObj.Length; i++)
            ActiveObj[i].SetActive(false);
    }


    //�����L���O�̕\��
    public void DisplayRanking()
    {
        //�\�ʂ܂ŕ\��
        for (int i = 0; i < ScoreText.Length; i++)
        {
            if (i + 1 < 10)
            {
                ScoreText[i].text = " " + (i + 1) + "�ʁ@" + Score[i].ToString();
                ComboText[i].text = " " + (i + 1) + "�ʁ@" + Combo[i].ToString();
                TimeText[i].text = " " + (i + 1) + "�ʁ@" + Time[i].ToString();
            }
            else
            {
                ScoreText[i].text = (i + 1) + "�ʁ@" + Score[i].ToString();
                ComboText[i].text = (i + 1) + "�ʁ@" + Combo[i].ToString();
                TimeText[i].text = (i + 1) + "�ʁ@" + Time[i].ToString();
            }
        }
    }

    /// <summary>
    /// �\�[�g����
    /// </summary>
    public void Sort()
    {
        int max = 0;
        int max_pos = 0;
        //�X�R�A�̃\�[�g
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
        //�R���{�̃\�[�g
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
        //�^�C���̃\�[�g
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

    //������
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


    //�X�R�A�L���p�֐�
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

    //�X�R�A�ǂݍ��ݗp�֐�
    public void WriteScore()
    {
        for (int i = 0; i < Score.Length; i++)
        {
            Score[i] = PlayerPrefs.GetInt("SCORE" + i, 0);
            Combo[i] = PlayerPrefs.GetInt("COMBO" + i, 0);
            Time[i] = PlayerPrefs.GetInt("TIME" + i, 0);
        }
    }

    //�����L���O�f�[�^�����p
    public void DeleteScore()
    {
        //SE�炷
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        PlayerPrefs.DeleteAll();
    }

}
