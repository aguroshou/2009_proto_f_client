using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    private SpawnerController enemySpawner;

    /// <summary>
    /// 敵の数
    /// </summary>
    public int EnemyCount
    {
        get { return enemySpawner.SpawnCount; }
        set
        {
            if(value > 0)
            {
                enemySpawner.SpawnCount = value;
            }
            else
            {
                Debug.LogError("敵の出現数を0より小さく出来ません");
            }
        }
    }

    void Awake()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<SpawnerController>();
    }


}
