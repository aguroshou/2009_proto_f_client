﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bullet : MonoBehaviour
{
    public float speed = 1.0f;
    public int attackPoint = 3;
    private Rigidbody2D rb2d;

    [SerializeField]
    float lifetime = 10f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb2d.velocity = new Vector2(0f, speed);
        StartCoroutine(TimeoutDestroyCoroutine(lifetime));
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO:　弾を消す
        if (collision.CompareTag("Enemy"))
        {
            var bulletDamaged = collision.gameObject.GetComponent<IBulletDamaged>();
            bulletDamaged.AddDamage(attackPoint);
            Destroy(gameObject);
        }
    }


    private IEnumerator TimeoutDestroyCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
