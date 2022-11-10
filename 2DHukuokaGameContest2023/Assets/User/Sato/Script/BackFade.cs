using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFade : MonoBehaviour
{
    [SerializeField, Header("背景のグラデーション")]
    private Gradient Gradation = default;

    [SerializeField, Header("色を変える時間と現在の時間")]
    private float FADE_COLOR_TIME;


    private float _currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //時間を進める
        _currentTime += Time.deltaTime;
        var timeRate = Mathf.Min(1f, _currentTime / FADE_COLOR_TIME);

        //色を変更
        gameObject.GetComponent<SpriteRenderer>().color = Gradation.Evaluate(timeRate);
    }
}
