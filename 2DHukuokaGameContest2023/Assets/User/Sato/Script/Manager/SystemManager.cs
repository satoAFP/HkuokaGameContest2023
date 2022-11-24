using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.systemManager = this;
    }
}
