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
    public int AllCombo = 0;        //���R���{��
    [System.NonSerialized]
    public bool FeverTime = false;  //�t�B�[�o�[�^�C��
    [System.NonSerialized]
    public int Score = 0;           //�X�R�A
    [System.NonSerialized]
    public int Time = 0;            //�Q�[�����̎���
    [System.NonSerialized]
    public bool GameEnd = false;    //�Q�[���I��
    [System.NonSerialized]
    public bool GameStart = false;  //�Q�[���J�n
    [System.NonSerialized]
    public bool MoveCamera = false; //�{�X���S���̃J�����ړ�
    [System.NonSerialized]
    public bool WeakCamera = false; //��_�U�����̃J�����ړ�
    [System.NonSerialized]
    public bool BossDethEnd = false;//��_�U�����̃J�����ړ�


    [SerializeField, Header("�R���{�e�L�X�g")]
    public Text textCombo;

    [SerializeField, Header("�ő�R���{�e�L�X�g")]
    public Text textMaxCombo;

    [SerializeField, Header("�X�R�A�e�L�X�g")]
    public Text textScore;

    [SerializeField, Header("�t�B�[�o�[�C���[�W")]
    private Image imageFever;

    [SerializeField, Header("�t�B�[�o�[�o���ʒu")]
    private Vector3 feverPos;

    [SerializeField, Header("�t�B�[�o�[�ʉߑ��x"), Range(0, 500)]
    private float feverSpeed;

    [SerializeField, Header("�t�B�[�o�[�ҋ@����(�b)"), Range(0, 100)]
    private int feverStopTime;

    [SerializeField, Header("�R���{���Z�b�g���o�̎���(�t���[����)"), Range(0, 100)]
    private int comboResetTime;

    [SerializeField, Header("������"), Range(0, 50)]
    private float moveWidth;

    [SerializeField, Header("�ҋ@�t���[��"), Range(0, 10)]
    private int stopFrame;

    [SerializeField, Header("�{�X��HP"), Range(1, 20)]
    public int BossHP;

    [SerializeField, Header("�{�X���j���̃X�R�A���Z��")]
    public int BossScore;

    [SerializeField, Header("�{�X���V�ŏ�����܂ł̎���")]
    public int BossDethTime;

    private bool fever_in = false;      //�t�B�[�o�[�̃C���@�i�E�[���璆���܂Łj
    private bool fever_out = false;     //�t�B�[�o�[�̃A�E�g�i�������獶�[�܂Łj
    private int fever_stop_time = 0;    //�t�B�[�o�[�����ҋ@���Ԍv���p
    private int combo_reset_time = 0;   //�R���{���Z�b�g�̎���
    private bool reset_once = false;    //�R���{���Z�b�g��1��̂ݎ��s
    private Vector3 firstPos;       //�����ʒu�L���p
    private Vector3 movePos;        //�ړ��ʓ��͗p
    private int frameCount = 0;     //�t���[���J�E���g�p


    private void Start()
    {
        ManagerAccessor.Instance.systemManager = this;

        //�e�L�X�g������
        textCombo.text = Combo.ToString();
        textMaxCombo.text = MaxCombo.ToString();
        textScore.text = Score.ToString();

        //�t�B�[�o�[�C���[�W��[�ɐݒ�
        imageFever.transform.localPosition = feverPos;


        //�h��̏����ʒu�ݒ�
        firstPos = textCombo.transform.localPosition;
        movePos = textCombo.transform.localPosition;
    }

    private void FixedUpdate()
    {
        //�t�B�[�o�[�^�C���ɓ�����
        if (FeverTime)
        {
            //�t�B�[�o�[���܂���ʂɉf���ĂȂ�
            if (!fever_in)
            {
                //�Ƃ肠�����t�B�[�o�[�C���[�W��[�Ɉړ�
                imageFever.transform.localPosition = feverPos;
                fever_in = true;
            }
            else
            {
                //�t�B�[�o�[�C���[�W�������ɓ��B���Ă��Ȃ�
                if (!fever_out)
                {
                    //�����Ɉړ�
                    imageFever.transform.localPosition = Vector3.MoveTowards(imageFever.transform.localPosition, new Vector3(0.0f, feverPos.y, 0.0f), feverSpeed);

                    //�����ɓ��B
                    if (imageFever.transform.localPosition == new Vector3(0.0f, feverPos.y, 0.0f))
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
                    //�t�B�[�o�[�C���[�W��[�Ɉړ�
                    imageFever.transform.localPosition = Vector3.MoveTowards(imageFever.transform.localPosition, new Vector3(-feverPos.x, feverPos.y, 0.0f), feverSpeed);
                }
            }
        }
        else
        {
            //�t���O���Z�b�g
            fever_in = false;
            fever_out = false;
        }

        //�R���{�����Z�b�g���ꂽ�Ƃ�
        if(ManagerAccessor.Instance.player.combo_reset)
        {
            if (!reset_once)
            {
                reset_once = true;
                textCombo.color = new Color32(255, 0, 0, 255);
            }

            combo_reset_time++;
            ComboResetShaking();

            if (combo_reset_time >= comboResetTime)
            {
                combo_reset_time = 0;
                reset_once = false;
                textCombo.color = new Color32(255, 135, 0, 255);
                textCombo.transform.localPosition = firstPos;
                ManagerAccessor.Instance.player.combo_reset = false;
            }
        }
    }

    //�R���{���Z�b�g�h��(HitStop���p)
    private void ComboResetShaking()
    {
        frameCount++;
        if (frameCount == stopFrame)
        {
            if (firstPos.x <= textCombo.transform.localPosition.x)
                movePos.x = firstPos.x - moveWidth;
            else
                movePos.x = firstPos.x + moveWidth;

            textCombo.transform.localPosition = movePos;
            frameCount = 0;
        }
    }
}
