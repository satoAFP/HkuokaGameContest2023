using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    [System.NonSerialized]
    public int Combo = 0;           //コンボ数
    [System.NonSerialized]
    public int MaxCombo = 0;        //最大コンボ数
    [System.NonSerialized]
    public bool FeverTime = false;  //フィーバータイム
    [System.NonSerialized]
    public int Score = 0;           //スコア
    [System.NonSerialized]
    public int Time = 0;            //ゲーム内の時間
    [System.NonSerialized]
    public bool GameEnd = false;    //ゲーム終了

    [SerializeField, Header("コンボテキスト")]
    public Text textCombo;

    [SerializeField, Header("最大コンボテキスト")]
    public Text textMaxCombo;

    [SerializeField, Header("スコアテキスト")]
    public Text textScore;


    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.systemManager = this;

        //テキスト初期化
        textCombo.text = Combo.ToString();
        textMaxCombo.text = MaxCombo.ToString();
        textScore.text = Score.ToString();
    }
}
