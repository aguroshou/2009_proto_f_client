using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameDirector : MonoBehaviour
{
    [SerializeField] NetworkSample ns;

    public Text text;

    public void InputText()
    {
        Debug.Log(text.text);
        StartCoroutine(ns.UserCriate(text.text));
    }
}
