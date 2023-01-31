using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField, Header("�O���t�̐j")] private GameObject[] Needle;

    [SerializeField, Header("�O���t�̃X�R�A�ő�l")] private int MaxScore;
    [SerializeField, Header("�O���t�̃X�R�A���E�˔j�l")] private int OverScore;
    [SerializeField, Header("�O���t�̃R���{�ő�l")] private int MaxCombo;
    [SerializeField, Header("�O���t�̃R���{���E�˔j�l")] private int OverCombo;
    [SerializeField, Header("�O���t�̃^�C���ő�l")] private int MaxTime;
    [SerializeField, Header("�O���t�̃^�C�����E�˔j�l")] private int OverTime;

    [SerializeField, Header("���E�˔j���̊g���")] private float OverNeedle;

    [SerializeField, Header("�O���t�̐j")] private Text EvaluationText;


    private int score = 0;      //�v�Z�p
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
        //���ꂼ��̏��擾
        score = ManagerAccessor.Instance.systemManager.Score;
        allcombo = ManagerAccessor.Instance.systemManager.AllCombo;
        time = ManagerAccessor.Instance.systemManager.Time;

        //1��������̐��l
        hps = Needle[0].GetComponent<RectTransform>().sizeDelta.y / MaxScore;
        hpc = Needle[1].GetComponent<RectTransform>().sizeDelta.y / MaxCombo;
        hpt = Needle[2].GetComponent<RectTransform>().sizeDelta.y / MaxTime;

        //������
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

        if (overcount == 3)
            EvaluationText.text = "�����ɗ�������";
        else if (overcount == 2)
            EvaluationText.text = "���~�b�g�u���C�J�[";
        else if (overcount == 1)
        {
            if (OverEvaluationFrag[0])
                EvaluationText.text = "�X�R�A�S�b�h";
            if (OverEvaluationFrag[1])
                EvaluationText.text = "�A�^�b�N�S�b�h";
            if (OverEvaluationFrag[2])
                EvaluationText.text = "�X�s�[�h�S�b�h";
        }
        else if (maxcount == 3)
            EvaluationText.text = "�X���b�V���S�b�h";
        else if (maxcount == 2)
            EvaluationText.text = "�X���b�V���L���O";
        else if (maxcount == 1)
        {
            if (MaxEvaluationFrag[0])
                EvaluationText.text = "�X�R�A�L���O";
            if (MaxEvaluationFrag[1])
                EvaluationText.text = "�A�^�b�N�L���O";
            if (MaxEvaluationFrag[2])
                EvaluationText.text = "�X�s�[�h�L���O";
        }
        else
        {
            EvaluationText.text = "�����ĂȂ�";
        }

    }

}
