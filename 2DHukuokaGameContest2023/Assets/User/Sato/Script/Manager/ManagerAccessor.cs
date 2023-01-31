using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAccessor
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
    public SystemManager systemManager;
    public SceneManagerScript sceneManager;
    public BossManager bossManager;
    public RankingSystem rankingSystem;
    public SoundManager soundManager;
    public MenuPop menuPop;
    public FeverGage feverGage;
}
