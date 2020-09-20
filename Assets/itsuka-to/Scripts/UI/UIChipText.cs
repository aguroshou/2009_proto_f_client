using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class UIChipText : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        GameManager.Instance.Chip.Subscribe((chip)=>
        {
            text.text = chip.ToString();
       });
    }
}
