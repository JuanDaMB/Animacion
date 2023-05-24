using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator agentAnimator;
    private static readonly int MovingID = Animator.StringToHash("Moving");
    private static readonly int JumpingID = Animator.StringToHash("Jump");
    private static readonly int AttackingID = Animator.StringToHash("Attacking");
    private static readonly int AttackNumberID = Animator.StringToHash("Attack");
    private static readonly int DamageID = Animator.StringToHash("Damage");
    private static readonly int AliveID = Animator.StringToHash("Alive");
    private bool _jumping, _cooldown, _attacking, _damaged, _alive = true;
    private int _attack;
    private float _cooldownTime, _attackReset;

    public bool Alive()
    {
        return _alive;
    }
    public bool OnAnimation()
    {
        return _jumping || _attacking || _damaged || !_alive;
    }

    public void ControlMovement(float value)
    {
        agentAnimator.SetFloat(MovingID, value);
    }

    public void SetJump()
    {
        if (_jumping)
        {
            return;
        }
        _jumping = true;
        agentAnimator.SetBool(JumpingID,true);
        StartCoroutine(ResetJump());
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1.4f);
        agentAnimator.SetBool(JumpingID,false);
        _jumping = false;
    }
    
    public void SetDamage()
    {
        _damaged = true;
        agentAnimator.SetBool(DamageID, _damaged);
        StartCoroutine(ResetDamage());
    }

    IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(1.167f);
        _damaged = false;
        agentAnimator.ResetTrigger(DamageID);
    }

    public void SetDead()
    {
        _alive = !_alive;
        agentAnimator.SetBool(AliveID, _alive);
    }

    public void SetAttack()
    {
        if (_cooldown)
        {
            return;
        }

        _attacking = true;
        _cooldown = true;

        switch (_attack)
        {
            case 0:
                _cooldownTime = 0.5f;
                _attackReset = 0.7f;
                break;
            case 1:
                _cooldownTime = 1.6f;
                _attackReset = 1.9f;
                break;
            case 2:
                _cooldownTime = 2.6f;
                _attackReset = 2.5f;
                break;
        }

        agentAnimator.SetInteger(AttackNumberID,_attack);
        agentAnimator.SetBool(AttackingID,_attacking);
        _attack++;
    }
    private void Update()
    {
        if (_cooldownTime > 0)
        {
            _cooldownTime -= Time.deltaTime;
        }

        if (_cooldownTime <= 0)
        {
            _cooldown = false;
        }
        
        if (_attackReset > 0)
        {
            _attackReset -= Time.deltaTime;
        }

        if (_attackReset <= 0)
        {
            _attack = 0;
            _cooldown = false;
            _attacking = false;
            agentAnimator.SetBool(AttackingID,_attacking);
        }
    }
}
