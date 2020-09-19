using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoreCrystal : MonoBehaviour, IBulletDamaged
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDamage(int damage)
    {
        hp -= damage;
        sr.DOColor(Color.red, 0.1f)
            .SetLoops(2, LoopType.Yoyo);
        if (hp < 0)
        {
            SEManager.Instance.PlayOneShot(breakClip, breakVolume);
            GameManager.Instance.ChangePhase();
            Destroy(gameObject);
        }
    }
}
