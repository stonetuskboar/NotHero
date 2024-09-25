using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public List<BasicPlayer> PlayerList = new List<BasicPlayer>();
    public EnemyManager enemyManager;
    public SkillManager skillManager;
    public Image ExpBar;
    public int NowExp = 0;
    public int MaxExp = 100;
    public int NowLevel = 1;
    public void Start()
    {
        MaxExp = 50;
        ExpBar.fillAmount = NowExp / MaxExp;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerList[i].playerManager = this;
            PlayerList[i].enemyManager = enemyManager;
        }
    }

    public BasicPlayer GetClosestPlayer(Transform transform)
    {
        if(PlayerList.Count <= 0)
        {
            return null;
        }

        float MinDistance = 999999f;
        int index = -1;
        float distance;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            distance = (PlayerList[i].transform.position - transform.position).magnitude;
            if (distance < MinDistance && PlayerList[i].IsAlive == true) { 
                MinDistance = distance;
                index = i;
            }
        }
        if(index < 0)
        {
            return null;
        }
        else
        {
            return PlayerList[index];
        }
    }
    public BasicPlayer FindAlivePlayer()
    {
        BasicPlayer player = null;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].IsAlive == true)
            {
                player = PlayerList[i];
                return player;
            }
        }
        return player;
    }
    public BasicPlayer FindNonPriestPlayer( BasicPlayer priest)
    {
        BasicPlayer player = null;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].IsAlive == true && PlayerList[i] != priest)
            {
                player = PlayerList[i];
                return player;
            }
        }
        return player;
    }
    public BasicPlayer FindPlayerByType(PlayerType type)
    {
        BasicPlayer player = null;
        for(int i = 0;i < PlayerList.Count;i++)
        {
            if (PlayerList[i].PlayerType == type && PlayerList[i].IsAlive == true)
            {
                player = PlayerList[i];
            }
        }
        return player;
    }
    public BasicPlayer FindPlayerByHateness()
    {
        int rand = Random.Range(0, 10);
        if( rand == 0 )
        {
            return FindPlayerByType(PlayerType.priest);
        }
        else if ( rand == 1 )
        {
            return FindPlayerByType(PlayerType.archer);
        }else if ( rand <= 4) // 2 3 4
        {
            return FindPlayerByType(PlayerType.fighter);
        }
        else
        {
            return FindPlayerByType(PlayerType.tank);
        }
    }
    public void AddExp(int Exp)
    {
        NowExp += Exp;
        if(NowExp >= MaxExp)
        {
            NowExp -= MaxExp;
            NowLevel++;
            MaxExp = 5 * NowLevel + (int)(Mathf.Sqrt(NowLevel) * 50);
            skillManager.UpgradeSkill();
            enemyManager.DecreaseEnemyWaveTime();
            for (int i = 0; i< PlayerList.Count;i++)
            {
                PlayerList[i].Upgrade();
            }
        }
        ExpBar.fillAmount = (float)NowExp / (float)MaxExp;
    }
}
