using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeleteTime : MonoBehaviour
{
    [SerializeField, Header("è¡Ç¶ÇÈÇ‹Ç≈ÇÃéûä‘")]
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
