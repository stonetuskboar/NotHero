using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArchery : BasicEnemy
{
    public Transform arrowPosition;
    public GameObject arrowPrefab;
    public float ArrowSpeed;


    public void ShootArrow()
    {
        AudioManager.instance.PlayOneShotAt(2);
        GameObject tmp = Instantiate(arrowPrefab, arrowPosition);
        EnemyArrow arrowScript = tmp.GetComponent<EnemyArrow>();
        arrowScript.InitArrow("Player", targetPlayer.transform, ArrowSpeed, AttackDamage, attackForce , this);
    }
}
