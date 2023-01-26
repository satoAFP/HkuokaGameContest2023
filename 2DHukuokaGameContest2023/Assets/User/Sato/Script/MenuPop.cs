using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPop : MonoBehaviour
{
    [SerializeField, Header("メニュー")] private GameObject Menu;

    [SerializeField, Header("メニューボタンのテキスト")] private Text text;

    [System.NonSerialized] public bool menu_pop_now = false;

    private void Start()
    {
        //マネージャーに登録
        ManagerAccessor.Instance.menuPop = this;
    }

    public void PopMenu()
    {
        if (Menu.activeSelf)
        {
            Menu.SetActive(false);
            menu_pop_now = false;
            text.text = "メニュー";
        }
        else
        {
            Menu.SetActive(true);
            menu_pop_now = true;
            text.text = "モドル";
        }
    }

}
