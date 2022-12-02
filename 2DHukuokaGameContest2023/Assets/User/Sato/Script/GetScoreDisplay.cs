using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreDisplay : MonoBehaviour
{
    [SerializeField, Header("�\�����������e�L�X�g")]
    private GameObject ScoreText;

    private int MemScore = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //�X�R�A����������
        if (MemScore < ManagerAccessor.Instance.systemManager.Score) 
        {
            GameObject clone = Instantiate(ScoreText, transform.position, Quaternion.identity);
            clone.transform.parent = gameObject.transform;
            clone.GetComponent<Text>().text = ManagerAccessor.Instance.player.score_add.ToString();

            MemScore = ManagerAccessor.Instance.systemManager.Score;
        }
    }
}
