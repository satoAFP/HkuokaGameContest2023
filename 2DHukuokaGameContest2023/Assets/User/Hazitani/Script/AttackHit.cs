using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    private Player_Ver2 player = null;
    private BaseEnemyFly baseEnemyFly = null;//�U�������������G�̃I�u�W�F�N�g������

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
                Debug.Log("�G���ɂ܂���");
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
            //Debug.Log("�G�̈ʒu:" + player.hit_enemy_pos);
        }
    }
}