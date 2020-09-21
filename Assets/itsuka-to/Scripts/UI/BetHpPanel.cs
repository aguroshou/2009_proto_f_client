using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetHpPanel : MonoBehaviour
{
    public float betRate = 1.0f;

    private Text hpText;
    private Text chipText;

    private void OnEnable()
    {
        hpText = transform.Find("HpText").GetComponent<Text>();
        chipText = transform.Find("BetText").GetComponent<Text>();
    }

    public void SetBetRate(float b)
    {
        float crossRate = GameManager.Instance.SetBetHpRate(b);
        betRate = b;
        hpText.text = "体力 : " + (int)(100 * b) + "%";
        chipText.text = "貰えるチップ : x" + crossRate.ToString("F2");
    }
}
