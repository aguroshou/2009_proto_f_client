using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour, IBulletDamaged
{
    public int hp = 5;

    [SerializeField]
    private AudioClip breakClip;

    [SerializeField]
    private float breakVolume;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private Vector3 targetpos;

    void Start()
    {
        targetpos = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time) * 1.0f + targetpos.x, targetpos.y, targetpos.z);
    }

    public void AddDamage(int damage)
    {
        hp -= damage;
        sr.DOColor(Color.red, 0.1f)
            .SetLoops(2, LoopType.Yoyo);
        if (hp < 0)
        {
            SEManager.Instance.PlayOneShot(breakClip, breakVolume);
            Destroy(gameObject);
        }
    }
}