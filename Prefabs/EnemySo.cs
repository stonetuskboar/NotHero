using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemySo : ScriptableObject
{
    public List<EnemyData> EnemyDatas;
    public List<EnemyWave> EnemyWaves;
}

[Serializable]
public class EnemyData
{
    public int id;
    public float nowHp;
    public float MaxHp;
    public float MoveSpeed;
    public float AttackSpeed;
    public float AttackDamage;
    public float Defense;
    public float Toughness;
    public float Force = 0.5f;
    public float Range = 1f;
    public float size = 1f;
    public int Exp;
}

[Serializable]
public class EnemyWave
{
    public int waveId;
    public List<int> Enemys;
    public int enemyAmount;
}
