using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    [SerializeField, Header("何秒でゲーム終わるか")]
    private int EndTime;

    [SerializeField, Header("タイム表示用テキスト")]
    private Text TimeText;

    [SerializeField, Header("ランキングパネル")]
    private GameObject ResultPanel;

    private int FrameCount = 0;     //フレームカウント用
    private bool first = true;      //最初の一回だけ入れる処理
    private ResultManager result;  //ランキングシステム

    private bool first2 = true;


    private void Start()
    {
        result = ResultPanel.GetComponent<ResultManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FrameCount++;


        //時間の設定
        if (first)
        {
            ManagerAccessor.Instance.systemManager.Time = EndTime;
            first = false;
        }

        //ゲームスタートしたらタイムカウント
        if (ManagerAccessor.Instance.systemManager.Time > 0)
        {
            if (FrameCount % 50 == 0)
                ManagerAccessor.Instance.systemManager.Time--;
            TimeText.text = "残り:" + ManagerAccessor.Instance.systemManager.Time + "秒";
        }
        else
        {
            if (first2)
            {
                ResultPanel.SetActive(true);

                //ranking.Init();
                //ranking.WriteScore();
                //ranking.Score[10] = ManagerAccessor.Instance.systemManager.Score;
                //ranking.Sort();
                //ranking.MemScore();

                ManagerAccessor.Instance.systemManager.GameEnd = true;
                first2 = false;
            }
        }
    }

    public void Reload()
    {
        ManagerAccessor.Instance.sceneManager.SceneReload();
    }
}
