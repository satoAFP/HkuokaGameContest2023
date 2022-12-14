using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveText : MonoBehaviour
{
    [SerializeField, Header("à⁄ìÆó ")]
    private float MoveSpeed;

    [SerializeField, Header("è¡Ç¶ÇÈë¨ìx")]
    private int DeleteSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {

        gameObject.GetComponent<RectTransform>().position += new Vector3(0, MoveSpeed, 0);
        gameObject.GetComponent<Text>().color -= new Color32(0, 0, 0, (byte)DeleteSpeed);

        if (gameObject.GetComponent<Text>().color.a < 0)
            Destroy(gameObject);
    }
}
