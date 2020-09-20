using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPokerSystemReferenceForShuffleCardButton : MonoBehaviour
{
    GameObject pokerSystemGameObject;

    void Start()
    {
        pokerSystemGameObject = GameObject.Find("PokerSystemGameObject");
        Button button = GetComponent<Button>();
        PokerSystem pokerSystem = pokerSystemGameObject.GetComponent<PokerSystem>();
        button.onClick.AddListener(pokerSystem.ShuffleCards);
    }
}
