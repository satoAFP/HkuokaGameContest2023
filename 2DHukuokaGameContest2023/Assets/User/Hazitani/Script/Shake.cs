using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField, Header("�R���{���Z�b�g���o�̎���(�t���[����)"), Range(0, 100)]
    protected int comboResetTime;

    [SerializeField, Header("������"), Range(0, 50)]
    protected float moveWidth;

    [SerializeField, Header("�ҋ@�t���[��"), Range(0, 10)]
    protected int stopFrame;

    [System.NonSerialized]
    protected Vector3 firstPos;       //�����ʒu�L���p
    [System.NonSerialized]
    protected Vector3 movePos;        //�ړ��ʓ��͗p
    [System.NonSerialized]
    protected int frameCount = 0;     //�t���[���J�E���g�p
    [System.NonSerialized]
    protected int combo_reset_time = 0;   //�R���{���Z�b�g�̎���
    [System.NonSerialized]
    protected bool reset_once = false;    //�R���{���Z�b�g��1��̂ݎ��s

    //�����ʒu�ݒ�
    protected void SetStartPos(GameObject obj)
    {
        //�h��̏����ʒu�ݒ�
        firstPos = obj.transform.localPosition;
        movePos = obj.transform.localPosition;
    }

    //�h��
    protected void ComboResetShaking(GameObject obj)
    {
        frameCount++;
        if (frameCount == stopFrame)
        {
            if (firstPos.x <= obj.transform.localPosition.x)
                movePos.x = firstPos.x - moveWidth;
            else
                movePos.x = firstPos.x + moveWidth;

            obj.transform.localPosition = movePos;
            frameCount = 0;
        }
    }
}