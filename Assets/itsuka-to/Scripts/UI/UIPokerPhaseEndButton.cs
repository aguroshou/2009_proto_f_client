using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIPokerPhaseEndButton : MonoBehaviour
{
    // Start is called before the first frame update

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        GameManager.Instance.Phase.Subscribe((phase) => {
            if (phase == GameManager.EGamePhase.POKER_PHASE)
                button.interactable = true;
            else
                button.interactable = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
