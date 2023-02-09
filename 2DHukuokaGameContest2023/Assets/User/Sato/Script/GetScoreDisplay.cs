using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreDisplay : MonoBehaviour
{
    [SerializeField, Header("表示させたいテキスト")]
    private GameObject ScoreText;

    [SerializeField, Header("表示させたい座標")]
    private Vector3 TextPos;

    private int MemScore = 0;
    private bool first = true;  //最初だけ入る処理

    // Update is called once per frame
    void FixedUpdate()
    {
        //ボス倒したとき
        if (ManagerAccessor.Instance.systemManager.BossDethEnd)
        {
            if (first)
            {
                GameObject clone = Instantiate(ScoreText, transform.position, Quaternion.identity, transform.parent);
                clone.GetComponent<RectTransform>().localPosition = TextPos;
                clone.GetComponent<Text>().text = ManagerAccessor.Instance.systemManager.BossScore.ToString();
                first = false;
            }
        }

        //スコアが増えた時
        if (MemScore < ManagerAccessor.Instance.systemManager.Score) 
        {
            //ボス倒した後は入らない
            if (!ManagerAccessor.Instance.systemManager.BossDethEnd)
            {
                GameObject clone = Instantiate(ScoreText, transform.position, Quaternion.identity, transform.parent);
                clone.GetComponent<RectTransform>().localPosition = TextPos;
                clone.GetComponent<Text>().text = ManagerAccessor.Instance.player.score_add.ToString();

                MemScore = ManagerAccessor.Instance.systemManager.Score;
            }
        }

    }
}
