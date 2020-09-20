using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class UIStatusPowerupPhasePanel : MonoBehaviour
{
    void Start()
    {
        var childDisplayPanel = transform.Find("StatusPowerupPhaseDisplayPanel").gameObject;
        GameManager.Instance.Phase.Subscribe((phase) => {
            if (phase == GameManager.EGamePhase.STATUS_POWERUP_PHASE)
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
