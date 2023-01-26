using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnCursolUI : MonoBehaviour
{
    [SerializeField, Header("�ω�������F")] private Color color;

    // Update is called once per frame
    void Update()
    {
        //�{�^����ɃJ�[�\��������Ƃ��g������
        if(EventSystem.current.IsPointerOverGameObject())
            gameObject.GetComponent<Outline>().effectColor = color;
        else
            gameObject.GetComponent<Outline>().effectColor = new Color(1, 1, 1, 0);

    }
}
