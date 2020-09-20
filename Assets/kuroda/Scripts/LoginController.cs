using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [SerializeField] PopButton button;
    [SerializeField] NameDirector nd;

    void Start()
    {
        button.Init(onButtonDown);
    }

    private void onButtonDown()
    {
        nd.InputText();
        //SceneManager.LoadSceneAsync("SelectScene");
    }
}
