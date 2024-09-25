using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : BasicPlayer
{
    public Transform arrowPosition;
    public Transform arrowPool;
    public GameObject arrowPrefab;
    public float ArrowSpeed;
    public void InitPlayer(PlayerManager pmanager)
    {
        playerManager = pmanager;
    }

    public override void Update()
    {
        if (IsTargetOk() == false)
        {
            targetEnemy = enemyManager.GetClosestEnemy(transform);
            if (targetEnemy == null)
            {
                if (nowState == PlayerAnimation.walk)
                {
                    WalkStateEnd();
                }
                return;
            }
        }
        float realRange = range + targetEnemy.AdditionalSize;
        Vector2 distance = targetEnemy.transform.position - transform.position;
        attackLeftTime -= Time.deltaTime;
        if (distance.x > 0)
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
            if (distance.magnitude > realRange)
            {
                ChangeToWalkState();
            }else if(distance.magnitude > realRange / 3)
            {
                if (attackLeftTime <= 0f)
                {
                    AttackEnemy();
                }
            } else
            {
                ChangeToFleeState();
            }
        }
        if (nowState == PlayerAnimation.walk)
        {
            if (distance.magnitude > realRange)
            {
                rb2D.velocity = distance.normalized * GetMoveSpeed();
            }
            else
            {
                WalkStateEnd();
                if (distance.magnitude > realRange / 3) // 跑到1/2的距离才会继续攻击
                {
                    if (attackLeftTime <= 0f)
                    {
                        AttackEnemy();
                    }
                }
                else
                {
                    ChangeToFleeState();
                }
            }
        }
        if (nowState == PlayerAnimation.flee)
        {
            if (distance.magnitude < realRange / 2) // 跑到1/2的距离才会继续攻击
            {
                if(Mathf.Abs(transform.position.y) > 11)
                {
                    rb2D.velocity = -(2 + GetMoveSpeed()) * (distance * new Vector2(3,1)).normalized;
                }
                else
                {
                    rb2D.velocity = -(2 + GetMoveSpeed()) * distance.normalized;
                }
            }
            else
            {
                FleeStateEnd();
                if (attackLeftTime <= 0f)
                {
                    AttackEnemy();
                }
            }
        }
    }
    public override void Upgrade()
    {
        MaxHp += BasicHp * 0.05f;
        nowHp += BasicHp * 0.05f;
        attackDamage += BasicAttackDamage * 0.1f;
        attackSpeed += BasicAttackSpeed * 0.1f;
        moveSpeed += BasicMoveSpeed * 0.05f;
    }

    public void ChangeToFleeState()
    {
        nowState = PlayerAnimation.flee;
        animator.SetBool("walk", true);
    }

    public void FleeStateEnd()
    {
        rb2D.velocity = Vector2.zero;
        nowState = PlayerAnimation.idle;
        animator.SetBool("walk", false);
    }

    public void ShootArrow()
    {
        if (IsTargetOk() == true)
        {
            Vector2 distance = targetEnemy.transform.position - transform.position;
            if (distance.magnitude < range * 1.5f)
            {
                AudioManager.instance.PlayOneShotAt(1);
                GameObject tmp = Instantiate(arrowPrefab, arrowPool);
                tmp.transform.position = arrowPosition.position;
                Arrow arrowScript = tmp.GetComponent<Arrow>();
                arrowScript.InitArrow("Enemy", targetEnemy.transform, ArrowSpeed, GetAttackDamage(), attackForce, this);
                // 攻击之后
                if (distance.magnitude < range / 3)
                {
                    AttackStateEnd();
                    ChangeToFleeState();
                }
                else if (StabAmount > 0 )
                {
                    StabAmount--;
                    animator.Play("attack", 0, StabAnimationProgress);
                }
            }
            else
            {
                AttackStateEnd();
            }
        }
        else
        {
            AttackStateEnd();
        }
    }


}