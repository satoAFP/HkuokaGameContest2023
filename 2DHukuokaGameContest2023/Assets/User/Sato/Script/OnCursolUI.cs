using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnCursolUI : MonoBehaviour
{
    [SerializeField, Header("変化させる色")] private Color color;

    // Update is called once per frame
    void Update()
    {
        //ボタン上にカーソルがあるとき枠をつける
        if(EventSystem.current.IsPointerOverGameObject())
            gameObject.GetComponent<Outline>().effectColor = color;
        else
            gameObject.GetComponent<Outline>().effectColor = new Color(1, 1, 1, 0);

    }
}
