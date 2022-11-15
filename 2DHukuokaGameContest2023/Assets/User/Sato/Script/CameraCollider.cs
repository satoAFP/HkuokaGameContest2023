using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    private GameObject player;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.localPosition;

        if (player.transform.localPosition.y > 0) 
        {
            pos.y = player.transform.localPosition.y;
        }

        transform.localPosition = pos;
    }
}
