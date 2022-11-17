using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFade : MonoBehaviour
{
    [SerializeField, Header("背景の画像")]
    private SpriteRenderer[] image;

    [SerializeField, Header("背景のグラデーション")]
    private Gradient Gradation = default;

    [SerializeField, Header("色を変える時間と現在の時間")]
    private float FADE_COLOR_TIME;

    [SerializeField, Header("fadeのスタートライン")]
    private GameObject StartLine;

    [SerializeField, Header("fadeの終了ライン")]
    private GameObject FinishLine;


    private float _currentTime = 0;
    public float MoveAlpha = 0.0f;
    private int NowImage = 0;
    private float FadeDistance = 0;
    private Player_Ver2 player;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < image.Length; i++)
            image[i].color = new Color(1, 1, 1, 0f);
        //MoveAlpha = 1.0f / (50 * FADE_COLOR_TIME);
        FadeDistance = FinishLine.transform.position.y - StartLine.transform.position.y;

        player = GameObject.Find("Player").GetComponent<Player_Ver2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ImageGradation();

        PlayerMoveGradation();

    }

    private void ImageGradation()
    {
        if ((image.Length - 1) > NowImage) 
        {
            image[NowImage].color -= new Color(0, 0, 0, MoveAlpha);
            image[NowImage + 1].color += new Color(0, 0, 0, MoveAlpha);

            if (image[NowImage].color.a <= 0.0f)
                NowImage++;
        }
    }

    private void PlayerMoveGradation()
    {
        MoveAlpha = ((image.Length - 1) / FadeDistance) * (player.transform.position.y - StartLine.transform.position.y);

        if (MoveAlpha > 0.0f && MoveAlpha <= 1.0f) 
        {
            image[NowImage].color = new Color(1, 1, 1, 1 - MoveAlpha);
            image[NowImage + 1].color = new Color(1, 1, 1, MoveAlpha);
        }
        else if (MoveAlpha > 1.0f && MoveAlpha <= 2.0f)
        {
            image[NowImage+1].color = new Color(1, 1, 1, 2 - MoveAlpha);
            image[NowImage + 2].color = new Color(1, 1, 1, MoveAlpha - 1);
        }
    }


    private void ColorGradation()
    {
        //時間を進める
        _currentTime += Time.deltaTime;
        var timeRate = Mathf.Min(1f, _currentTime / FADE_COLOR_TIME);

        //色を変更
        gameObject.GetComponent<SpriteRenderer>().color = Gradation.Evaluate(timeRate);
    }
}
