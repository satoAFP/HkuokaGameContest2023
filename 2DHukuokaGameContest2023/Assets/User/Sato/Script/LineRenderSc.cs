using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderSc : MonoBehaviour
{
    LineRenderer linerend;

    void Start()
    {
        linerend = gameObject.AddComponent<LineRenderer>();

        Vector3 pos1 = new Vector3(0, -3, 0);
        Vector3 pos2 = new Vector3(0, 3, 0);

        // 線を引く場所を指定する
        //linerend.SetPosition(0, pos1);
        //linerend.SetPosition(1, pos2);
    }
}
