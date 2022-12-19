using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSet : MonoBehaviour
{
    [SerializeField, Header("�ʏ�̂Ƃ���BGM")]
    private AudioClip DefoltSound;

    [SerializeField, Header("�t�B�[�o�[�^�C���̂Ƃ���BGM")]
    private AudioClip FeverSound;

    private AudioSource audio;      //�I�[�f�B�I
    private bool first = true;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�t�B�[�o�[�^�C���̂Ƃ�BGM���ς��
        if (first == !ManagerAccessor.Instance.systemManager.FeverTime)
        {
            if (ManagerAccessor.Instance.systemManager.FeverTime)
            {
                audio.clip = FeverSound;
                audio.Play();
                first = ManagerAccessor.Instance.systemManager.FeverTime;
            }
            else
            {
                audio.clip = DefoltSound;
                audio.Play();
                first = ManagerAccessor.Instance.systemManager.FeverTime;
            }
        }


    }
}
