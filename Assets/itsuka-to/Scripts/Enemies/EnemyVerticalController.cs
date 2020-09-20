using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVerticalController : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField]
    float speedY = 1.0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, speedY);
    }
}
