using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class UIPokerPhasePanel : MonoBehaviour
{

    void Start()
    {
        var childDisplayPanel = transform.Find("PokerPhaseDisplayPanel").gameObject;
        GameManager.Instance.Phase.Subscribe((phase) => {
            if(phase == GameManager.EGamePhase.POKER_PHASE)
            {
                childDisplayPanel.SetActive(true);
            }
            else
            {
                childDisplayPanel.SetActive(false);
            }
        });
    }
}
