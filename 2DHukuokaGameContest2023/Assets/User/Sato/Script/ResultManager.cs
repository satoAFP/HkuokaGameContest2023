using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField, Header("�\���܂ł̑҂�����")]
    private int WaitFrame;

    [SerializeField, Header("���Ԃɕ\������I�u�W�F�N�g")]
    private GameObject[] DisplayObj;

    [SerializeField, Header("�X�R�A�p�e�L�X�g")]
    private Text ScoreText;

    [SerializeField, Header("�}�b�N�X�R���{�p�e�L�X�g")]
    private Text MaxComboText;

    [SerializeField, Header("�c��^�C���p�e�L�X�g")]
    private Text TimeText;


    private int FrameCount = 0;     //�t���[���J�E���g�p
    private int ObjCount = 0;       //�I�u�W�F�N�g�J�E���g�p


    // Update is called once per frame
    void FixedUpdate()
    {
        FrameCount++;

        //�X�R�A�A�R���{�A�c��^�C���\���p
        ScoreText.text = ManagerAccessor.Instance.systemManager.Score.ToString();
        MaxComboText.text = ManagerAccessor.Instance.systemManager.AllCombo.ToString();
        TimeText.text = ManagerAccessor.Instance.systemManager.Time.ToString() + "�b";

        //���U���g���Ԃɏo���p�̏���
        if (WaitFrame <= FrameCount) 
        {
            if (DisplayObj.Length > ObjCount)
            {
                DisplayObj[ObjCount].gameObject.SetActive(true);
                FrameCount = 0;
                ObjCount++;
            }
        }

    }
}
