using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCousor : MonoBehaviour
{
    private Vector3 mousePos;   //マウスカーソルの位置

    private void Start()
    {
        //マウスカーソルの初期位置設定
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        //ゲーム中は表示
        if(!ManagerAccessor.Instance.systemManager.GameEnd)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        //マウススカーソルの位置更新
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        gameObject.transform.position = mousePos;
    }
}