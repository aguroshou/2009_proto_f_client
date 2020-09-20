using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    public int attackPoint = 1;  // 攻撃力

    public float _duration = 0.1f;

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
            var b = Instantiate(bullet, transform.position, Quaternion.identity);
            var bulletScript = b.GetComponent<Bullet>();
            bulletScript.attackPoint = attackPoint;  // 攻撃力を変更
            yield return new WaitForSeconds(_duration);
        }
    }
}
