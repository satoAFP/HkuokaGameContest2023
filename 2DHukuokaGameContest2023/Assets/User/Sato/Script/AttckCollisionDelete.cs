using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckCollisionDelete : MonoBehaviour
{
    [SerializeField,Header("�����蔻�������܂ł�frame")] private int DeleteFrame;


    private int count = 0;      //�t���[���J�E���g�p

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;

        if (DeleteFrame == count)
            Destroy(gameObject);
    }
}
