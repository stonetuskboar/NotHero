using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;


public delegate void BasicBuffDelegate(ref float targetValue);
public class BasicPlayer : MonoBehaviour
{
    public bool IsAlive = true;
    public PlayerType PlayerType;
    public float nowHp;
    public float MaxHp;
    public float moveSpeed;
    public float attackSpeed;
    public float attackDamage;
    public float defense;
    public float toughness;
    public float range;
    public float attackForce = 2;
    protected Rigidbody2D rb2D;
    public PlayerManager playerManager;
    public EnemyManager enemyManager;
    public BasicEnemy targetEnemy;
    public PlayerAnimation nowState = PlayerAnimation.idle;
    protected Animator animator;
    public Image lifeBar;
    public float attackLeftTime = 0f;

    protected int StabAmount = 0;
    public float StabAnimationProgress = 0.5f;

    protected SpriteRenderer sr;
    protected Material playerMaterial;

    public BasicBuffDelegate AttackBuff = null;
    public BasicBuffDelegate DefenseBuff = null;
    public BasicBuffDelegate AttackSpeedBuff = null;
    public BasicBuffDelegate MoveSpeedBuff = null;
    public virtual void Start()
    {
        nowState = PlayerAnimation.idle;
        sr = GetComponent<SpriteRenderer>();
        playerMaterial = sr.material;
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetEnemy = enemyManager.GetClosestEnemy(transform);
    }
    public virtual void Update()
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
            if (distance.magnitude < realRange)
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
            rb2D.velocity = distance.normalized * GetMoveSpeed();
            if (distance.magnitude < realRange)
            {
                WalkStateEnd();
                if (attackLeftTime <= 0f)
                {
                    AttackEnemy();
                }
            }
        }
    }
    public void AddHp(float addHp)
    {
        nowHp += addHp;
        lifeBar.fillAmount = nowHp / MaxHp;
    }

    public virtual void AttackEnemy()
    {
        ChangeToAttackState();
        attackLeftTime = 1 / GetAttackSpeed();
    }

    public virtual void DealDamanaToEnemey()
    {

        if (IsTargetOk() == true)
        {
            float realRange = range + targetEnemy.AdditionalSize;
            Vector2 distance = targetEnemy.transform.position - transform.position;
            if(distance.magnitude < (realRange*1.5f) )
            {
                AudioManager.instance.playPlayerHitSound();
                Vector2 rbForce = (targetEnemy.transform.position - transform.position).normalized * attackForce;
                targetEnemy.GetDamage(GetAttackDamage(), rbForce, AttackType.Melee, this);
                if (StabAmount > 0)
                {
                    StabAmount--;
                    animator.Play("attack", 0, StabAnimationProgress);
                }
                OnDealMeleeDamage();
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

    public virtual void OnDealMeleeDamage()
    {

    }

    public virtual void Upgrade()
    {
        moveSpeed *= 1.05f;
    }
    public virtual void GetDamage(float damage, Vector2 force, AttackType type , BasicEnemy enemy)
    {
        damage -= GetDefense();
        if (damage > 0)
        {
            nowHp -= damage;
            lifeBar.fillAmount = nowHp / MaxHp;
        }
        if(damage > toughness)
        {
            rb2D.AddForce(force, ForceMode2D.Impulse);
            ChangeToHurtState();
        }
        if(nowHp < 0)
        {
            SceneManager.LoadScene("BadEnd");
        }
    }
    public float GetDefense()
    {
        float Defense = defense;
        DefenseBuff?.Invoke(ref Defense);
        return Defense;
    }
    public float GetAttackDamage()
    {
        float damage = attackDamage;
        AttackBuff?.Invoke(ref damage);
        return damage;
    }
    public float GetAttackSpeed()
    {
        float damage = attackSpeed;
        AttackSpeedBuff?.Invoke(ref damage);
        return damage;
    }
    public float GetMoveSpeed()
    {
        float damage = moveSpeed;
        MoveSpeedBuff?.Invoke(ref damage);
        return damage;
    }
    public virtual void ChangeToHurtState()
    {
        nowState = PlayerAnimation.hurt;
        animator.SetBool("hurt", true);
        animator.SetBool("attack", false);
        animator.SetBool("walk", false);
    }

    public virtual void HurtStateEnd()
    {
        nowState = PlayerAnimation.idle;
        animator.SetBool("hurt", false);
    }

    public virtual void ChangeToAttackState()
    {
        rb2D.velocity = Vector2.zero;
        animator.SetBool("walk", false);
        animator.SetFloat("attackSpeed", GetAttackSpeed());
        nowState = PlayerAnimation.attack;
        animator.SetBool("attack", true);
    }
    public virtual void AttackStateEnd()
    {
        nowState = PlayerAnimation.idle;
        animator.SetBool("attack", false);
        StabAmount = playerManager.NowLevel / 4;
    }

    public virtual void ChangeToWalkState()
    {
        nowState = PlayerAnimation.walk;
        animator.SetBool("walk", true);
    }
    public virtual void WalkStateEnd()
    {
        rb2D.velocity = Vector2.zero;
        nowState = PlayerAnimation.idle;
        animator.SetBool("walk", false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            if(nowState == PlayerAnimation.walk)
            {
                targetEnemy = enemyManager.GetEnemyByItsTransform(collision.transform);
            }
        }
    }

    public bool IsTargetOk()
    {
        if(targetEnemy == null || targetEnemy.IsAlive == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}

public enum PlayerType
{
    fighter = 0,
    tank = 1,
    archer = 2,
    priest = 3,
}

public enum PlayerAnimation
{
    other = -1,
    idle = 0,
    walk =1,
    attack = 2,
    hurt =3,
    death =4,
    flee = 5,
    charge = 6,
}

public enum AttackType
{
    Melee = 0,
    Arrow = 1,
}