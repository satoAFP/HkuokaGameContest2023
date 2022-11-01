using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    private Player_Ver2 player;

    private GameObject enemyObj;

    void Start()
    {
        player = transform.root.GetComponent<Player_Ver2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyObj = collision.gameObject;

            if (enemyObj != null)
            {
                player.enemy_alive = true;
                player.hit_enemy = true;
                player.hit_enemy_pos = collision.gameObject.transform.position;
                Debug.Log("�G�̈ʒu:" + player.hit_enemy_pos);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject == null)
            {
                player.enemy_alive = false;
                Debug.Log("�G�|����");
            }
        }
    }
}