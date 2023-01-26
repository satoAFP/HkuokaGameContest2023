using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPop : MonoBehaviour
{
    [SerializeField, Header("メニュー")] private GameObject Menu;

    [SerializeField, Header("メニューボタンのテキスト")] private Text text;


    public void PopMenu()
    {
        if (Menu.activeSelf)
        {
            Menu.SetActive(false);
            text.text = "メニュー";
        }
        else
        {
            Menu.SetActive(true);
            text.text = "モドル";
        }
    }

}
