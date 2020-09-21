using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerPokerHandUI : MonoBehaviour
{
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();

        GameManager.Instance.playerPokerHand.Subscribe((hand) =>
        {
            text.text = "自分の役 : " + GameManager.EPokerHandToString(hand);
        });
    }
}
