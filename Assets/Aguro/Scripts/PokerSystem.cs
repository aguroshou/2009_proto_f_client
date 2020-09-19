using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerSystem : MonoBehaviour
{
    //ポーカーのカードの枚数は5枚で固定
    const int numberOfCard = 5;

    //敵の種類とカードの種類は変更がある可能性
    int enemyCardType = 6;

    const int playerCardType = 6;

    //シューティングのシステムに渡す敵の数
    public int enemyNumber;

    //シューティングのシステムに渡すPlayerの揃えたペアによる効果の配列
    public int[] playerCardTypeLevel = new int[playerCardType];
    //最低0〜最大4が代入されます
    //(例)1番目が3カード、4番目が2カードのフルハウスのとき
    //playerCardTypeLevel[0] = 0; //0番目の柄のレベルは0
    //playerCardTypeLevel[1] = 2; //1番目の柄のレベルは2
    //playerCardTypeLevel[2] = 0; //2番目の柄のレベルは0
    //playerCardTypeLevel[3] = 0; //3番目の柄のレベルは0
    //playerCardTypeLevel[4] = 1; //4番目の柄のレベルは1
    //playerCardTypeLevel[5] = 0; //5番目の柄のレベルは0

    [SerializeField] List<Sprite> playerCardSpriteList = new List<Sprite>();
    [SerializeField] List<Sprite> enemyCardSpriteList = new List<Sprite>();

    [SerializeField] GameObject[] playerCardObjects = new GameObject[numberOfCard];
    [SerializeField] GameObject[] enemyCardObjects = new GameObject[numberOfCard];

    int[] playerCardsNumber = new int[numberOfCard];
    int[] enemyCardsNumber = new int[numberOfCard];

    bool[] isPlayerCardSelected = new bool[numberOfCard];
    bool[] isEnemyCardSelected = new bool[numberOfCard];

    private void Start()
    {
        StartPorker();
    }

    void StartPorker()
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardsNumber[i] = Random.Range(0, enemyCardType);
            isEnemyCardSelected[i] = true;
        }
        for (int i = 0; i < numberOfCard; i++)
        {
            playerCardsNumber[i] = Random.Range(0, playerCardType);
            isPlayerCardSelected[i] = true;
        }
        ChangePlayerCardSprite();
        ChangeEnemyCardSprite();
    }

    public void ChangeCard()
    {
        ChangePlayerCard();
        ChangeEnemyCard();
    }

    public void ChangePlayerCard()
    {
        int[] playerCardTypeEachSum = new int[playerCardType];
        for (int i = 0; i < numberOfCard; i++)
        {
            playerCardTypeEachSum[playerCardsNumber[i]]++;
        }

        for (int i = 0; i < numberOfCard; i++)
        {
            if (isPlayerCardSelected[i] == true)
            {
                playerCardsNumber[i] = Random.Range(0, playerCardType);
            }
        }
        ChangePlayerCardSprite();
    }

    private void ChangePlayerCardSprite()
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            if (isPlayerCardSelected[i] == true)
            {
                Image image = playerCardObjects[i].GetComponent<Image>();
                image.sprite = playerCardSpriteList[playerCardsNumber[i]];
                Vector3 position = playerCardObjects[i].transform.position;
                position.y -= 100.0f;
                playerCardObjects[i].transform.position = position;
                isPlayerCardSelected[i] = false;
            }
        }
    }

    public void PlayerCardClicked(int cardNumber)
    {
        isPlayerCardSelected[cardNumber] = !isPlayerCardSelected[cardNumber];

        Vector3 position = playerCardObjects[cardNumber].transform.position;
        if (isPlayerCardSelected[cardNumber] == true)
        {
            position.y += 100.0f;
        }
        else
        {
            position.y -= 100.0f;
        }
        playerCardObjects[cardNumber].transform.position = position;
    }

    public void ChangeEnemyCard()
    {
        int[] enemyCardTypeEachSum = new int[enemyCardType];
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardTypeEachSum[enemyCardsNumber[i]]++;
        }

        for (int i = 0; i < numberOfCard; i++)
        {
            if (enemyCardTypeEachSum[enemyCardsNumber[i]]<=1)
            {
                enemyCardsNumber[i] = Random.Range(0, enemyCardType);
                isEnemyCardSelected[i] = true;
            }
        }
        ChangeEnemyCardSprite();
    }

    private void ChangeEnemyCardSprite()
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            if (isEnemyCardSelected[i] == true)
            {
                Image image = enemyCardObjects[i].GetComponent<Image>();
                image.sprite = enemyCardSpriteList[enemyCardsNumber[i]];
                isEnemyCardSelected[i] = false;
            }
        }
    }
}