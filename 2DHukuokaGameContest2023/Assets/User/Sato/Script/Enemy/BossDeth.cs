using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossDeth : MonoBehaviour
{
    [SerializeField, Header("カメラコライダー")]
    private GameObject CColl;

    public CinemachineConfiner camera;

    // Start is called before the first frame update
    void Start()
    {
        camera.m_BoundingShape2D = CColl.GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
