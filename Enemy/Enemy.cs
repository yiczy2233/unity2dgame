using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]protected LayerMask whatIsPlayer;
    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDirection;
    public bool canBeStun;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultmoveSpeed;

    [Header("Attack info")]
    public float attackDistance;
    public float dashSpeed =20f;
    public float attackCooldown;
    [HideInInspector]public float lastTimeAttacked;
    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }

   protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultmoveSpeed = moveSpeed;
    }
    public override void SlowEntity(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultmoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen) 
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else 
        {
            moveSpeed = defaultmoveSpeed;
            anim.speed = 1;
        }
    
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimeCoroutine(_duration));

    protected virtual IEnumerator FreezeTimeCoroutine(float _seconds) 
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow() 
    {
        canBeStun = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow() 
    {
        canBeStun=false;
        counterImage.SetActive(false);
    }
    #endregion
    public virtual bool CanBeStunned() 
    {
        if (canBeStun) 
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void AssignLastAnimName(string _animBoolName) 
    {
        lastAnimBoolName= _animBoolName;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,50,whatIsPlayer);
    public virtual void AnimationFinishTrigger() =>stateMachine.currentState.AnimationFinishTrigger();
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
