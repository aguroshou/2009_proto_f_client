using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] PokerSystem ps;

    public GameObject enemyPrefab;
    private float interval;
    private float time = 0f;

    void Start()
    {
        interval = 1f;
    }
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if(time>interval)
        {
            if(interval<=ps.enemyNumber)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.transform.position = new Vector3(Random.Range(-500f,500f), Random.Range(0,900f), 0);
            }
        }
    }
}
