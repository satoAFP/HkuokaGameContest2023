using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [System.NonSerialized]
    public int Combo;
    [System.NonSerialized]
    public int MaxCombo;
    [System.NonSerialized]
    public bool FeverTime = false;
    [System.NonSerialized]
    public int Score;
    [System.NonSerialized]
    public int Time;


    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.systemManager = this;
    }
}
