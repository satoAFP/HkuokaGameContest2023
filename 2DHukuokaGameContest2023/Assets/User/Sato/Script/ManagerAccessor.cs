using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAccessor : MonoBehaviour
{
    //�V���O���g���p�^�[��
    private static ManagerAccessor instance = null;
    public static ManagerAccessor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ManagerAccessor();
            }
            return instance;
        }
    }

    //�}�l�[�W���[�̎Q��
    public Player_Ver2 player;
}
