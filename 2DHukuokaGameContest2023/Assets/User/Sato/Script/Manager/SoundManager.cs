using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.soundManager = this;
    }

}
