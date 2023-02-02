using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    [SerializeField, Header("���b�ŃQ�[���I��邩")]
    private int EndTime;

    [SerializeField, Header("�X�^�[�g�܂ł̃J�E���g�_�E��")]
    private int CountDown;

    [SerializeField, Header("�^�C���\���p�e�L�X�g")]
    private Text TimeText;

    [SerializeField, Header("�J�E���g�_�E���\���p�e�L�X�g")]
    private Text CountDownText;

    [SerializeField, Header("�Q�[���I���\���p�e�L�X�g")]
    private GameObject TimeUpText;

    [SerializeField, Header("�����L���O�p�l��")]
    private GameObject ResultPanel;

    [SerializeField, Header("�{�X���j������SE")]
    private AudioClip ClearSE;

    [SerializeField, Header("�^�C���A�b�v����SE")]
    private AudioClip TimeUpSE;

    private int FrameCount = 0;         //�t���[���J�E���g�p
    private int CDFrameCount = 0;       //�J�E���g�_�E���J�E���g�p
    private int BossDethCount = 0;      //�{�X������ł��烊�U���g�܂ōs�����Ԃ��J�E���g����ϐ�


    //�ŏ��̈�񂾂�����鏈��
    private bool first = true;          
    private bool first2 = true;
    private bool first3 = true;

    private void Start()
    {
        //50�t���[����b�Ȃ̂�
        CDFrameCount = CountDown * 50;
        //�e�L�X�g�\��
        CountDownText.text = CountDown.ToString();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //���Ԃ̐ݒ�
        if (first)
        {
            ManagerAccessor.Instance.systemManager.Time = EndTime;
            first = false;
        }

        //���j���[���J���Ă��Ȃ��Ƃ��t���[���J�E���g
        if (!ManagerAccessor.Instance.menuPop.menu_pop_now)
            FrameCount++;


        //�X�^�[�g�܂ł̃J�E���g�_�E��
        CDFrameCount--;
        if (CDFrameCount % 50 == 0) 
        {
            //�J�E���g
            CountDown--;
            CountDownText.text = CountDown.ToString();

            //�Q�[���X�^�[�g
            if (CountDown == 0) 
            {
                ManagerAccessor.Instance.systemManager.GameStart = true;
                ManagerAccessor.Instance.player.rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                CountDownText.gameObject.SetActive(false);
            }
        }

        //�Q�[�����J�n���ꂽ��
        if (ManagerAccessor.Instance.systemManager.GameStart)
        {
            //�Q�[���X�^�[�g������^�C���J�E���g
            if (ManagerAccessor.Instance.systemManager.Time > 0 && ManagerAccessor.Instance.systemManager.BossHP > 0)
            {
                if (FrameCount % 50 == 0)
                    ManagerAccessor.Instance.systemManager.Time--;
                TimeText.text = ManagerAccessor.Instance.systemManager.Time.ToString();
            }
            else
            {
                //�t���[���J�E���g
                BossDethCount++;

                //���L���̃Q�[���I��
                ManagerAccessor.Instance.systemManager.GameEnd = true;

                //�{�X���j�Ȃ��ʗh�炷
                if (ManagerAccessor.Instance.systemManager.BossHP <= 0)
                {
                    ManagerAccessor.Instance.systemManager.MoveCamera = true;

                    //SE��炷
                    if(first3)
                        gameObject.GetComponent<AudioSource>().PlayOneShot(ClearSE);
                    first3 = false;
                }
                else//�^�C���A�b�v�Ȃ�e�L�X�g�\��
                {
                    TimeUpText.SetActive(true);

                    //SE��炷
                    if (first3)
                        gameObject.GetComponent<AudioSource>().PlayOneShot(TimeUpSE);
                    first3 = false;
                }

                if (ManagerAccessor.Instance.systemManager.BossDethTime * 50 <= BossDethCount)
                {
                    //��ʗh��~�߂�
                    ManagerAccessor.Instance.systemManager.MoveCamera = false;
                    TimeUpText.SetActive(false);

                    if (first2)
                    {
                        //�{�X���j���X�R�A���Z
                        if (ManagerAccessor.Instance.systemManager.BossHP <= 0)
                        {
                            ManagerAccessor.Instance.systemManager.BossDethEnd = true;
                            ManagerAccessor.Instance.systemManager.Score += ManagerAccessor.Instance.systemManager.BossScore;
                            ManagerAccessor.Instance.systemManager.textScore.text = ManagerAccessor.Instance.systemManager.Score.ToString();
                        }

                        //���U���g��ʕ\��
                        ResultPanel.SetActive(true);


                        //�����L���O���X�V
                        RankingSystem ranking = ManagerAccessor.Instance.rankingSystem;
                        ranking.Init();
                        ranking.WriteScore();
                        ranking.Score[10] = ManagerAccessor.Instance.systemManager.Score;
                        ranking.Combo[10] = ManagerAccessor.Instance.systemManager.MaxCombo;
                        ranking.Time[10] = ManagerAccessor.Instance.systemManager.Time;
                        ranking.Sort();
                        ranking.MemScore();

                        
                        first2 = false;
                    }
                }
            }
        }
    }

    public void Reload()
    {
        ManagerAccessor.Instance.sceneManager.SceneReload();
    }
}
