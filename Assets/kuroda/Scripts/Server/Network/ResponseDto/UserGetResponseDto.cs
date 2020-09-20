using System;

namespace ProjectConnect.Network.ResponseDto
{
    [Serializable]
    public class UserGetResponseDto : DtoBase
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <value>ID</value>
        public string id;
        
        /// <summary>
        /// ユーザ名
        /// </summary>
        /// <value>ユーザ名</value>
        public string name;

        /// <summary>
        /// ハイスコア
        /// </summary>
        /// <value>ハイスコア</value>
        public int highScore;

        /// <summary>
        /// 所持コイン
        /// </summary>
        /// <value>所持コイン</value>
        public int coin;
    }
}
