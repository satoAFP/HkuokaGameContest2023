using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFade : MonoBehaviour
{
    [SerializeField, Header("�w�i�̉摜")]
    private SpriteRenderer[] image;

    [SerializeField, Header("�w�i�̃O���f�[�V����")]
    private Gradient Gradation = default;

    [SerializeField, Header("�F��ς��鎞�Ԃƌ��݂̎���")]
    private float FADE_COLOR_TIME;


    private float _currentTime = 0;
    private float MoveAlpha = 0.0f;
    private int NowImage = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < image.Length; i++)
            image[i].color = new Color(1, 1, 1, 0f);
        MoveAlpha = 1.0f / (50 * FADE_COLOR_TIME);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ImageGradation();

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


    private void ColorGradation()
    {
        //���Ԃ�i�߂�
        _currentTime += Time.deltaTime;
        var timeRate = Mathf.Min(1f, _currentTime / FADE_COLOR_TIME);

        //�F��ύX
        gameObject.GetComponent<SpriteRenderer>().color = Gradation.Evaluate(timeRate);
    }
}
