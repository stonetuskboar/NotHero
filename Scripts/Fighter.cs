using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : BasicPlayer
{
    public void InitPlayer(PlayerManager pmanager)
    {
        playerManager = pmanager;
    }
    public override void Upgrade()
    {
        MaxHp += BasicHp * 0.1f;
        nowHp += BasicHp * 0.1f;
        moveSpeed += BasicMoveSpeed * 0.05f;
        attackDamage += BasicAttackDamage * 0.05f;
        attackSpeed += BasicAttackSpeed * 0.1f;
    }
}


