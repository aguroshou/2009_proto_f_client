using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    [SerializeField] PopButton inGameButton;
    [SerializeField] PopButton MultiButton;

    void Start()
    {
        inGameButton.Init(onSoloPlayButtonDown);
        MultiButton.Init(onMultiPlayButtonDown);
    }

    private void onSoloPlayButtonDown()
    {
        SceneManager.LoadSceneAsync("IngameScene");
    }

    private void onMultiPlayButtonDown()
    {
        SceneManager.LoadSceneAsync("MatchingScene");
    }
}
