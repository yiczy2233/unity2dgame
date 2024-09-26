using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WolfBattleState : EnemyState
{
    private Transform player;
    private Enemy_Wolf enemy;
    private int moveDir;
    public Enemy_WolfBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wolf enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
     }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Vector2.Distance(player.transform.position, enemy.transform.position) < .1f)
            stateMachine.ChangeState(enemy.idleState);
            
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else 
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idleState);
        }


        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }
    private bool CanAttack() 
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown) 
        {
            enemy.lastTimeAttacked = Time.deltaTime;
            return true;
        }

        return false;
    }
}
