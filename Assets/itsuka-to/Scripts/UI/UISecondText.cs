using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UISecondText : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

        GameManager.Instance.ShootingTime.Subscribe((sec) => {
            text.text = "sec: " + sec.ToString("F2");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
