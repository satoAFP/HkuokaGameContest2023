using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeleteTime : MonoBehaviour
{
    [SerializeField, Header("消えるまでの時間")]
    private int DeleteTime;

    private int frameCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (frameCount == DeleteTime)
        {
            Destroy(gameObject);
        }

        frameCount++;
    }
}
