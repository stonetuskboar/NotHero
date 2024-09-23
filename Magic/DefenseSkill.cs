using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSkill : BasicSkill
{
    public override void UseSkill()
    {
        skillManager.CreateMagicEffectAt(targetPlayer.transform);
        targetPlayer.DefenseBuff += Buff;
    }

    public override void Buff(ref float defense)
    {
        defense += buffValue;
    }

    public override void OnBuffEnd()
    {
        targetPlayer.DefenseBuff -= Buff;
    }
}
