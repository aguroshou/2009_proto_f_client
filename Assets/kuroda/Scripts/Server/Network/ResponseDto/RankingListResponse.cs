using System;
using System.Collections.Generic;

[Serializable]
public class RankingListResponse : DtoBase
{
    public List<RankingRank> ranks;
    public List<RankingMyRank> myranks;
}

[Serializable]
public class RankingRank
{
    public string userId;
    public string userName;
    public int rank;
    public int score;

}

[Serializable]
public class RankingMyRank
{
    public string userId;
    public string userName;
    public int rank;
    public int score;
    public bool isme;
}