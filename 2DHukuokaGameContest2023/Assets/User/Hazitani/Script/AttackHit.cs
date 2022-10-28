using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    private Player_Ver2 player;

    void Start()
    {
        player = transform.root.GetComponent<Player_Ver2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player.hit_enemy = true;
            player.hit_enemy_pos = collision.gameObject.transform.position;
            Debug.Log( "“G‚ÌˆÊ’u:" + player.hit_enemy_pos);
        }
    }
}