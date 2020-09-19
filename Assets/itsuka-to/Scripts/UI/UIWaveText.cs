using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIWaveText : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();

        GameManager.Instance.Wave.Subscribe((wave) => {
            text.text = "Wave : " + wave.ToString();
        });
    }
}
