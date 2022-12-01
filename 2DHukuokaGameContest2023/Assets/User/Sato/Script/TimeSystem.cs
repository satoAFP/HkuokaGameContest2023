using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    [SerializeField, Header("���b�ŃQ�[���I��邩")]
    private int EndTime;

    [SerializeField, Header("�^�C���\���p�e�L�X�g")]
    private Text TimeText;

    [SerializeField, Header("�����L���O�p�l��")]
    private GameObject RankingPanel;

    private int FrameCount = 0;     //�t���[���J�E���g�p
    private bool first = true;      //�ŏ��̈�񂾂�����鏈��
    private RankingSystem ranking;  //�����L���O�V�X�e��

    private bool first2 = true;


    private void Start()
    {
        ranking = RankingPanel.GetComponent<RankingSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FrameCount++;


        //���Ԃ̐ݒ�
        if (first)
        {
            ManagerAccessor.Instance.systemManager.Time = EndTime;
            first = false;
        }

        //�Q�[���X�^�[�g������^�C���J�E���g
        if (ManagerAccessor.Instance.systemManager.Time > 0)
        {
            if (FrameCount % 50 == 0)
                ManagerAccessor.Instance.systemManager.Time--;
            TimeText.text = ManagerAccessor.Instance.systemManager.Time.ToString();
        }
        else
        {
            if (first2)
            {
                RankingPanel.SetActive(true);

                ranking.Init();
                ranking.WriteScore();
                ranking.Score[10] = ManagerAccessor.Instance.systemManager.Score;
                ranking.Sort();
                ranking.MemScore();

                first2 = false;
            }
        }
    }

    public void Reload()
    {
        ManagerAccessor.Instance.sceneManager.SceneReload();
    }
}
