using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    private Player_Ver2 player = null;
    private BaseEnemyFly enemyObj = null;

    private void Start()
    {
        player = transform.root.GetComponent<Player_Ver2>();
    }

    private void Update()
    {
        if(enemyObj != null)
        {
            if(enemyObj.deth)
            {
                player.hit_enemy = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyObj = collision.GetComponent<BaseEnemyFly>();

            player.hit_enemy = true;
            player.hit_enemy_pos = collision.gameObject.transform.position;
            //Debug.Log("“G‚ÌˆÊ’u:" + player.hit_enemy_pos);
        }
    }
}