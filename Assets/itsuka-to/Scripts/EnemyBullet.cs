using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
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
        rb2d.velocity = new Vector2(0f, -speed);
        StartCoroutine(TimeoutDestroyCoroutine(lifetime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
