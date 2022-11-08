using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeleteTime : MonoBehaviour
{
    [SerializeField, Header("������܂ł̎���")]
    private int DeleteTime;

    private int frameCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (frameCount == DeleteTime)
        {
            Destroy(gameObject);
        }

        frameCount++;
    }
}
