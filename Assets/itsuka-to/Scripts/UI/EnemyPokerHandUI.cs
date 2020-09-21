using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class EnemyPokerHandUI : MonoBehaviour
{
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();

        GameManager.Instance.enemyPokerHand.Subscribe((hand) =>
        {
            text.text = "相手の役 : " + GameManager.EPokerHandToString(hand);
        });
    }
}
