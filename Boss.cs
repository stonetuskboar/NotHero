using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss :BasicEnemy
{
    public float LossHp = 0;
    public float LossHpLimit;
    public float walkWaitTime = 0f;
    public float SpellLeftTime = 0;
    public float fleeLeftTIme = 0;

    public float fleetime = 0;
    public float chargetime = 0;

    public float idleTime = 0;
    public float attackTime = 0;

    public bool isChargeBack = false;
    public Vector3 fleeposition;
    // Start is called before the first frame update
    public override void OnGetDamage(float damage)
    {
        LossHp += damage;
    }

    public override void AfterInit()
    {
        LossHpLimit = MaxHp / 4;
    }

    public override void ChangeToAttackState()
    {
        rb2D.velocity = Vector2.zero;
        animator.SetFloat("attackSpeed", AttackSpeed);

        float rand = Random.Range(0,500);
        if (nowHp < MaxHp/2  )
        {
            rand = Random.Range(0, 650);
        }
        if(rand > 500 )
        {
            if( chargetime < 0)
            {
                ChangeToChargeState();
                attackLeftTime += Random.Range(0.5f, 1f);
                nowState = PlayerAnimation.attack;
            }
        }
        else if (rand > 100)
        {
            animator.SetBool("attack", true);
            attackLeftTime += Random.Range(0.5f,2f);
            nowState = PlayerAnimation.attack;
        }
        else
        {
            if(SpellLeftTime < 0)
            {
                SpellLeftTime = 12f;
                animator.SetBool("spell", true);
                nowState = PlayerAnimation.attack;
                attackLeftTime += 1.5f;
                walkWaitTime = 1.5f;
            }

        }
    }
    public void AttackEnemy()
    {
        AudioManager.instance.PlayOneShotAt(4);
        if(targetPlayer != null)
        {
            Vector2 rbForce = (targetPlayer.transform.position - transform.position).normalized * attackForce;
            targetPlayer.GetDamage(AttackDamage, rbForce, AttackType.Melee, this);
        }
        enemyManager.CreateEvilBlurst(targetPlayer.transform);
    }
    public void SpellEnemy()
    {
        AudioManager.instance.PlayOneShotAt(5);
       Vector3 position = transform.position + new Vector3(Random.Range(-2f,2f),Random.Range(-2f,2f),0);
       enemyManager.CreateFiveEnemy(position);
    }

    public override void AttackStateEnd()
    {
        nowState = PlayerAnimation.idle;
        animator.SetBool("attack", false);
    }

    public void SpellStateEnd()
    {
        nowState = PlayerAnimation.idle;
        animator.SetBool("spell", false);
    }

    public void ChangeToChargeState()
    {
        isChargeBack = false;
        nowState = PlayerAnimation.charge;
        animator.SetBool("charge", true);
    }

    public void ChargeToPlayer()
    {

        float ogForce = attackForce;
        float attack = AttackDamage;
        AttackDamage *= 2;
        attackForce *= 2;
        Vector3 distance = targetPlayer.transform.position - transform.position;
        rb2D.MovePosition(transform.position + distance.normalized * (distance.magnitude - 1));
        DealDamanaToEnemey();
        AttackDamage = attack;
        attackForce = ogForce;
        isChargeBack = true;
    }

    public void ChargeStateEnd()
    {


        Vector3 distance = targetPlayer.transform.position - transform.position;

        rb2D.velocity = Vector3.zero;
        isChargeBack = false;
        nowState = PlayerAnimation.idle;
        animator.SetBool("charge", false);
        chargetime = 8f;
        fleeposition = transform.position - distance.normalized * range;
        fleetime = 3f;
        fleeLeftTIme += 6f;
        ChangeToFleeState();

    }

    public override void ChangeToWalkState()
    {
        if (walkWaitTime <= 0)
        {
            nowState = PlayerAnimation.walk;
            animator.SetBool("walk", true);
        }
    }

    public void ChangeToFleeState()
    {

        nowState = PlayerAnimation.flee;
        animator.SetBool("walk", true);
    }
    public void FleeStateEnd()
    {
        nowState = PlayerAnimation.idle;
        animator.SetBool("walk", false);
    }

    public override void OnUpdate(Vector3 distance)
    {
        SpellLeftTime -= Time.deltaTime;
        walkWaitTime -= Time.deltaTime;
        chargetime -= Time.deltaTime;
        fleeLeftTIme -= Time.deltaTime;


        if (animator.GetBool("walk") == true)
        {
            idleTime += Time.deltaTime;
        }
        else
        {
            idleTime = 0;
        }

        if (attackTime > 2f)
        {
            animator.SetBool("attack", false);
            attackTime = 0;
        }
        if (animator.GetBool("attack") == true)
        {
            attackTime += Time.deltaTime;
        }
        else
        {
            attackTime = 0;
        }

        if (attackTime > 3f)
        {
            animator.SetBool("attack", false);
            attackTime = 0;
        }

        if (distance.magnitude < range/2 && fleeLeftTIme <0)
        {
            fleetime = 3f;
            fleeLeftTIme = 10f;
            ChangeToFleeState();
            fleeposition = transform.position - distance.normalized * range;
            
        }
        if(nowState == PlayerAnimation.flee)
        {
            fleetime -= Time.deltaTime;
            Vector3 dis2 = fleeposition - transform.position;
            rb2D.velocity =  3 * MoveSpeed * (dis2).normalized;

            if(fleetime < 0 || dis2.magnitude < 0.5f )
            {
                FleeStateEnd();
            }
        }

        if (nowState == PlayerAnimation.charge )
        {
            if(isChargeBack == false)
            {
                rb2D.velocity = 6 * MoveSpeed * distance.normalized;

                if (distance.magnitude < 2f)
                {
                    animator.Play("charge",0,0.45f);
                    ChargeToPlayer();
                }
            }
            else
            {
                rb2D.velocity =  - 6 * MoveSpeed * distance.normalized;
            }

        }

    }

    public override void ChangeToDeathState()
    {
        SceneManager.LoadScene("GoodEnd");
        LifeBarCanvas.enabled = false;
        IsAlive = false;
        rb2D.velocity = Vector2.zero;
        rb2D.simulated = false;
        nowState = PlayerAnimation.death;
        animator.SetBool("death", true);
        animator.SetBool("attack", false);
        animator.SetBool("walk", false);
    }
}
