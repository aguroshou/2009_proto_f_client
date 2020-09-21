using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

public class UIShootingReadyText : MonoBehaviour
{
    private Text text;

    //[SerializeField]
    //private float hidePosY = 100f;  // 隠れている時のY座標

    //[SerializeField]
    //private float displayPosY = 0f;  // タイムを表示している時のY座標

    private RectTransform rectTran;

    // Start is called before the first frame update
    void Start()
    {
        //rectTran = GetComponent<RectTransform>();
        //text = transform.GetChild(0).GetComponent<Text>();
        text = GetComponent<Text>();

        //GameManager.Instance.Phase.Subscribe((phase) =>
        //{
        //    if (phase == GameManager.EGamePhase.SHOOTING_PHASE)
        //    {
        //        rectTran.DOAnchorPosY(displayPosY, 1.0f);
        //    }
        //    else
        //    {
        //        rectTran.DOAnchorPosY(hidePosY, 1.0f);
        //    }
        //});

        GameManager.Instance.ShootingReadyTime.Subscribe((sec) => {
            text.text = sec.ToString("F1");
        });
    }
}
