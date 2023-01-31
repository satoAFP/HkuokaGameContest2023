using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField, Header("�G�t�F�N�g")] private GameObject Effect;

    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //�o������
        if(Input.GetMouseButton(0))
        {
            if (first)
                OnEffect();
            first = false;
        }
        else
        {
            first = true;
        }
    }

    public void OnEffect()
    {
        Vector2 mousePos = Input.mousePosition;
        // �X�N���[�����W��Z�l��5�ɕύX  
        Vector3 screenPos = new Vector3(mousePos.x, mousePos.y, 5f);
        // ���[���h���W�ɕϊ�  
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        // ���[���h���W��3D�I�u�W�F�N�g�̍��W�ɓK�p  
        transform.localPosition = worldPos;

        //�G�t�F�N�g����
        GameObject clone = Instantiate(Effect, Vector3.zero, Quaternion.identity);
        clone.transform.localPosition = worldPos;
    }
}
