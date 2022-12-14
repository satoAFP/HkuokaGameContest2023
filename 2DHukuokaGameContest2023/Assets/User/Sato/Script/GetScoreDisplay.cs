using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreDisplay : MonoBehaviour
{
    [SerializeField, Header("表示させたいテキスト")]
    private GameObject ScoreText;

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
                GameObject clone = Instantiate(ScoreText, transform.position, Quaternion.identity);
                clone.transform.parent = gameObject.transform;
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
                GameObject clone = Instantiate(ScoreText, transform.position, Quaternion.identity);
                clone.transform.parent = gameObject.transform;
                clone.GetComponent<Text>().text = ManagerAccessor.Instance.player.score_add.ToString();

                MemScore = ManagerAccessor.Instance.systemManager.Score;
            }
        }

    }
}
