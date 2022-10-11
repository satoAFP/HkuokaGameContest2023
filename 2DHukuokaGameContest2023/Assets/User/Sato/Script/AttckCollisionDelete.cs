using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckCollisionDelete : MonoBehaviour
{
    [SerializeField,Header("当たり判定消えるまでのframe")] private int DeleteFrame;


    private int count = 0;      //フレームカウント用

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;

        if (DeleteFrame == count)
            Destroy(gameObject);
    }
}
