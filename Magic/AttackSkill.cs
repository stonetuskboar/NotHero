using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : BasicSkill
{
    public override void UseSkill()
    {
        skillManager.CreateMagicEffectAt(targetPlayer.transform);
        targetPlayer.AttackBuff += Buff;
    }
    public override void OnBuffEnd()
    {
        targetPlayer.AttackBuff -= Buff;
    }

}
