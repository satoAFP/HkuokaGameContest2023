using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreDisplay : MonoBehaviour
{
    [SerializeField, Header("�\�����������e�L�X�g")]
    private GameObject ScoreText;

    private int MemScore = 0;
    private bool first = true;  //�ŏ��������鏈��

    // Update is called once per frame
    void FixedUpdate()
    {
        //�{�X�|�����Ƃ�
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

        //�X�R�A����������
        if (MemScore < ManagerAccessor.Instance.systemManager.Score) 
        {
            //�{�X�|������͓���Ȃ�
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
