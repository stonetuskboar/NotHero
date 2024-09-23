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
        MaxHp *= 1.1f;
        nowHp += MaxHp * 0.1f;
        moveSpeed *= 1.05f;
        attackDamage *= 1.05f;
        attackSpeed *= 1.1f;
    }
}


