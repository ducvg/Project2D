using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public int damage = 25;
    public float lifeTime = 10f;
    public GameObject hitEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        GameManager.Instance.damageDealt += damage;
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        Physics2D.IgnoreLayerCollision(7, 9);
    }



    // Update is called once per frame
    void Update()
    {
    }
}
