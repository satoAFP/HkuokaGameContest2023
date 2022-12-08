using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCousor : MonoBehaviour
{
    private Vector3 mousePos;   //�}�E�X�J�[�\���̈ʒu

    private void Start()
    {
        //�}�E�X�J�[�\���̏����ʒu�ݒ�
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        //�Q�[�����͕\��
        if(!ManagerAccessor.Instance.systemManager.GameEnd)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        //�}�E�X�X�J�[�\���̈ʒu�X�V
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        gameObject.transform.position = mousePos;
    }
}