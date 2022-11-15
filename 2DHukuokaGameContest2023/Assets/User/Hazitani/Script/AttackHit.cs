using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    private Player_Ver2 player = null;

    private void Start()
    {
        player = transform.root.GetComponent<Player_Ver2>();
    }

    private void Update()
    {
        if(player.enemyObj != null)
        {
            if(player.enemyObj.deth)
            {
                player.hit_enemy = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player.enemyObj = collision.GetComponent<BaseEnemyFly>();

            player.hit_enemy = true;
        }
    }
}