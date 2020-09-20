using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;

    [SerializeField]
    private float _duration = 0.1f;

    private Coroutine c;

    void Start()
    {
        GameManager.Instance.Phase.Subscribe((phase) => {
            if(phase == GameManager.EGamePhase.SHOOTING_PHASE)
            {
                c = StartCoroutine(ShootingCoroutine());
            }
            else {
                if(c != null) StopCoroutine(c);
            }

        });
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
