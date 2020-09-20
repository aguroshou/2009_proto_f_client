using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectConnect.Network.RequestDto;
using ProjectConnect.Network.ResponseDto;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace ProjectConnect.Network
{
    /// <summary>
    /// 通信するための最低限の処理を行うクラス
    /// このクラスをコピペすれば通信処理が出来上がる
    /// </summary>
    public class WebRequest
    {
        public const string DEFAULT_SERVER_ADDRESS = "http://localhost:8080";
        
        private string _serverAddress;
        private string _token;
        private const int Timeout = 5;


        public void SetServerAddress(string address)
        {
            _serverAddress = address;
        }

        public void SetToken(string token)
        {
            _token = token;
        }

        /// <summary>
        /// User情報を受け取る処理
        /// </summary>
        /// <param name="onSuccess">成功時のコールバック。サーバーからのレスポンスを持つ</param>
        /// <param name="onError">失敗時のコールバック。サーバーからのエラーメッセージを持つ</param>
        /// <returns></returns>
        public IEnumerator FetchUser(Action<UserGetResponseDto> onSuccess, Action<string> onError = null)
        {
            yield return GetRequest("/user/get", onSuccess, onError);
        }

        public IEnumerator PushUserCreate(UserCreateRequest requestDto, Action<UserCreateResponse> onSuccess,
            Action<string> onError = null)
        {
            yield return PostRequest("/user/create", requestDto, onSuccess, onError, false);
        }

        public IEnumerator PushGameFinish(ScoreRequest requestDto, Action<RankingListResponse> onSuccess,
            Action<string> onError = null)
        {
            yield return PostRequest("/game/finish", requestDto, onSuccess, onError, true);
        }

        public IEnumerator FetchRanking( Action<RankingListResponse> onSuccess,
            Action<string> onError = null)
        {
            yield return GetRequest("/ranking/list", onSuccess, onError, true);
        }

        /// <summary>
        /// ユーザー作成リクエストを行う処理
        /// </summary>
        /// <param name="requestDto">リクエストに必要な内容</param>
        /// <param name="onSuccess">成功時のコールバック。サーバーからのレスポンスを持つ</param>
        /// <param name="onError">失敗時のコールバック</param>
        /// <returns></returns>
        //public IEnumerator PushUserCreate(UserCreateRequestDto requestDto, Action<UserCreateResponseDto> onSuccess,
        //    Action<string> onError = null)
        //{
        //    yield return PostRequest("/user/create", requestDto, onSuccess, onError, false);
        //}

        /// <summary>
        /// ユーザー更新リクエストを行う処理
        /// </summary>
        /// <param name="requestDto">リクエストに必要な内容</param>
        /// <param name="onSuccess">成功時のコールバック。サーバーからのレスポンスを持つ</param>
        /// <param name="onError">失敗時のコールバック</param>
        /// <returns></returns>
        public IEnumerator PushUserUpdate(UserUpdateRequestDto requestDto, Action onSuccess, Action<string> onError = null)
        {
            yield return PostRequest("/user/update", requestDto, onSuccess, onError);
        }

        /// <summary>
        /// Postリクエストを行いレスポンスを型に変換する
        /// </summary>
        /// <param name="method">リクエスト先URL</param>
        /// <param name="request">リクエストDto</param>
        /// <param name="onSuccess">通信成功時のレスポンスDto</param>
        /// <param name="useToken">UserTokenを使用するか</param>
        /// <typeparam name="TResponse">レスポンスDtoの形</typeparam>
        /// <typeparam name="TRequest">リクエストDtoの形</typeparam>
        /// <returns></returns>
        private IEnumerator PostRequest<TResponse, TRequest>(string method, TRequest request,
            Action<TResponse> onSuccess, Action<string> onError,
            bool useToken = true)
            where TResponse : DtoBase
            where TRequest : DtoBase
        {
            yield return PostRequestImpl(
                CreateUrl(method),
                CreateCommonHeader(false, useToken),
                request.ToJson(),
                text =>
                {
                    // レスポンスのJsonをDtoクラスに変換する
                    var responseFromJson = JsonUtility.FromJson<TResponse>(text);
                    Assert.IsNotNull(responseFromJson);
                    onSuccess?.Invoke(responseFromJson);
                }, onError);
        }

        /// <summary>
        /// Postリクエストを行う(レスポンスがないバージョン)
        /// </summary>
        /// <param name="method">リクエスト先URL</param>
        /// <param name="request">リクエストDto</param>
        /// <param name="onSuccess">通信成功時のコールバック</param>
        /// <param name="useToken">UserTokenを使用するか</param>
        /// <typeparam name="TRequest">リクエストDtoの型</typeparam>
        /// <returns></returns>
        private IEnumerator PostRequest<TRequest>(string method, TRequest request, Action onSuccess, Action<string> onError,
            bool useToken = true)
            where TRequest : DtoBase

        {
            yield return PostRequestImpl(
                CreateUrl(method),
                CreateCommonHeader(false, useToken),
                request.ToJson(),
                text => { onSuccess?.Invoke(); }, onError);
        }

        /// <summary>
        /// Getリクエストを行う
        /// </summary>
        /// <param name="method">リクエスト先URL</param>
        /// <param name="response">レスポンスDto</param>
        /// <param name="useToken">UserTokenを使用するか</param>
        /// <param name="additionalHeader">追加で使用するHeader</param>
        /// <typeparam name="TResponse">レスポンスDtoの型</typeparam>
        /// <returns></returns>
        private IEnumerator GetRequest<TResponse>(string method, Action<TResponse> response, Action<string> onError,
            bool useToken = true,
            KeyValuePair<string, string>[] additionalHeader = default)
            where TResponse : DtoBase
        {
            // ヘッダを作成
            var headers = CreateCommonHeader(true, useToken);

            // 追加のヘッダがある場合、くっつける
            if (additionalHeader != default)
            {
                foreach (var keyValuePair in additionalHeader)
                {
                    headers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            yield return GetRequestImpl(
                CreateUrl(method),
                headers.ToArray(),
                text =>
                {
                    // 通信成功時の処理
                    var responseFromJson = JsonUtility.FromJson<TResponse>(text);
                    Assert.IsNotNull(responseFromJson);
                    response?.Invoke(responseFromJson);
                }, onError);
        }

        /// <summary>
        /// Getリクエスト処理本体
        /// </summary>
        /// <param name="url">リクエスト先URL</param>
        /// <param name="headers">リクエストヘッダ</param>
        /// <param name="OnRequestSuccess">リクエスト成功時のレスポンステキスト(Json)</param>
        /// <returns></returns>
        private IEnumerator GetRequestImpl(string url, KeyValuePair<string, string>[] headers,
            Action<string> OnRequestSuccess, Action<string> onError)
        {
            // UnityWebRequestクラスの生成
            UnityWebRequest webRequest = UnityWebRequest.Get(url);

            // タイムアウトの指定
            webRequest.timeout = Timeout;

            // ヘッダの設定
            foreach (var keyValuePair in headers)
            {
                webRequest.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
            }

            // リクエスト送信
            yield return webRequest.SendWebRequest();

            // エラー処理
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError("通信エラー:" + webRequest.error);
                onError?.Invoke(webRequest.error);
            }
            else
            {
                OnRequestSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }

        /// <summary>
        /// Postリクエスト処理本体
        /// </summary>
        /// <param name="url">リクエスト先URL</param>
        /// <param name="headers">リクエストヘッダ</param>
        /// <param name="postJson">ポストするJsonテキスト</param>
        /// <param name="OnRequestSuccess">リクエスト成功時のレスポンステキスト(Json)</param>
        /// <returns></returns>
        private IEnumerator PostRequestImpl(string url, Dictionary<string, string> headers, string postJson,
            Action<string> OnRequestSuccess, Action<string> onError)
        {
            // JsonをByteに変換する
            byte[] postByte = System.Text.Encoding.UTF8.GetBytes(postJson);
            var webRequest = new UnityWebRequest(url, "POST");

            // タイムアウトの指定
            webRequest.timeout = Timeout;

            // Byteに変換したJsonデータをアップロードする
            webRequest.uploadHandler = new UploadHandlerRaw(postByte);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // ヘッダの設定
            foreach (var keyValuePair in headers)
            {
                webRequest.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
            }

            // リクエスト送信
            yield return webRequest.SendWebRequest();

            // エラー処理
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError("通信エラー:" + webRequest.error);
                onError?.Invoke(webRequest.error);
            }
            else
            {
                OnRequestSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }

        /// <summary>
        /// Tokenヘッダを作成する
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateTokenHeader()
        {
            return new Dictionary<string, string>(1)
            {
                {"x-token", _token}
            };
        }

        /// <summary>
        /// Acceptヘッダを作成する
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateAcceptHeader()
        {
            return new Dictionary<string, string>(1)
            {
                {"accept", "application/json"}
            };
        }

        /// <summary>
        /// Content-Typeヘッダを作成する
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> CreateContentTypeHeader()
        {
            return new Dictionary<string, string>(1)
            {
                {"Content-Type", "application/json"}
            };
        }

        /// <summary>
        /// TokenとAcceptヘッダを作成する
        /// </summary>
        /// <param name="isGetRequest">Getリクエストか</param>
        /// <param name="useToken">トークンヘッダを追加するか</param>
        /// <returns></returns>
        private Dictionary<string, string> CreateCommonHeader(bool isGetRequest, bool useToken)
        {
            var header = new Dictionary<string, string>();

            if (useToken)
            {
                var tokenHeader = CreateTokenHeader().First();
                header.Add(tokenHeader.Key, tokenHeader.Value);
            }

            if (isGetRequest)
            {
                var acceptHeader = CreateAcceptHeader().First();
                header.Add(acceptHeader.Key, acceptHeader.Value);
            }
            else
            {
                var contentTypeHeader = CreateContentTypeHeader().First();
                header.Add(contentTypeHeader.Key, contentTypeHeader.Value);
            }

            return header;
        }

        /// <summary>
        /// URLを作成する
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private string CreateUrl(string method)
        {
            Assert.IsNotNull(_serverAddress, "サーバーアドレスが設定されていません");
            Assert.IsNotNull(method, "リクエストメソッドが空です");

            // もしアドレスが設定されていなかったらlocalhostを使用
            if (String.IsNullOrEmpty(_serverAddress))
            {
                _serverAddress = DEFAULT_SERVER_ADDRESS;
            }
            
            return $"{_serverAddress}{method}";
        }
    }
}
