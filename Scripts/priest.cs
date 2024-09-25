using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class priest : BasicPlayer
{
    public BasicPlayer targetPlayer;
    public MoveCamera moveCamera;


    public void InitPlayer(PlayerManager pmanager)
    {
        playerManager = pmanager;
    }

    public override void Update()
    {
        if (targetPlayer == null || targetPlayer.IsAlive == false)
        {
            SetTargetPlayer(playerManager.FindNonPriestPlayer(this));
            if (targetPlayer == null )
            {
                return;
            }
        }
        attackLeftTime -= Time.deltaTime;

        float RealRange = range;
        if (targetPlayer.PlayerType == PlayerType.archer)
        {
            RealRange = range / 2;
        }

        Vector2 distance = targetPlayer.transform.position - transform.position;

        if(distance.x >0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        if (nowState == PlayerAnimation.idle)
        {
            rb2D.velocity = Vector2.zero;
            if (distance.magnitude > (RealRange + 1))
            {
                ChangeToWalkState();
            }
        }
        if (nowState == PlayerAnimation.walk)
        {
           rb2D.velocity = distance.normalized * GetMoveSpeed();
            if (distance.magnitude < RealRange)
            {
                WalkStateEnd();
            }
        }
    }

    public override void Upgrade()
    {
        MaxHp += BasicHp * 0.05f;
        nowHp += BasicHp * 0.05f;
        moveSpeed += BasicMoveSpeed * 0.05f;
    }

    public void TargetThisPlayer(PlayerType type)
    {
        SetTargetPlayer(playerManager.FindPlayerByType(type));
    }

    public void SetTargetPlayer(BasicPlayer player)
    {
        targetPlayer = player;
        if(player != null)
        {
            moveCamera.player = targetPlayer.transform;
        }

    }

    public override void ChangeToAttackState()
    {
        rb2D.velocity = Vector2.zero;
        animator.SetBool("walk", false);
        animator.SetFloat("attackSpeed", attackSpeed);
        nowState = PlayerAnimation.attack;
        animator.SetBool("attack",true);
    }

    public bool IsSpellOk()
    {
        if (targetPlayer != null && targetPlayer.IsAlive != false)
        {
            if (nowState == PlayerAnimation.idle || nowState == PlayerAnimation.walk)
            {
                return true;
            }
        }
        return false;
    }

    public void PriestCastSpell()
    {
        ChangeToAttackState();
    }

 
}


