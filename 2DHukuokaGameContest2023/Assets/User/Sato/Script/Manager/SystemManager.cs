using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    [System.NonSerialized]
    public int Combo = 0;
    [System.NonSerialized]
    public int MaxCombo = 0;
    [System.NonSerialized]
    public bool FeverTime = false;
    [System.NonSerialized]
    public int Score = 0;
    [System.NonSerialized]
    public int Time = 0;

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
