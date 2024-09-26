using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        EnemyStats enemyTarget = collision.gameObject.GetComponent<EnemyStats>();
        if (collision.GetComponent<Enemy>() != null)
            playerStats.DoDamage(enemyTarget);
    }

}
