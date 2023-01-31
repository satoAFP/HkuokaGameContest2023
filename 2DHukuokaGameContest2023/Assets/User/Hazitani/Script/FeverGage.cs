using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverGage : Shake
{
    [SerializeField, Header("�t�B���I�u�W�F�N�g")]
    private GameObject FillObj;

    [SerializeField, Header("�t�B�[�o�[�^�C�����̃Q�[�W�X�v���C�g")]
    private Sprite FeverSprite;

    [SerializeField, Header("�t�B�[�o�[�^�C�����̃Q�[�W�F")]
    private Color32 FeverColor;

    [SerializeField, Header("�ʏ펞�̃Q�[�W�X�v���C�g")]
    private Sprite CommonSprite;

    [SerializeField, Header("�ʏ펞�̃Q�[�W�F")]
    private Color32 CommonColor;

    [SerializeField, Header("�t�B�[�o�[�I�u�W�F�N�g")]
    public Slider sliderFever;


    private bool once = false;
    [System.NonSerialized]
    public float countdown = 10.0f;

    private void Start()
    {
        //�}�l�[�W���[�ɓo�^
        ManagerAccessor.Instance.feverGage = this;

        //countdown = ManagerAccessor.Instance.player.FeverTime;

        this.GetComponent<Slider>().value = 0;

        SetStartPos(sliderFever.gameObject);
    }

    private void FixedUpdate()
    {
        if(!ManagerAccessor.Instance.systemManager.FeverTime)
        {
            if (!once)
            {
                once = true;
                FillObj.GetComponent<Image>().color = CommonColor;
                FillObj.GetComponent<Image>().sprite = CommonSprite;
            }

            if(ManagerAccessor.Instance.player.fever_down)
            {
                combo_reset_time++;
                ComboResetShaking(sliderFever.gameObject);

                if (combo_reset_time >= comboResetTime)
                {
                    combo_reset_time = 0;
                    this.transform.localPosition = firstPos;
                    ManagerAccessor.Instance.player.fever_down = false;
                }
            }
            
            this.GetComponent<Slider>().value = ManagerAccessor.Instance.player.combo_fever_count;
        }
        else
        {
            if (once)
            {
                once = false;
                FillObj.GetComponent<Image>().color = FeverColor;
                FillObj.GetComponent<Image>().sprite = FeverSprite;
                countdown = ManagerAccessor.Instance.player.FeverTime;
            }

            countdown -= Time.deltaTime;
            this.GetComponent<Slider>().value = countdown;
        }
    }
}