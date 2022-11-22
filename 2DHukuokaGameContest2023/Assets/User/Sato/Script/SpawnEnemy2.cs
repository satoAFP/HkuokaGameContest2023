using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    [SerializeField, Header("���X�|�[���܂ł̃t���[��")]
    private int RespownFrame;

    [SerializeField, Header("�o��������I�u�W�F�N�g")]
    private GameObject SpownEnemy;


    private int FrameCount = 0; //�t���[���J�E���g�p
    private GameObject clone;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (clone == null)
        {
            if (RespownFrame <= FrameCount)
            {
                clone = Instantiate(SpownEnemy, transform.position, Quaternion.identity);
                clone.transform.parent = transform;
                FrameCount = 0;
            }
            FrameCount++;
        }

    }
}
