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

    // Start is called before the first frame update
    void Start()
    {
        image[1].color = new Color(1, 1, 1, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        image[0].color -= new Color(0, 0, 0, 0.001f);
        image[1].color += new Color(0, 0, 0, 0.001f);
    }


    private void GRADATION()
    {
        //���Ԃ�i�߂�
        _currentTime += Time.deltaTime;
        var timeRate = Mathf.Min(1f, _currentTime / FADE_COLOR_TIME);

        //�F��ύX
        gameObject.GetComponent<SpriteRenderer>().color = Gradation.Evaluate(timeRate);
    }
}
