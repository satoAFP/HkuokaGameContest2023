using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverGage : MonoBehaviour
{
    [SerializeField, Header("フィーバータイム中のゲージスプライト")]
    private Sprite FeverSprite;

    [SerializeField, Header("通常時のゲージスプライト")]
    private Sprite CommonSprite;

    private void Start()
    {
        this.GetComponent<Slider>().value = 0;
    }

    private void FixedUpdate()
    {
        if(!ManagerAccessor.Instance.systemManager.FeverTime)
        {
            this.GetComponent<Image>().color = new Color32(255, 135, 0, 255);
            this.GetComponent<Image>().sprite = CommonSprite;
            this.GetComponent<Slider>().value = ManagerAccessor.Instance.player.combo_fever_count;
        }
        else
        {
            this.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            this.GetComponent<Image>().sprite = FeverSprite;
            this.GetComponent<Slider>().value -= 0.1f;
        }
    }
}