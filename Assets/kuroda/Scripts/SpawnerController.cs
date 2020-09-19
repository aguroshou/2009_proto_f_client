using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] PokerSystem ps;

    public GameObject enemyPrefab;

    void Start()
    {
        int count;
        for (count = 1; count <= 3; count = count + 1)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(0, 4f), 1);
        }
    }
}
