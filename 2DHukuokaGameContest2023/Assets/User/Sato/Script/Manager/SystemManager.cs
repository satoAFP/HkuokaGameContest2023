using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : Shake
{
    [System.NonSerialized]
    public int Combo = 0;           //コンボ数
    [System.NonSerialized]
    public int MaxCombo = 0;        //最大コンボ数
    [System.NonSerialized]
    public int AllCombo = 0;        //総コンボ数
    [System.NonSerialized]
    public bool FeverTime = false;  //フィーバータイム
    [System.NonSerialized]
    public int Score = 0;           //スコア
    [System.NonSerialized]
    public int Time = 0;            //ゲーム内の時間
    [System.NonSerialized]
    public bool GameEnd = false;    //ゲーム終了
    [System.NonSerialized]
    public bool GameStart = false;  //ゲーム開始
    [System.NonSerialized]
    public bool MoveCamera = false; //ボス死亡時のカメラ移動
    [System.NonSerialized]
    public bool WeakCamera = false; //弱点攻撃時のカメラ移動
    [System.NonSerialized]
    public bool BossDethEnd = false;//弱点攻撃時のカメラ移動


    [SerializeField, Header("コンボテキスト")]
    public Text textCombo;

    [SerializeField, Header("最大コンボテキスト")]
    public Text textMaxCombo;

    [SerializeField, Header("スコアテキスト")]
    public Text textScore;

    [SerializeField, Header("フィーバーイメージ")]
    private Image imageFever;

    [SerializeField, Header("フィーバー出現位置")]
    private Vector3 feverPos;

    [SerializeField, Header("フィーバー通過速度"), Range(0, 500)]
    private float feverSpeed;

    [SerializeField, Header("フィーバー待機時間(秒)"), Range(0, 100)]
    private int feverStopTime;

    [SerializeField, Header("ボスのHP"), Range(1, 20)]
    public int BossHP;

    [SerializeField, Header("ボス撃破時のスコア加算量")]
    public int BossScore;

    [SerializeField, Header("ボスが新で消えるまでの時間")]
    public int BossDethTime;

    private bool fever_in = false;      //フィーバーのイン　（右端から中央まで）
    private bool fever_out = false;     //フィーバーのアウト（中央から左端まで）
    private int fever_stop_time = 0;    //フィーバー中央待機時間計測用


    private void Start()
    {
        ManagerAccessor.Instance.systemManager = this;

        //テキスト初期化
        textCombo.text = Combo.ToString();
        textMaxCombo.text = MaxCombo.ToString();
        textScore.text = Score.ToString();

        //フィーバーイメージを端に設定
        imageFever.transform.localPosition = feverPos;

        //初期位置設定
        SetStartPos(textCombo.gameObject);
    }

    private void FixedUpdate()
    {
        //フィーバータイムに入った
        if (FeverTime)
        {
            //フィーバーがまだ画面に映ってない
            if (!fever_in)
            {
                //とりあえずフィーバーイメージを端に移動
                imageFever.transform.localPosition = feverPos;
                fever_in = true;
            }
            else
            {
                //フィーバーイメージが中央に到達していない
                if (!fever_out)
                {
                    //中央に移動
                    imageFever.transform.localPosition = Vector3.MoveTowards(imageFever.transform.localPosition, new Vector3(0.0f, feverPos.y, 0.0f), feverSpeed);

                    //中央に到達
                    if (imageFever.transform.localPosition == new Vector3(0.0f, feverPos.y, 0.0f))
                    {
                        //止める時間を計測
                        fever_stop_time++;

                        //時間になったらフラグ変更
                        if (fever_stop_time >= feverStopTime * 50)
                        {
                            fever_out = true;
                            fever_stop_time = 0;
                        }
                    }
                }
                else
                {
                    //フィーバーイメージを端に移動
                    imageFever.transform.localPosition = Vector3.MoveTowards(imageFever.transform.localPosition, new Vector3(-feverPos.x, feverPos.y, 0.0f), feverSpeed);
                }
            }
        }
        else
        {
            //フラグリセット
            fever_in = false;
            fever_out = false;
        }

        //コンボがリセットされたとき
        if(ManagerAccessor.Instance.player.combo_reset)
        {
            if (!reset_once)
            {
                reset_once = true;
                textCombo.color = new Color32(255, 0, 0, 255);
            }

            combo_reset_time++;
            ComboResetShaking(textCombo.gameObject);

            if (combo_reset_time >= comboResetTime)
            {
                combo_reset_time = 0;
                reset_once = false;
                textCombo.color = new Color32(255, 135, 0, 255);
                textCombo.transform.localPosition = firstPos;
                ManagerAccessor.Instance.player.combo_reset = false;
            }
        }
    }
}
