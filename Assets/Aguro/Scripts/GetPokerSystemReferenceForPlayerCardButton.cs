using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPokerSystemReferenceForPlayerCardButton : MonoBehaviour
{
    [SerializeField] int playerCardNumber;
    GameObject pokerSystemObject;

    void Start()
    {
        pokerSystemObject = GameObject.Find("PokerSystemGameObject");
        Button button = GetComponent<Button>();
        PokerSystem pokerSystem =  pokerSystemObject.GetComponent<PokerSystem>();
        button.onClick.AddListener(() => pokerSystem.PlayerCardClicked(playerCardNumber));
    }
}
