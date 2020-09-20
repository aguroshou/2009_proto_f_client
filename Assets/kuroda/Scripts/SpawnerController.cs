using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] PokerSystem ps;

    public GameObject enemyPrefab;

    void Start()
    {
        int count;

        GameManager.Instance.Phase.Subscribe((phase) => {
            if(phase == GameManager.EGamePhase.SHOOTING_PHASE)
            {
                for (count = 1; count <= 3; count = count + 1)
                {
                    GameObject enemy = Instantiate(enemyPrefab, transform);  // EnemySpawnerの子に生成

                    enemy.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(0, 4f), 1);
                }
            }
            else
            {
                foreach(Transform n in gameObject.transform)
                {
                    Destroy(n.gameObject);
                }
            }
        });
    }
}
