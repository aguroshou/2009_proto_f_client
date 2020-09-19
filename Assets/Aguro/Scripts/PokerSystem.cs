using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStageAndBackground : MonoBehaviour
{

    [SerializeField] List<GameObject> enemyCards = new List<GameObject>();


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

    [SerializeField] List<GameObject> enemyCards = new List<GameObject>();
    float generateStagePointX;
    //一番右にあるステージオブジェクトを保存するため
    //Inspector上で右端のステージオブジェクトを代入する必要があります。
    [SerializeField] GameObject rightSideStagePatternObject;

    [SerializeField] GameObject backgroundObject;
    [SerializeField] float generateBackgroundPointX;
    //一番右にある背景オブジェクトを保存するため
    //Inspector上で右端の背景オブジェクトを代入する必要があります。
    [SerializeField] GameObject rightSideBackgroundObject;


    //ステージが時間につれて速くなっていく加速度
    [SerializeField] float stageSpeedAcceleration;

    //スピードはマイナスの値なので、最大マイナス値を制限
    [SerializeField] float stageSpeedMax;

    [SerializeField] float initialStageSpeed;

    [SerializeField] bool isGameOver;

    //マイナスの値で左に移動
    //MoveStagePattern.csとMoveBackgroundでステージ全体の移動速度を利用しています
    public float stagePatternMoveSpeed;

    //小人と巨人の速度変更
    public float speedRate;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        stagePatternMoveSpeed = initialStageSpeed;
        generateStagePointX = 19.0f;
        generateBackgroundPointX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver == true)
        {
            return;
        }
        if (stagePatternMoveSpeed >= stageSpeedMax)
        {
            stagePatternMoveSpeed += stageSpeedAcceleration;
        }

        //ステージパターンの繋ぎ目の隙間防止
        float offset = stagePatternMoveSpeed * speedRate;

        if (rightSideStagePatternObject.transform.position.x < generateStagePointX - offset)
        {
            rightSideStagePatternObject = Instantiate(stagePatternObjectList[Random.Range(0, stagePatternObjectList.Count)], new Vector3(28.9f, -5.0f, 0.0f), Quaternion.identity);
        }

        if (rightSideBackgroundObject.transform.position.x < generateBackgroundPointX - offset / 4f)
        {
            rightSideBackgroundObject = Instantiate(backgroundObject, new Vector3(33.0f, 0.0f, 0.0f), Quaternion.identity);
        }
    }

    public void GameOver()
    {
        stagePatternMoveSpeed = 0.0f;
        isGameOver = true;
    }

    public void SetSpeedRate(float rate)
    {
        speedRate = rate;
    }
}