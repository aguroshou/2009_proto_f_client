using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;

    [SerializeField]
    private float _duration = 0.1f;

    void Start()
    {
        StartCoroutine(ShootingCoroutine());
    }

    void Update()
    {
        
    }

    IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_duration);
        }
    }
}
