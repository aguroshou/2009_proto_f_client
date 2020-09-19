using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIStatusPowerupPhaseEndButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        GameManager.Instance.Phase.Subscribe((phase) => {
            if (phase == GameManager.EGamePhase.STATUS_POWERUP_PHASE)
                button.interactable = true;
            else
                button.interactable = false;
        });
    }
}
