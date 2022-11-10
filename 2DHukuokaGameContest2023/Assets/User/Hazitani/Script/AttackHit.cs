using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    private Player_Ver2 player = null;
    private BaseEnemyFly baseEnemyFly = null;//攻撃が当たった敵のオブジェクトを入れる

    private void Start()
    {
        player = transform.root.GetComponent<Player_Ver2>();
    }

    private void Update()
    {
        if (baseEnemyFly != null)
        {
            if (baseEnemyFly.deth)
            {
                player.enemy_alive = false;
                player.hit_enemy = false;
                Debug.Log("敵死にました");
            }
            else
            {
                player.enemy_alive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            baseEnemyFly = collision.gameObject.GetComponent<BaseEnemyFly>();

            player.enemy_alive = true;
            player.hit_enemy = true;
            player.hit_enemy_pos = collision.gameObject.transform.position;
            //Debug.Log("敵の位置:" + player.hit_enemy_pos);
        }
    }
}