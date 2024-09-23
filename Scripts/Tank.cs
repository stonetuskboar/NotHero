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
        MaxHp *= 1.15f;
        nowHp += MaxHp * 0.15f;
        moveSpeed *= 1.05f;
        attackDamage *= 1.05f;
        attackSpeed *= 1.1f;
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


