using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BasicEnemy : MonoBehaviour
{
    public int id = 0;
    public bool IsAlive = true;
    public float nowHp;
    public float MaxHp;
    public float MoveSpeed;
    public float AttackSpeed;
    public float AttackDamage;
    public float Defense;
    public float Toughness;
    public float attackForce = 50f;
    public float range = 1f;
    public float AdditionalSize = 0f;
    public int Exp;
    protected Rigidbody2D rb2D;
    protected Canvas LifeBarCanvas;
    public Image lifeBar;
    protected PlayerManager playerManager;
    protected EnemyManager enemyManager;
    protected DamageTextPool damageTextPool;
    protected BasicPlayer targetPlayer;
    public PlayerAnimation nowState = PlayerAnimation.idle;
    protected Animator animator;

    public float attackLeftTime = 0f;
    protected SpriteRenderer sr;

    private float nextRaycastTime = 0f;
    public bool IsRaycastCollider = false;

    public virtual void Awake()
    {
        LifeBarCanvas = transform.Find("lifeBarCanvas").GetComponent<Canvas>();
        sr = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void Init(EnemyData data , PlayerManager pm, EnemyManager em , DamageTextPool dtp)
    {
        Init(data.MaxHp, data.MoveSpeed, data.AttackSpeed, data.AttackDamage, data.Defense, data.Toughness, data.Force,data.Exp,data.Range,data.size, pm, em , dtp);
    }
    public void Init(float maxHp, float moveSpeed, float attackSpeed, float attackDamage, float defense, float toughness, float force, int exp, float Range, float size, PlayerManager pm,EnemyManager em , DamageTextPool dtp)
    {
        MaxHp = maxHp;
        nowHp = maxHp;
        MoveSpeed = moveSpeed;
        AttackSpeed = attackSpeed;
        AttackDamage = attackDamage;
        Defense = defense;
        Toughness = toughness;
        attackForce = force;
        range = Range;
        AdditionalSize = size;
        Exp = exp;
        playerManager = pm;
        enemyManager = em;
        damageTextPool = dtp;
        AfterInit();
    }

    public virtual void AfterInit()
    {

    }

    public virtual void FreshFromPool()
    {
        nowHp = MaxHp;
        lifeBar.fillAmount = nowHp / MaxHp;
        rb2D.velocity = Vector2.zero;
        nowState = PlayerAnimation.idle;
        animator.SetBool("death", false);
        animator.SetBool("hurt", false);
        animator.SetBool("attack", false);
        animator.SetBool("walk", false);
        rb2D.simulated = true;
        LifeBarCanvas.enabled = true;
        gameObject.SetActive(true);

        SetTargetPlayer(playerManager.GetClosestPlayer(transform));
        IsAlive = true;
    }

    public void Update()
    {
        if(targetPlayer == null || targetPlayer.IsAlive == false)
        {
            SetTargetPlayer(playerManager.GetClosestPlayer(transform));
            if (targetPlayer == null)
            {
                return;
            }
        }
        Vector3 distance = targetPlayer.transform.position - transform.position;
        if (distance.x > 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        attackLeftTime -= Time.deltaTime;
        OnUpdate(distance);
        if (nowState == PlayerAnimation.idle)
        {
            rb2D.velocity = Vector2.zero;
            if (distance.magnitude < range)
            {
                if (attackLeftTime <= 0f)
                {
                    AttackPlayer();
                }
            }
            else
            {
                ChangeToWalkState();
            }
        }

        if (nowState == PlayerAnimation.walk)
        {
            nextRaycastTime -= Time.deltaTime;
            if (distance.magnitude < range)
            {
                WalkStateEnd();
                if (attackLeftTime <= 0f)
                {
                    AttackPlayer();
                }
            }
            else
            {
                if(distance.magnitude < range + 4f)
                {
                    if (nextRaycastTime <= 0f)
                    {
                        CheckRayCast();
                    }
                    if (IsRaycastCollider == true)
                    {
                        if (distance.y > 0)
                        {
                            rb2D.velocity = new Vector2(-distance.y  , distance.x).normalized * MoveSpeed;
                        }
                        else
                        {
                            rb2D.velocity = new Vector2(distance.y  , -distance.x).normalized * MoveSpeed;
                        }
                    }
                    else
                    {
                        rb2D.velocity = distance.normalized * MoveSpeed;
                    }
                }
                else
                {
                    rb2D.velocity = distance.normalized * MoveSpeed;
                }

            }
        }
    }


    public virtual void OnUpdate(Vector3 distance)
    {

    }

    public virtual void AttackPlayer()
    {
        ChangeToAttackState();
        attackLeftTime = 1 / AttackSpeed;
    }


    public void CheckRayCast()
    {
        nextRaycastTime += Random.Range(0.1f, 0.25f);
        List<RaycastHit2D> hitList = new();
        float distance = (targetPlayer.transform.position - transform.position).magnitude - range;
        if(distance <= 0)
        {
            IsRaycastCollider = false;
            return;
        }
        int amount = Physics2D.Raycast(transform.position, targetPlayer.transform.position, enemyManager.enemyFilter, hitList , distance);
        for (int i = 0; i < amount; i++)
        {
            if (hitList[i].transform == transform)
            {
                amount--;
            }
        }
        if(amount > 0 )
        {
            IsRaycastCollider = true;
        }
        else
        {
            IsRaycastCollider = false;
        }
    }

    public void DealDamanaToEnemey()
    {
        if (targetPlayer != null)
        {
            AudioManager.instance.playPlayerHitSound();
            Vector2 rbForce = (targetPlayer.transform.position - transform.position).normalized * attackForce;
            targetPlayer.GetDamage(AttackDamage, rbForce, AttackType.Melee ,this);
        }
    }

    public virtual void GetDamage(float damage , Vector2 force, AttackType type , BasicPlayer player)
    {
        damage -= Defense;
        if(damage > 0)
        {
            nowHp -= damage;
            OnGetDamage(damage);
            lifeBar.fillAmount = nowHp / MaxHp;
            damageTextPool.CreateTextAt(damage.ToString("F0"), lifeBar.transform.position);
        }
        if (damage > Toughness)
        {
            rb2D.AddForce(force, ForceMode2D.Impulse);
            ChangeToHurtState();
            if (type == AttackType.Melee)
            {
                SetTargetPlayer(player);
            }
        }
        if(nowHp <= 0 && nowState != PlayerAnimation.death)
        {
            ChangeToDeathState();
        }
    }

    public virtual void OnGetDamage(float damage)
    {

    }

    public void SetTargetPlayer(BasicPlayer player)
    {
        targetPlayer = player;
    }

    public void ChangeToHurtState()
    {
        nowState = PlayerAnimation.hurt;
        animator.SetBool("hurt", true);
        animator.SetBool("attack", false);
        animator.SetBool("walk", false);
    }

    public void HurtStateEnd()
    {
        nowState = PlayerAnimation.idle;
        animator.SetBool("hurt", false);
    }

    public virtual void ChangeToAttackState()
    {
        rb2D.velocity = Vector2.zero;
        animator.SetFloat("attackSpeed", AttackSpeed);
        nowState = PlayerAnimation.attack;
        animator.SetBool("attack", true);
    }
    public virtual void AttackStateEnd()
    {
        nowState = PlayerAnimation.idle;
        animator.SetBool("attack", false);
    }

    public virtual void ChangeToWalkState()
    {
        nowState = PlayerAnimation.walk;
        animator.SetBool("walk", true);
    }
    public void WalkStateEnd()
    {
        rb2D.velocity = Vector2.zero;
        nowState = PlayerAnimation.idle;
        animator.SetBool("walk", false);
    }
    public virtual void ChangeToDeathState()
    {
        LifeBarCanvas.enabled = false;
        IsAlive = false;
        rb2D.velocity = Vector2.zero;
        rb2D.simulated = false;
        nowState = PlayerAnimation.death;
        animator.SetBool("death", true);
        animator.SetBool("attack", false);
        animator.SetBool("walk", false);
    }

    public void UnPoolThisGameobject()
    {
        playerManager.AddExp(Exp);
        enemyManager.UnPoolThis(this); //其实是从场景的池回到备用池，我写反了。
    }

}
