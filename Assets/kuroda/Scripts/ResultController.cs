﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultController : MonoBehaviour
{
    [SerializeField] PopButton inGameButton;

    void Start()
    {
        inGameButton.Init(onSoloPlayButtonDown);
    }

    private void onSoloPlayButtonDown()
    {
        SceneManager.LoadSceneAsync("SelectScene");
    }
}
