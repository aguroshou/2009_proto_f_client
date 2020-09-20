using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPokerSystemReferenceForPlayerCardButton : MonoBehaviour
{
    [SerializeField] int playerCardNumber;
    GameObject pokerSystemGameObject;
    // Start is called before the first frame update
    void Start()
    {
        pokerSystemGameObject = GameObject.Find("PokerSystemGameObject");
        Button button = GetComponent<Button>();
        PokerSystem pokerSystem =  pokerSystemGameObject.GetComponent<PokerSystem>();
        button.onClick.AddListener(() => pokerSystem.PlayerCardClicked(playerCardNumber));
    }
}
