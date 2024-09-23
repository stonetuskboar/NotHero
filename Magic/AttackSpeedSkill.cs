using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedSkill : BasicSkill
{
    public override void UseSkill()
    {
        skillManager.CreateMagicEffectAt(targetPlayer.transform);
        targetPlayer.AttackSpeedBuff += Buff;
    }

    public override void Buff(ref float attackSpeed)
    {
        attackSpeed *= 1 + buffValue;
    }

    public override void OnBuffEnd()
    {
        targetPlayer.AttackSpeedBuff -= Buff;
    }
}
