using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using System.Linq;

public class PokerSystem : MonoBehaviour
{
    public GameObject pokerPhaseEndButton;

    //ポーカーのカードの枚数は5枚で固定
    const int numberOfCard = 5;

    //敵の種類とカードの種類は変更がある可能性
    int enemyCardType = 6;

    const int playerCardType = 6;

    //シューティングのシステムに渡す敵の数
    public int enemyNumber;


    //シューティングのシステムに渡すEnemyの揃えたペアによる出現数と種類の配列
    public int[] enemyCardTypeLevel = new int[playerCardType];
    //最低0〜最大4が代入されます
    //(例)1番目が3カード、4番目が2カードのフルハウスのとき
    //enemyCardTypeLevel[0] = 0; //0番目の敵の数は0体
    //enemyCardTypeLevel[1] = 2; //1番目の敵の数は2体
    //enemyCardTypeLevel[2] = 0; //2番目の敵の数は0体
    //enemyCardTypeLevel[3] = 0; //3番目の敵の数は0体
    //enemyCardTypeLevel[4] = 1; //4番目の敵の数は1体
    //enemyCardTypeLevel[5] = 0; //5番目の敵の数は0体

    //シューティングのシステムに渡すPlayerの揃えたペアによる効果レベルの配列
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

    [SerializeField] public GameObject shuffleCardButtonObject;
    public GameObject betPanel;

    //シャッフルボタンを2回以上押せなくするため
    //bool isShuffleButtonActive;


    private void OnEnable()
    {
        // shuffleCardButtonObject = GameObject.Find("ShuffleCardButton");
        betPanel.SetActive(true);
        pokerPhaseEndButton.SetActive(false);
        for (int i = 0; i < numberOfCard; i++)
        {
            playerCardObjects[i] = GameObject.Find("PlayerCardButton" + i.ToString());
        }
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardObjects[i] = GameObject.Find("EnemyCardImage" + i.ToString());
        }

        shuffleCardButtonObject.SetActive(true);
        StartPorker();
    }


    private void Start()
    {
        // shuffleCardButtonObject = GameObject.Find("ShuffleCardButton");
        pokerPhaseEndButton.SetActive(false);
        //GameManager.Instance.Phase.Subscribe((phase) => {
            //if(phase == GameManager.EGamePhase.POKER_PHASE)
            //{
                for (int i = 0; i < numberOfCard; i++)
                {
                    playerCardObjects[i] = GameObject.Find("PlayerCardButton" + i.ToString());
                }
                for (int i = 0; i < numberOfCard; i++)
                {
                    enemyCardObjects[i] = GameObject.Find("EnemyCardImage" + i.ToString());
                }
                
                shuffleCardButtonObject.SetActive(true);
                StartPorker();
            //}
        //});
    }

    void StartPorker()
    {
        // シャッフル
        for (int i = 0; i < numberOfCard; i++)
        {
            playerCardsNumber[i] = Random.Range(0, playerCardType);
            isPlayerCardSelected[i] = true;
        }
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardsNumber[i] = Random.Range(0, enemyCardType);
            isEnemyCardSelected[i] = true;
        }

        UpdatePlayerCardTypeLevel();
        ChangePlayerCardSprite(false);
        UpdateEnemyCardTypeLevel();
        ChangeEnemyCardSprite();
    }

    /// <summary>
    /// シャッフルボタンを押した時の動作
    /// </summary>
    public void ShuffleCards()
    {
        ShufflePlayerCards();
        ShuffleEnemyCards();
        shuffleCardButtonObject.active = false;
        SendGameManager(playerCardTypeLevel, enemyCardTypeLevel);
        pokerPhaseEndButton.SetActive(true);
    }

    /// <summary>
    /// ゲームマネージャーへ値を送る
    /// </summary>
    /// <param name="playerCardTypeLevel"></param>
    /// <param name="enemyCardTypeLevel"></param>
    public void SendGameManager(int[] playerCardTypeLevel, int[] enemyCardTypeLevel)
    {
        GameManager.Instance.SavePokerHand(CheckPair(playerCardTypeLevel), CheckPair(enemyCardTypeLevel));
    }

    public GameManager.EPokerHand CheckPair(int[] cardTypeLevel)
    {
        List<int> ctLavelList = new List<int>(cardTypeLevel);

        int pairCount = 0;
        foreach(int pairs in cardTypeLevel)
        {
            pairCount += pairs;
        }

        if(pairCount == 0)
        {
            return GameManager.EPokerHand.HIGH_CARD;  // 役なし
        }else if(pairCount == 1)
        {
            return GameManager.EPokerHand.ONE_PAIR;  // ワンペア
        }else if(ctLavelList.Where(p => p == 1).Count() == 2)
        {
            return GameManager.EPokerHand.TWO_PAIR;  // ツーペア
        }else if(ctLavelList.Where(p => p==2).Count() == 1)
        {
            if(ctLavelList.Where(p=> p==1).Count() == 1)
            {
                return GameManager.EPokerHand.FULL_HOUSE;  // フルハウス
            }
            else
            {
                return GameManager.EPokerHand.THREE_CARD;  // スリーカード
            }
        }else if (ctLavelList.Where(p => p == 3).Count() == 1)
        {
            return GameManager.EPokerHand.FOUR_CARD;  // フォーカード
        }
        else
        {
            return GameManager.EPokerHand.FIVE_CARD;  // ファイブカード
        }

    }

    public void ShufflePlayerCards()
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            if (isPlayerCardSelected[i] == true)
            {
                playerCardsNumber[i] = Random.Range(0, playerCardType);
            }
        }
        UpdatePlayerCardTypeLevel();
        ChangePlayerCardSprite(true);
    }

    /// <summary>
    /// 値と画像を一致させる
    /// 引数のisShuffleは、シャッフルボタンを押したときか最初の初期化のどちらか
    /// カードが下にさがる現象をとめるため
    /// </summary>
    private void ChangePlayerCardSprite(bool isShuffle)
    {
        for (int i = 0; i < numberOfCard; i++)
        {
            if (isPlayerCardSelected[i] == true)
            {
                try
                {
                    GameObject playerCardImageObject = playerCardObjects[i].transform.Find("PlayerCardImage").gameObject;
                    Image image = playerCardImageObject.GetComponent<Image>();
                    image.sprite = playerCardSpriteList[playerCardsNumber[i]];
                    if (isShuffle)
                    {
                        Vector3 position = playerCardObjects[i].transform.position;
                        position.y -= 100.0f;
                        playerCardObjects[i].transform.position = position;
                    }
                    isPlayerCardSelected[i] = false;
                }
                catch (System.NullReferenceException e)
                {
                    Debug.LogError("参照エラーだが無視していく");
                }

            }
        }
    }

    /// <summary>
    /// カードをクリックして，シャッフル用に選択（プレイヤー）
    /// </summary>
    /// <param name="cardNumber"></param>
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

    /// <summary>
    /// ペア=>1 , 3カード=>2
    /// </summary>
    void UpdatePlayerCardTypeLevel()
    {
        int[] playerCardTypeEachSum = new int[playerCardType];
        for (int i = 0; i < numberOfCard; i++)
        {
            playerCardTypeEachSum[playerCardsNumber[i]]++;
        }

        for (int i = 0; i < playerCardType; i++)
        {
            if (playerCardTypeEachSum[i] >= 2)
            {
                playerCardTypeLevel[i] = playerCardTypeEachSum[i] - 1;
            }
            else
            {
                playerCardTypeLevel[i] = 0;
            }
        }
    }

    public void ShuffleEnemyCards()
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
        UpdateEnemyCardTypeLevel();
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

    void UpdateEnemyCardTypeLevel()
    {
        int[] enemyCardTypeEachSum = new int[enemyCardType];
        for (int i = 0; i < numberOfCard; i++)
        {
            enemyCardTypeEachSum[enemyCardsNumber[i]]++;
        }

        enemyNumber = 0;
        for (int i = 0; i < enemyCardType; i++)
        {
            if (enemyCardTypeEachSum[i] >= 2)
            {
                enemyCardTypeLevel[i] = enemyCardTypeEachSum[i] - 1;
                enemyNumber += enemyCardTypeLevel[i];
            }
            else
            {
                enemyCardTypeLevel[i] = 0;
            }
        }
    }
}