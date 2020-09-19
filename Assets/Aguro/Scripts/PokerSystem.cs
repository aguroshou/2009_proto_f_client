using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerSystem : MonoBehaviour
{
    //シューティングのシステムに渡す敵の数
    public int enemyNumber;

    //ポーカーのカードの枚数は5枚で固定
    const int numberOfCard = 5;

    [SerializeField] List<Sprite> enemyCardSpriteList = new List<Sprite>();
    [SerializeField] List<Sprite> playerCardSpriteList = new List<Sprite>();

    [SerializeField] GameObject[] enemyCardObjects = new GameObject[numberOfCard];
    [SerializeField] GameObject[] playerCardObjects = new GameObject[numberOfCard];

    int[] enemyCardsNumber = new int[numberOfCard];
    int[] playerCardsNumber = new int[numberOfCard];

    int enemyCardType = 6;
    int playerCardType = 6;

    bool[] doShufflePlayerCards = new bool[numberOfCard];

    private void Start()
    {
        StartPorker();
    }

    void StartPorker()
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardsNumber[i] = Random.Range(0, enemyCardType);
            playerCardsNumber[i] = Random.Range(0, playerCardType);
        }
        ChangeEnemyCardSprite();
    }

    public void ChangeEnemyCard()
    {
        int[] enemyCardTypeEachSum = new int[enemyCardType];
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardTypeEachSum[enemyCardsNumber[i]]++;
        }

        bool[] needToShuffleEnemyCards = new bool[numberOfCard];
        for (int i = 0; i < numberOfCard; i++)
        {
            if (enemyCardTypeEachSum[enemyCardsNumber[i]]<=1)
            {
                enemyCardsNumber[i] = Random.Range(0, enemyCardType);
            }
        }
        ChangeEnemyCardSprite();
    }

    private void ChangeEnemyCardSprite()
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            Image image = enemyCardObjects[i].GetComponent<Image>();
            image.sprite = enemyCardSpriteList[enemyCardsNumber[i]];
        }
    }
}