using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BasicPlayer
{
    public Animator tauntAnimator;
    public void InitPlayer(PlayerManager pmanager)
    {
        playerManager = pmanager;
    }

    public override void Upgrade()
    {
        MaxHp += BasicHp * 0.15f;
        nowHp += BasicHp * 0.15f;
        moveSpeed += BasicMoveSpeed * 0.05f;
        attackDamage += BasicAttackDamage * 0.05f;
        attackSpeed += BasicAttackSpeed * 0.05f;
    }
    public override void OnDealMeleeDamage()
    {
        int rand = Random.Range(0,4);
        if(rand == 0)
        {
            tauntAnimator.Play("Taunt", 0);
            enemyManager.TauntEnemyArea(3f, this);
        }
    }
}


