using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverGage : MonoBehaviour
{
    [SerializeField, Header("フィルオブジェクト")]
    private GameObject FillObj;

    [SerializeField, Header("フィーバータイム中のゲージスプライト")]
    private Sprite FeverSprite;

    [SerializeField, Header("フィーバータイム中のゲージ色")]
    private Color32 FeverColor;

    [SerializeField, Header("通常時のゲージスプライト")]
    private Sprite CommonSprite;

    [SerializeField, Header("通常時のゲージ色")]
    private Color32 CommonColor;


    private bool once = false;
    private float countdown = 10.0f;

    private void Start()
    {
        this.GetComponent<Slider>().value = 0;
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