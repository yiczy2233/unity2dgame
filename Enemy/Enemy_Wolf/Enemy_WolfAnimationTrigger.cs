using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WolfAnimationTrigger : MonoBehaviour
{
    private Enemy_Wolf enemy =>GetComponentInParent<Enemy_Wolf>();
    private void AnimationTrigger() 
    {
        enemy.AnimationFinishTrigger();
    
    }
    private void AttackTrigger() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null) 
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }

        }
    }
    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
