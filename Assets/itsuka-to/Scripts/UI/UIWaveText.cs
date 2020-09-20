using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class UIWaveText : MonoBehaviour
{
    private Text text;
    private RectTransform rectTran;
    [SerializeField]
    private float displayHeight = 200f;  // 表示時の幅
    private Image image;

    private void Start()
    {
        rectTran = GetComponent<RectTransform>();
        text = transform.Find("UIWaveTextChild").GetComponent<Text>();
        image = GetComponent<Image>();

        GameManager.Instance.Phase.Subscribe((phase) =>
        {
            if(phase == GameManager.EGamePhase.SHOOTING_PHASE)
            {
                // ボス戦開始時なら
                if (GameManager.Instance.IsBoss.Value)
                {
                    TextChange("Boss Battle");
                }
                else
                {
                    TextChange("Wave : " + GameManager.Instance.Wave.Value.ToString());
                }
            }


        });
    }

    private void TextChange(string t)
    {
        text.text = t;
        var sizeDelta = rectTran.sizeDelta;
        sizeDelta.y = displayHeight;

        image.color = new Color(0f, 0f, 0f, 1f);
        rectTran.DOSizeDelta(sizeDelta, 1.0f).SetLoops(2, LoopType.Yoyo)
            .OnComplete(()=> {
                image.color = new Color(0f, 0f, 0f, 0f);
            });
    }
}
