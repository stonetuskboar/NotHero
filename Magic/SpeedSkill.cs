using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSkill : BasicSkill
{
    public void UseSkillToArcher(Archer player)
    {
        if(player.GetMoveSpeed() < 4)
        {
            player.MoveSpeedBuff += Buff;
            StartCoroutine(OnTargetBuff(player));
        }
    }
    public void UseSkillToTarget(BasicPlayer player)
    {
        player.MoveSpeedBuff += Buff;
        StartCoroutine(OnTargetBuff(player));
    }

    public IEnumerator OnTargetBuff(BasicPlayer player)
    {
        float time = 0f;
        while (time < BuffTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        player.MoveSpeedBuff -= Buff;
    }
    public override void UseSkill()
    {
        List<BasicPlayer> list = skillManager.PlayerManager.PlayerList;
        for(int i = 0; i < list.Count; i++)
        {
            skillManager.CreateMagicEffectAt(list[i].transform);
            list[i].MoveSpeedBuff += Buff;
        }
    }

    public override void Buff(ref float moveSpeed)
    {
        moveSpeed *= 1 + buffValue;
    }

    public override void OnBuffEnd()
    {
        for (int i = 0; i < skillManager.PlayerManager.PlayerList.Count; i++)
        {
            skillManager.PlayerManager.PlayerList[i].MoveSpeedBuff -= Buff;
        }
    }
}
