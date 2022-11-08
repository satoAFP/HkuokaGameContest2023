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

    [SerializeField, Header("コンボ表示用")]
    public GameObject textMeshObj;

    private int ComboNum = 0;
    private AnimatorStateInfo animStateInfo;

    // Start is called before the first frame update
    void Start()
    {
        textMeshObj.gameObject.GetComponent<MeshRenderer>().sortingOrder = 10;
    }

    // Update is called once per frame
    void Update()
    {






        if (Combo)
        {
            ComboNum++;
            sizeUpAni.SetTrigger("SizeUp");
            

            Combo = false;
        }
        text.text = "" + ComboNum;
        textMeshObj.GetComponent<TextMesh>().text = "" + ComboNum;
    }
}
