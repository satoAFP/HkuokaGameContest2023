using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObj : MonoBehaviour
{
    [SerializeField, Header("消えるまでの時間")] private int DeleteFrame;

    private int count = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;

        if (DeleteFrame <= count)
            Destroy(gameObject);
    }
}
