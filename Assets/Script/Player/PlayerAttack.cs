using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float bulletForce = 10f;
    public Transform gunpoint;
    public GameObject bulletPrefab;
    public float attackCooldown = 1.5f;
    public float nextTime = 0f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(Time.time > nextTime)
            {
                Shoot();
                nextTime = Time.time + attackCooldown;
            }
        }
    }

    void Shoot()
    {
        SoundManager.Instance.Play("PlayerAttack");
        GameObject bullet = Instantiate(bulletPrefab, gunpoint.position, gunpoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(gunpoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
