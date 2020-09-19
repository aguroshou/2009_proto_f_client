using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] PopButton button;

    void Start()
    {
        button.Init(onButtonDown);
    }

    private void onButtonDown()
    {
        SceneManager.LoadSceneAsync("SelectScene");
    }
}
