using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDetection : MonoBehaviour
{
    private EnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
        GetComponent<CircleCollider2D>().radius = enemy.attackRange;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time >= enemy.nextTime)
            {
                enemy.nextTime = Time.time + enemy.attackCooldown;
                enemy.Attack();
                enemy.Freeze();
            }
        }
    }

}
