using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Wolf : Enemy
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    #region States
    public Enemy_WolfIdleState idleState { get; private set; }
    public Enemy_WolfMoveState moveState { get; private set; }
    public Enemy_WolfBattleState battleState { get; private set; }
    public Enemy_WolfStunState stunState { get; private set; }
    public Enemy_WolfAttackState attackState { get; private set; }
    public Enemy_WolfDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_WolfIdleState(this,stateMachine,"Idle",this);
        moveState = new Enemy_WolfMoveState(this, stateMachine, "Move", this);
        battleState = new Enemy_WolfBattleState(this, stateMachine, "Move", this);
        attackState = new Enemy_WolfAttackState(this, stateMachine, "Attack", this);
        stunState = new Enemy_WolfStunState(this, stateMachine, "Stun", this);
        deadState = new Enemy_WolfDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Intialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            stateMachine.ChangeState(stunState);
        }
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned()) 
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
