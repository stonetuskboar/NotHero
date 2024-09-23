using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class EnemyArrow : Arrow
{
    private BasicEnemy CreaterEnemy;
    public void InitArrow(string tag, Transform enemy, float speed, float damage, float force, BasicEnemy creater)
    {
        CreaterEnemy = creater;
        InitArrow(tag, enemy, speed, damage, force);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == EnemyTag)
        {
            BasicPlayer enemy = collision.GetComponent<BasicPlayer>();
            if(enemy != null)
            {
                Vector2 contactPosition = collision.ClosestPoint(transform.position);
                Vector2 addforce = ((Vector2)collision.transform.position - contactPosition).normalized * Force;
                enemy.GetDamage(Damage, addforce, AttackType.Arrow , CreaterEnemy);
                Destroy(gameObject);
            }
        }
    }
}
