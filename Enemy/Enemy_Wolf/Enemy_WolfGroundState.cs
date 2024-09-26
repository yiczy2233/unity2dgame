using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WolfGroundState : EnemyState
{
    protected Enemy_Wolf enemy;
    protected Transform player;
    public Enemy_WolfGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wolf enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        if (enemy.IsPlayerDetected()|| Vector2.Distance(enemy.transform.position, player.transform.position) < 3)
            stateMachine.ChangeState(enemy.battleState);
    }
}
