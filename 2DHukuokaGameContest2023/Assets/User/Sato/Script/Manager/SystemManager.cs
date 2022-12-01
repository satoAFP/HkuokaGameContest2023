using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    [System.NonSerialized]
    public int Combo = 0;           //�R���{��
    [System.NonSerialized]
    public int MaxCombo = 0;        //�ő�R���{��
    [System.NonSerialized]
    public bool FeverTime = false;  //�t�B�[�o�[�^�C��
    [System.NonSerialized]
    public int Score = 0;           //�X�R�A
    [System.NonSerialized]
    public int Time = 0;            //�Q�[�����̎���
    [System.NonSerialized]
    public bool GameEnd = false;    //�Q�[���I��

    [SerializeField, Header("�R���{�e�L�X�g")]
    public Text textCombo;

    [SerializeField, Header("�ő�R���{�e�L�X�g")]
    public Text textMaxCombo;

    [SerializeField, Header("�X�R�A�e�L�X�g")]
    public Text textScore;


    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.systemManager = this;

        //�e�L�X�g������
        textCombo.text = Combo.ToString();
        textMaxCombo.text = MaxCombo.ToString();
        textScore.text = Score.ToString();
    }
}
