using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCombo : MonoBehaviour
{
    [SerializeField, Header("コンボプラス(仮)")]
    private bool Combo;

    [SerializeField, Header("コンボ表示用")]
    public Text text;

    [SerializeField, Header("コンボ表示用")]
    public Animator sizeUpAni;

    private int ComboNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Combo)
        {
            ComboNum++;
            if (!sizeUpAni.GetBool("SizeUp"))
                sizeUpAni.SetBool("SizeUp", true);

            

            Combo = false;
        }
        text.text = "" + ComboNum;
    }
}
