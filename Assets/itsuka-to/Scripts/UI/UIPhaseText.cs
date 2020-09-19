using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIPhaseText : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();

        GameManager.Instance.Phase.Subscribe((phase) => {
            text.text = "Phase : " + phase.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
