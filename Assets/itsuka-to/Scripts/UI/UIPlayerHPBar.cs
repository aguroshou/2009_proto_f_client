﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerHPBar : MonoBehaviour
{
    [SerializeField]
    private float hidePosX = 100f;  // 隠れている時のY座標

    [SerializeField]
    private float displayPosX = 0f;  // タイムを表示している時のY座標

    private RectTransform rectTran;

    // Start is called before the first frame update
    void Start()
    {
        rectTran = GetComponent<RectTransform>();

        GameManager.Instance.Phase.Subscribe((phase) =>
        {
            if (phase == GameManager.EGamePhase.SHOOTING_PHASE)
            {
                rectTran.DOAnchorPosX(displayPosX, 1.0f);
            }
            else
            {
                rectTran.DOAnchorPosX(hidePosX, 1.0f);
            }
        });

        // マウスホバーしたら半透明にする
        var image = GetComponent<Image>();
        var bar_image = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        var trigger = gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => {
            image.color = new Color(1f, 1f, 1f, 0.5f);  // 半透明
            bar_image.color = new Color(1f, 1f, 1f, 0.5f);  // 半透明
        });
        trigger.triggers.Add(entry);

        var entry_pointer_exit = new EventTrigger.Entry();
        entry_pointer_exit.eventID = EventTriggerType.PointerExit;
        entry_pointer_exit.callback.AddListener((data) =>
        {
            image.color = new Color(1f, 1f, 1f, 1f);  // 非透明（通常）
            bar_image.color = new Color(1f, 1f, 1f, 1f);  // 非透明（通常）
        });
        trigger.triggers.Add(entry_pointer_exit);
    }

    
}
