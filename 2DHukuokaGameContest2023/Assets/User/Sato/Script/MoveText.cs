using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveText : MonoBehaviour
{
    [SerializeField, Header("�ړ���")]
    private float MoveSpeed;

    [SerializeField, Header("�����鑬�x")]
    private int DeleteSpeed;

    [SerializeField, Header("������܂ł̎���")]
    private int DeleteFrame;


    private int FrameCount = 0;



    // Update is called once per frame
    void FixedUpdate()
    {
        FrameCount++;

        gameObject.GetComponent<RectTransform>().position += new Vector3(0, MoveSpeed, 0);
        gameObject.GetComponent<Text>().color -= new Color32(0, 0, 0, (byte)DeleteSpeed);

        if (gameObject.GetComponent<Text>().color.a < 0)
            Destroy(gameObject);
    }
}
