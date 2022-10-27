using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckCollision : MonoBehaviour
{
    [SerializeField,Header("当たり判定消えるまでのframe")] private int DeleteFrame;

    [System.NonSerialized] public int Damage = 0;

    private int count = 0;      //フレームカウント用

    void FixedUpdate()
    {
        count++;

        if (DeleteFrame == count)
            Destroy(gameObject);
    }
}