using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckCollision : MonoBehaviour
{
    [SerializeField,Header("�����蔻�������܂ł�frame")] private int DeleteFrame;

    [System.NonSerialized] public int Damage = 0;

    private int count = 0;      //�t���[���J�E���g�p

    void FixedUpdate()
    {
        count++;

        if (DeleteFrame <= count)
        {
            Destroy(gameObject);
        }
    }
}