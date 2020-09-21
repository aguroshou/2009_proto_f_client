using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaltiController : MonoBehaviour
{
    [SerializeField] PopButton inGameButton;
    [SerializeField] NetworkSample ns;

    void Start()
    {
        inGameButton.Init(onSoloPlayButtonDown);
    }

    private void onSoloPlayButtonDown()
    {
        SceneManager.LoadSceneAsync("IngameScene");
        //StartCoroutine(ns.GameFinish(1000));
    }
}