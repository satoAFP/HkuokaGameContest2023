using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPop : MonoBehaviour
{
    [SerializeField, Header("���j���[")] private GameObject Menu;

    [SerializeField, Header("���j���[�{�^���̃e�L�X�g")] private Text text;


    public void PopMenu()
    {
        if (Menu.activeSelf)
        {
            Menu.SetActive(false);
            text.text = "���j���[";
        }
        else
        {
            Menu.SetActive(true);
            text.text = "���h��";
        }
    }

}
