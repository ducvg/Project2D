using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.AI;
using Pathfinding;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float moveSpeed = 5f;
    public Transform gun;

    public Camera cam;

    private Rigidbody2D rb;
    private Collider2D collide;
    private Animator animator;
    private Vector2 movement;
    private Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        collide = GetComponent<Collider2D>(); 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("Horizontal",movement.x);
        animator.SetFloat("Vertical",movement.y);
        animator.SetFloat("Speed",movement.sqrMagnitude);

        if(movement != Vector2.zero){
            animator.SetFloat("LastHorizontal",movement.x);
            animator.SetFloat("LastVertical",movement.y);
        }

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        Vector2 gunDirection = mousePosition - rb.position;
        float angle  = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg - 90f;
        gun.rotation = Quaternion.Euler(0,0,angle);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        PlayerHealth.Instance.TakeDamage(damage);
        GameManager.Instance.damageReceived += damage;
        animator.SetTrigger("Hurt");
        SoundManager.Instance.Play("PlayerHurt");
        if (currentHealth <= 0)
        {
            Destroy(collide);
            animator.SetTrigger("Death");
            SoundManager.Instance.Play("PlayerDeath");
            animator.SetBool("IsDeath",true);
            moveSpeed = 0;
            SoundManager.Instance.Stop("background");
            SoundManager.Instance.Stop("GameOver");

            StartCoroutine(waitEndGame());   
        }
    }

    IEnumerator waitEndGame()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.GameOverEvent.Invoke();
    }

    public void endDeath()
    {
        
    }
}
