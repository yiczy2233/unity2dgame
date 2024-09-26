using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WolfAttackState : EnemyState
{
    private Enemy_Wolf enemy;
    public Enemy_WolfAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wolf enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(1, null);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;

    }

    public override void Update()
    {
        base.Update();
       enemy.SetVelocity(enemy.dashSpeed * enemy.facingDir, 0);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
