using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPop : MonoBehaviour
{
    [SerializeField, Header("���j���[")] private GameObject Menu;

    [SerializeField, Header("���j���[�{�^���̃e�L�X�g")] private Text text;

    [System.NonSerialized] public bool menu_pop_now = false;

    private void Start()
    {
        //�}�l�[�W���[�ɓo�^
        ManagerAccessor.Instance.menuPop = this;
    }

    public void PopMenu()
    {
        if (Menu.activeSelf)
        {
            Menu.SetActive(false);
            menu_pop_now = false;
            text.text = "���j���[";
        }
        else
        {
            Menu.SetActive(true);
            menu_pop_now = true;
            text.text = "���h��";
        }
    }

}
