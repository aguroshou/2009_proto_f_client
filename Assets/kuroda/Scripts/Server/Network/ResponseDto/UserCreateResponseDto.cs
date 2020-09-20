using System;

namespace ProjectConnect.Network.ResponseDto
{
    [Serializable]
    public class UserCreateResponseDto : DtoBase
    {
        /// <summary>
        /// クライアント側で保存するトークン
        /// </summary>
        /// <value>クライアント側で保存するトークン</value>
        public string token;
    }
}
