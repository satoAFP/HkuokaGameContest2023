using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    [SerializeField, Header("何秒でゲーム終わるか")]
    private int EndTime;

    [SerializeField, Header("スタートまでのカウントダウン")]
    private int CountDown;

    [SerializeField, Header("タイム表示用テキスト")]
    private Text TimeText;

    [SerializeField, Header("カウントダウン表示用テキスト")]
    private Text CountDownText;

    [SerializeField, Header("ゲーム終了表示用テキスト")]
    private GameObject TimeUpText;

    [SerializeField, Header("ランキングパネル")]
    private GameObject ResultPanel;

    [SerializeField, Header("ゲーム開始時のSE")]
    private AudioClip StartSE;

    [SerializeField, Header("ボス撃破時時のSE")]
    private AudioClip ClearSE;

    [SerializeField, Header("タイムアップ時のSE")]
    private AudioClip TimeUpSE;

    private int FrameCount = 0;         //フレームカウント用
    private int CDFrameCount = 0;       //カウントダウンカウント用
    private int BossDethCount = 0;      //ボスが死んでからリザルトまで行く時間をカウントする変数


    //最初の一回だけ入れる処理
    private bool first = true;          
    private bool first2 = true;
    private bool first3 = true;

    private void Start()
    {
        //50フレーム一秒なので
        CDFrameCount = CountDown * 50;
        //テキスト表示
        CountDownText.text = CountDown.ToString();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //時間の設定
        if (first)
        {
            ManagerAccessor.Instance.systemManager.Time = EndTime;
            first = false;
        }

        //メニューを開いていないときフレームカウント
        if (!ManagerAccessor.Instance.menuPop.menu_pop_now)
            FrameCount++;


        //スタートまでのカウントダウン
        CDFrameCount--;
        if (CDFrameCount % 50 == 0) 
        {
            //カウント
            CountDown--;
            CountDownText.text = CountDown.ToString();

            //ゲームスタート
            if (CountDown == 0) 
            {
                ManagerAccessor.Instance.systemManager.GameStart = true;
                ManagerAccessor.Instance.player.rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                CountDownText.gameObject.SetActive(false);

                gameObject.GetComponent<AudioSource>().PlayOneShot(StartSE);
            }
        }

        //ゲームが開始されたら
        if (ManagerAccessor.Instance.systemManager.GameStart)
        {
            //ゲームスタートしたらタイムカウント
            if (ManagerAccessor.Instance.systemManager.Time > 0 && ManagerAccessor.Instance.systemManager.BossHP > 0)
            {
                if (FrameCount % 50 == 0)
                    ManagerAccessor.Instance.systemManager.Time--;
                TimeText.text = ManagerAccessor.Instance.systemManager.Time.ToString();
            }
            else
            {
                //フレームカウント
                BossDethCount++;

                //共有情報のゲーム終了
                ManagerAccessor.Instance.systemManager.GameEnd = true;

                //ボス撃破なら画面揺らす
                if (ManagerAccessor.Instance.systemManager.BossHP <= 0)
                {
                    ManagerAccessor.Instance.systemManager.MoveCamera = true;

                    //SEを鳴らす
                    if(first3)
                        gameObject.GetComponent<AudioSource>().PlayOneShot(ClearSE);
                    first3 = false;
                }
                else//タイムアップならテキスト表示
                {
                    TimeUpText.SetActive(true);

                    //SEを鳴らす
                    if (first3)
                        gameObject.GetComponent<AudioSource>().PlayOneShot(TimeUpSE);
                    first3 = false;
                }

                if (ManagerAccessor.Instance.systemManager.BossDethTime * 50 <= BossDethCount)
                {
                    //画面揺れ止める
                    ManagerAccessor.Instance.systemManager.MoveCamera = false;
                    TimeUpText.SetActive(false);

                    if (first2)
                    {
                        //ボス撃破時スコア加算
                        if (ManagerAccessor.Instance.systemManager.BossHP <= 0)
                        {
                            ManagerAccessor.Instance.systemManager.BossDethEnd = true;
                            ManagerAccessor.Instance.systemManager.Score += ManagerAccessor.Instance.systemManager.BossScore;
                            ManagerAccessor.Instance.systemManager.textScore.text = ManagerAccessor.Instance.systemManager.Score.ToString();
                        }

                        //リザルト画面表示
                        ResultPanel.SetActive(true);


                        //ランキング情報更新
                        RankingSystem ranking = ManagerAccessor.Instance.rankingSystem;
                        ranking.Init();
                        ranking.WriteScore();
                        ranking.Score[10] = ManagerAccessor.Instance.systemManager.Score;
                        ranking.Combo[10] = ManagerAccessor.Instance.systemManager.MaxCombo;
                        ranking.Time[10] = ManagerAccessor.Instance.systemManager.Time;
                        ranking.Sort();
                        ranking.MemScore();

                        
                        first2 = false;
                    }
                }
            }
        }
    }

    public void Reload()
    {
        ManagerAccessor.Instance.sceneManager.SceneReload();
    }
}
