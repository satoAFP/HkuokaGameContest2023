using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField, Header("表示までの待ち時間")]
    private int WaitFrame;

    [SerializeField, Header("順番に表示するオブジェクト")]
    private GameObject[] DisplayObj;

    [SerializeField, Header("スコア用テキスト")]
    private Text ScoreText;

    [SerializeField, Header("マックスコンボ用テキスト")]
    private Text MaxComboText;


    private int FrameCount = 0;     //フレームカウント用
    private int ObjCount = 0;       //オブジェクトカウント用


    // Update is called once per frame
    void FixedUpdate()
    {
        FrameCount++;

        //スコア、コンボ表示用
        ScoreText.text = ManagerAccessor.Instance.systemManager.Score.ToString();
        MaxComboText.text = ManagerAccessor.Instance.systemManager.MaxCombo.ToString();

        //リザルト順番に出す用の処理
        if (WaitFrame <= FrameCount) 
        {
            if (DisplayObj.Length > ObjCount)
            {
                DisplayObj[ObjCount].gameObject.SetActive(true);
                FrameCount = 0;
                ObjCount++;
            }
        }

    }
}
