using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStageAndBackground : MonoBehaviour
{

    [SerializeField] List<Sprite> enemyCardSpriteList = new List<Sprite>();

    //ポーカーのカードの枚数は5枚で固定
    const int numberOfCard = 5;

    int[] enemyCardsNumber = new int[numberOfCard];
    int[] playerCardsNumber = new int[numberOfCard];

    int enemyCardVariety;
    int playerCardVariety;

    void PorkerStart()
    {
        int[] enemyCardsNumber = new int[5];
        int[] playerCardsNumber = new int[5];
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardsNumber[i] = Random.Range(0, numberOfCard);
            playerCardsNumber[i] = Random.Range(0, numberOfCard);
        }

    }

}