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
            if (distance.magnitude < realRange && distance.magnitude > realRange / 3)
            {
                if (attackLeftTime <= 0f)
                {
                    AttackEnemy();
                }
            }
            else
            {
                ChangeToWalkState();
            }
        }
        if (nowState == PlayerAnimation.walk)
        {
            if (distance.magnitude > realRange)
            {
                rb2D.velocity = distance.normalized * GetMoveSpeed();
            }
            else if( distance.magnitude > realRange / 2) // 跑到1/2的距离才会继续攻击
            {
                WalkStateEnd();
                if (attackLeftTime <= 0f)
                {
                    AttackEnemy();
                }
            }
            else
            {
                rb2D.velocity = - GetMoveSpeed() * distance.normalized;
            }
        }
    }
    public override void Upgrade()
    {
        MaxHp *= 1.05f;
        nowHp += MaxHp * 0.05f;
        attackDamage *= 1.1f;
        attackSpeed *= 1.1f;
        moveSpeed *= 1.05f;
    }

    public override void GetDamage(float damage, Vector2 force, AttackType type, BasicEnemy enemy)
    {
        damage -= GetDefense();
        nowHp -= damage;
        lifeBar.fillAmount = nowHp / MaxHp;
        if (damage > toughness)
        {
            rb2D.AddForce(force, ForceMode2D.Impulse);
            ChangeToHurtState();
            if(type == AttackType.Melee)
            {
                playerManager.skillManager.MoveSpeedSkill.UseSkillToTarget(this);
                targetEnemy = enemy;
            }
        }
    }
    public void ShootArrow()
    {
        if (IsTargetOk() == true)
        {
            AudioManager.instance.PlayOneShotAt(1);
            Vector2 distance = targetEnemy.transform.position - transform.position;
            if(distance.magnitude < range * 1.5f)
            {
                GameObject tmp = Instantiate(arrowPrefab, arrowPool);
                tmp.transform.position = arrowPosition.position;
                Arrow arrowScript = tmp.GetComponent<Arrow>();
                arrowScript.InitArrow("Enemy", targetEnemy.transform, ArrowSpeed, GetAttackDamage(), attackForce, this);
                if (StabAmount > 0)
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