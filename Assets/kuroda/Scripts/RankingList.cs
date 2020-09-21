using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RankingList : MonoBehaviour
{
    public GameObject listPrefab;
    public Transform targetTransform;

  

    public void Start()
    {
        var ns = FindObjectOfType<NetworkSample>();
        StartCoroutine(ns.GameFinish(11111, OnSuccessGameFinish));
    }

    private void OnSuccessGameFinish(RankingListResponse rankingListResponse)
    {

        int count;
        for (count = 0; count < rankingListResponse.ranks.Count; count += 1)
        {
            var rank = rankingListResponse.ranks[count];
                
            GameObject obj = Instantiate(listPrefab, targetTransform);
            RankingCell rankingCell = obj.GetComponent<RankingCell>();
            rankingCell.SetText(rank.rank,rank.userName, rank.score);
        }

    }
}
