using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WolfMoveState : Enemy_WolfGroundState
{
    public Enemy_WolfMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Wolf enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected()) 
        {
            enemy.Filp();
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}