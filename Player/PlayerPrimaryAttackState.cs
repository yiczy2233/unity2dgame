using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 0.1f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(3,null);

        xInput = 0;
        stateTimer = 0.1f;
        player.anim.speed = 1.1f;
        if (comboCounter > 2||Time.time>=lastTimeAttacked+comboWindow) 
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("CoboCounter", comboCounter);
        #region Choose attack direction
        float attackDir = player.facingDir;
        if(xInput!=0)
            attackDir = xInput;
        #endregion

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
        comboCounter++;
        lastTimeAttacked = Time.time;
        player.anim.speed = 1f;

    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.ZeroVelocitty();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
