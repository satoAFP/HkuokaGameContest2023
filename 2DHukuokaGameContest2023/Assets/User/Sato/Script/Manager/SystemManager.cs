using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    [System.NonSerialized]
    public int Combo = 0;           //�R���{��
    [System.NonSerialized]
    public int MaxCombo = 0;        //�ő�R���{��
    [System.NonSerialized]
    public bool FeverTime = false;  //�t�B�[�o�[�^�C��
    [System.NonSerialized]
    public int Score = 0;           //�X�R�A
    [System.NonSerialized]
    public int Time = 0;            //�Q�[�����̎���
    [System.NonSerialized]
    public bool GameEnd = false;    //�Q�[���I��

    [SerializeField, Header("�R���{�e�L�X�g")]
    public Text textCombo;

    [SerializeField, Header("�ő�R���{�e�L�X�g")]
    public Text textMaxCombo;

    [SerializeField, Header("�X�R�A�e�L�X�g")]
    public Text textScore;

    [SerializeField, Header("�t�B�[�o�[�C���[�W")]
    private Image imageFever;

    [SerializeField, Header("�t�B�[�o�[�ʉߑ��x"), Range(0, 200)]
    private float feverSpeed;

    [SerializeField, Header("�t�B�[�o�[�ҋ@����(�b)"), Range(0, 100)]
    private int feverStopTime;

    private bool fever_in = false;  //�t�B�[�o�[�̃C���@�i�E�[���璆���܂Łj
    private bool fever_out = false; //�t�B�[�o�[�̃A�E�g�i�������獶�[�܂Łj
    private int fever_stop_time = 0;//�t�B�[�o�[�����ҋ@���Ԍv���p


    private void Start()
    {
        ManagerAccessor.Instance.systemManager = this;

        //�e�L�X�g������
        textCombo.text = Combo.ToString();
        textMaxCombo.text = MaxCombo.ToString();
        textScore.text = Score.ToString();

        //�t�B�[�o�[�C���[�W���E�[�ɐݒ�
        imageFever.transform.localPosition = new Vector3(1200.0f, 350.0f, 0.0f);
    }

    private void FixedUpdate()
    {
        //�t�B�[�o�[�^�C���ɓ�����
        if (FeverTime)
        {
            //�t�B�[�o�[���܂���ʂɉf���ĂȂ�
            if (!fever_in)
            {
                //�Ƃ肠�����t�B�[�o�[�C���[�W���E�[�Ɉړ�
                imageFever.transform.localPosition = new Vector3(1200.0f, 350.0f, 0.0f);
                fever_in = true;
            }
            else
            {
                //�t�B�[�o�[�C���[�W�������ɓ��B���Ă��Ȃ�
                if (!fever_out)
                {
                    //�����Ɉړ�
                    imageFever.transform.localPosition = Vector3.MoveTowards(imageFever.transform.localPosition, new Vector3(0.0f, 350.0f, 0.0f), feverSpeed);

                    //�����ɓ��B
                    if (imageFever.transform.localPosition == new Vector3(0.0f, 350.0f, 0.0f))
                    {
                        //�~�߂鎞�Ԃ��v��
                        fever_stop_time++;

                        //���ԂɂȂ�����t���O�ύX
                        if (fever_stop_time >= feverStopTime * 50)
                        {
                            fever_out = true;
                            fever_stop_time = 0;
                        }
                    }
                }
                else
                {
                    //�t�B�[�o�[�C���[�W�����[�Ɉړ�
                    imageFever.transform.localPosition = Vector3.MoveTowards(imageFever.transform.localPosition, new Vector3(-1200.0f, 350.0f, 0.0f), feverSpeed);
                }
            }
        }
        else
        {
            //�t���O���Z�b�g
            fever_in = false;
            fever_out = false;
        }
    }
}
