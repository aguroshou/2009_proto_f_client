using System.Collections;
using ProjectConnect.Network;
using ProjectConnect.Network.RequestDto;
using ProjectConnect.Network.ResponseDto;
using UnityEngine;

public class NetworkSample : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(NetworkCoroutine());
    }

    /// <summary>
    /// 通信サンプル用のCoroutine
    /// </summary>
    private IEnumerator NetworkCoroutine()
    {
        var userCreateRequest = new UserCreateRequest();
        userCreateRequest.name= "ユーザー";
        Debug.Log(userCreateRequest.ToJson());
        
        //// WebRequestクラスをインスタンス化
        var webRequest = new WebRequest();

        //// サーバーアドレスを設定する
        webRequest.SetServerAddress("http://54.150.161.227:8080");
        yield return webRequest.PushUserCreate(userCreateRequest, Tuusinseikou, Tuusinsipppai);

        //// ユーザー作成時の情報
        //var userCreateRequestDto = new UserCreateRequestDto()
        //{
        //    name = "テストユーザー"
        //};

        //// ユーザー作成リクエストを投げる
        //// 成功時: レスポンスのトークンをtoken変数に入れる
        //// 失敗時: エラーの内容をDebug.LogErrorで出力する
        //string token = null;
        //yield return webRequest.PushUserCreate(userCreateRequestDto,
        //    userCreateResponseDto => token = userCreateResponseDto.token, Debug.LogError);

        //// トークンを設定する
        //// 実装時はユーザ作成が成功したか確認した方が良い
        //webRequest.SetToken(token);

        //// ユーザー情報取得リクエストを投げる
        //// 成功時: name,id,coin,highScoreの情報を出力する
        //// 失敗時: エラーの内容をDebug.LogErrorで出力する
        //yield return webRequest.FetchUser(userGetResponseDto =>
        //{
        //    Debug.Log("name:" + userGetResponseDto.name);
        //    Debug.Log("id:" + userGetResponseDto.id);
        //    Debug.Log("coin:" + userGetResponseDto.coin);
        //    Debug.Log("highScore:" + userGetResponseDto.highScore);
        //}, Debug.LogError);

        //// ユーザー更新時の情報
        //var userUpdateRequestDto = new UserUpdateRequestDto()
        //{
        //    name = "名前変更後済み"
        //};

        //// ユーザー更新リクエストを投げる
        //// 成功時: 「ユーザー更新完了」と出力する
        //// 失敗時: エラーの内容をDebug.LogErrorで出力する
        //yield return webRequest.PushUserUpdate(userUpdateRequestDto,
        //    () => Debug.Log("ユーザー更新完了"), Debug.LogError);

        //// ユーザー情報取得リクエストを投げる
        //// 名前が更新されてることを確認する
        //// 実装時はコピペせずにメソッドにまとめた方が良い
        /// 匿名関数
        //yield return webRequest.FetchUser(userGetResponseDto =>
        //{
        //    Debug.Log("name:" + userGetResponseDto.name);
        //    Debug.Log("id:" + userGetResponseDto.id);
        //    Debug.Log("coin:" + userGetResponseDto.coin);
        //    Debug.Log("highScore:" + userGetResponseDto.highScore);
        //}, Debug.LogError);

        //yield return webRequest.FetchUser(TuusinseikouFetchUser, Debug.LogError);

    }

    //private void TuusinseikouFetchUser(UserGetResponseDto userGetResponseDto)
    //{
    //    Debug.Log("name:" + userGetResponseDto.name);
    //    Debug.Log("id:" + userGetResponseDto.id);
    //    Debug.Log("coin:" + userGetResponseDto.coin);
    //    Debug.Log("highScore:" + userGetResponseDto.highScore);
    //}

    private void Tuusinseikou(UserCreateResponse userCreateResponse)
    {
        Debug.Log("成功");
        Debug.Log(userCreateResponse.token);
    }

    private void Tuusinsipppai(string errormasage)
    {
        Debug.Log("失敗");
        Debug.Log(errormasage);
    }
}
