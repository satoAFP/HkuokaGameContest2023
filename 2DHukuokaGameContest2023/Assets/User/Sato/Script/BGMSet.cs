using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSet : MonoBehaviour
{
    [SerializeField, Header("通常のときのBGM")]
    private AudioClip DefoltSound;

    [SerializeField, Header("フィーバータイムのときのBGM")]
    private AudioClip FeverSound;

    private AudioSource audio;      //オーディオ
    private bool first = true;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //フィーバータイムのときBGMが変わる
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
