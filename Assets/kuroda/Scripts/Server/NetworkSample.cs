using System;
using System.Collections;
using ProjectConnect.Network;
using ProjectConnect.Network.RequestDto;
using ProjectConnect.Network.ResponseDto;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSample : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Start");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Start1()
    {
        var resouces = Resources.Load("NetworkManager");
        var obj = Instantiate(resouces);
        DontDestroyOnLoad(obj);
    }

    private void Start2()
    {
        Debug.Log("Start2");
    }


    /// <summary>
    /// 通信サンプル用のCoroutine
    /// </summary>
    public IEnumerator UserCriate(string text)
    {
        var userCreateRequest = new UserCreateRequest();
        userCreateRequest.name= text;
        Debug.Log(userCreateRequest.ToJson());
        
        //// WebRequestクラスをインスタンス化
        var webRequest = new WebRequest();

        //// サーバーアドレスを設定する
        webRequest.SetServerAddress("http://54.150.161.227:8080");
        yield return webRequest.PushUserCreate(userCreateRequest, OnSuccessUserCreate, OnErrorUserCreate);
    }

    public IEnumerator GameFinish(int score, Action<RankingListResponse> onSuccess)
    {
        var webRequest = new WebRequest();
        webRequest.SetServerAddress("http://54.150.161.227:8080");
        var scoreRequest = new ScoreRequest();
        scoreRequest.score = score;
        var token = Playerprefs.GetString(Playerprefs.PlayerKeys.TOKEN);
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("なんもいてないよ");
            yield return null;

        }
        webRequest.SetToken(token);
        yield return webRequest.PushGameFinish(scoreRequest, onSuccess, OnErrorGameFinish);

    }

    public IEnumerator GetRanking(Action<RankingListResponse> onSuccess)
    {
        var webRequest = new WebRequest();
        webRequest.SetServerAddress("http://54.150.161.227:8080");
        var token = Playerprefs.GetString(Playerprefs.PlayerKeys.TOKEN);
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("なんもいてないよ");
            yield return null;

        }
        webRequest.SetToken(token);
        yield return webRequest.FetchRanking( onSuccess , OnErrorFetchRanking);

    }

    //private void TuusinseikouFetchUser(UserGetResponseDto userGetResponseDto)
    //{
    //    Debug.Log("name:" + userGetResponseDto.name);
    //    Debug.Log("id:" + userGetResponseDto.id);
    //    Debug.Log("coin:" + userGetResponseDto.coin);
    //    Debug.Log("highScore:" + userGetResponseDto.highScore);
    //}

    private void OnSuccessUserCreate(UserCreateResponse userCreateResponse)
    {
        Debug.Log("成功");
        Debug.Log(userCreateResponse.token);
        Playerprefs.SetString(Playerprefs.PlayerKeys.TOKEN, userCreateResponse.token);
        SceneManager.LoadSceneAsync("SelectScene");
    }

    private void OnErrorUserCreate(string errormasage)
    {
        Debug.Log("失敗");
        Debug.Log(errormasage);
    }

    private void OnSuccessGameFinish(RankingListResponse rankingListResponse)
    {
        Debug.Log("成功");
    }

    private void OnErrorGameFinish(string errormasage)
    {
        Debug.Log(errormasage);
    }

    private void OnSuccessFetchRanking(RankingListResponse rankingListResponse)
    {
        for(int i = 0; i<  rankingListResponse.ranks.Count; i++)
        {
            var rank = rankingListResponse.ranks[i];


            Debug.Log("ランクは" + rank.rank);
            Debug.Log("名前は" + rank.userName);
            Debug.Log("スコアは" + rank.score);
            Debug.Log("~~~~~~~~~~~~~~~~~~~~~");

             

        }
    }

    private void OnErrorFetchRanking(string errormasage)
    {
        Debug.Log(errormasage);
    }
}
