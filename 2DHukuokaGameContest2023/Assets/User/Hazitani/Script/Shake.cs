using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField, Header("コンボリセット演出の時間(フレーム数)"), Range(0, 100)]
    protected int comboResetTime;

    [SerializeField, Header("動く幅"), Range(0, 50)]
    protected float moveWidth;

    [SerializeField, Header("待機フレーム"), Range(0, 10)]
    protected int stopFrame;

    [System.NonSerialized]
    protected Vector3 firstPos;       //初期位置記憶用
    [System.NonSerialized]
    protected Vector3 movePos;        //移動量入力用
    [System.NonSerialized]
    protected int frameCount = 0;     //フレームカウント用
    [System.NonSerialized]
    protected int combo_reset_time = 0;   //コンボリセットの時間
    [System.NonSerialized]
    protected bool reset_once = false;    //コンボリセットで1回のみ実行

    //初期位置設定
    protected void SetStartPos(GameObject obj)
    {
        //揺れの初期位置設定
        firstPos = obj.transform.localPosition;
        movePos = obj.transform.localPosition;
    }

    //揺れ
    protected void ComboResetShaking(GameObject obj)
    {
        frameCount++;
        if (frameCount == stopFrame)
        {
            if (firstPos.x <= obj.transform.localPosition.x)
                movePos.x = firstPos.x - moveWidth;
            else
                movePos.x = firstPos.x + moveWidth;

            obj.transform.localPosition = movePos;
            frameCount = 0;
        }
    }
}