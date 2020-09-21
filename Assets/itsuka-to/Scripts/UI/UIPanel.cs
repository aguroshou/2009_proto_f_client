using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// displayPhase変数に設定したフェーズで子にある単一のpanelを
/// setActiveする
/// </summary>
public class UIPanel : MonoBehaviour
{
    public GameManager.EGamePhase displayPhase;

    void Start()
    {
        var childDisplayPanel = transform.GetChild(0).gameObject;
        GameManager.Instance.Phase.Subscribe((phase) =>
        {
            if (phase == displayPhase)
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
