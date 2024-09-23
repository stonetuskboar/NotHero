using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSkill : BasicSkill
{
    public override void UseSkill()
    {
        skillManager.CreateMagicEffectAt(targetPlayer.transform);
        targetPlayer.AddHp(buffValue * targetPlayer.MaxHp);
    }
}
