using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 10;
    public float maxSpeed = 3;
    public GameObject damageText;

    private AIPath aiPath;
    private Animator animator;
    private Transform attackPoint;
    private Rigidbody2D rb;

    public float attackRange = 0.3f;
    public float attackCooldown = 2f;
    public float nextTime = 0f;
    public LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 9);
        currentHealth = maxHealth;
        aiPath = GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackPoint = transform.Find("Axe/AttackPoint");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", aiPath.desiredVelocity.x);
        if (aiPath.desiredVelocity.x != 0)
        {
            animator.SetFloat("LastHorizontal", aiPath.desiredVelocity.x);
        }
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        var text = Instantiate(damageText, new Vector3(transform.position.x, transform.position.y+0.5f), Quaternion.identity);
        text.GetComponentInChildren<TMP_Text>().text = ""+damage;
        Destroy(text, 2f);
        Freeze();
        SoundManager.Instance.Play("EnemyHurt");
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            SoundManager.Instance.Play("EnemyDeath");
            GameManager.Instance.killCountEvent.Invoke();
        }
    }

    public void endHurt() => UnFreeze();
    public void endDeath() => Destroy(gameObject);
    public void Freeze() 
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        aiPath.isStopped = true;
    }
    public void UnFreeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        aiPath.isStopped = false;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        SoundManager.Instance.Play("EnemyAttack");
    }
    public void DealDamage()
    {
        if(attackPoint != null)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D hit in hitPlayer)
            {
                if (hit.CompareTag("Player"))
                {
                    hit.GetComponent<PlayerController>().TakeDamage(damage);
                }
            }
        }
    }
}
