using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private BasicPlayer CreaterPlayer;
    public string EnemyTag ="Enemy";
    private Transform target;
    private Vector3 targetPosition;
    public float Speed;
    public float Damage;
    public float Force = 0.5f;
    public float createTime = 0f;
    public void InitArrow(string tag, Transform enemy, float speed, float damage, float force)
    {
        EnemyTag = tag;
        target = enemy;
        Speed = speed;
        Damage = damage;
        Force = force;
        Vector3 distance = target.transform.position - targetPosition;
        transform.right = distance;
        createTime = 0f;
    }
    public void InitArrow(string tag, Transform enemy, float speed, float damage, float force , BasicPlayer creater)
    {
        CreaterPlayer = creater;
        InitArrow( tag, enemy, speed, damage, force);
    }
    public void Update()
    {
        createTime += Time.deltaTime;
        if (target == null || target.gameObject.activeInHierarchy == false)
        {
            target = null;
            if(createTime > 2f)
            {
                Destroy(gameObject);
                return;
            }
        }
        if (target != null && target.gameObject.activeInHierarchy == true)
        {
            targetPosition = target.transform.position;
        }
        Vector3 distance = targetPosition - transform.position;
        if(distance.magnitude > Speed * Time.deltaTime)
        {
            transform.position = transform.position + distance.normalized * Speed * Time.deltaTime;
            transform.right = distance;
        }
        else
        {
            transform.position = targetPosition;
        }

    }



    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == EnemyTag)
        {
            BasicEnemy enemy = collision.GetComponent<BasicEnemy>();
            if(enemy != null)
            {
                Vector2 contactPosition = collision.ClosestPoint(transform.position);
                Vector2 addforce = ((Vector2)collision.transform.position - contactPosition).normalized * Force;
                enemy.GetDamage(Damage, addforce , AttackType.Arrow, CreaterPlayer);
                Destroy(gameObject);
            }
        }
    }
}
