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
            TimeText.text = "�c��:" + ManagerAccessor.Instance.systemManager.Time + "�b";
        }
        else
        {
            RankingPanel.SetActive(true);
        }


    }
}
