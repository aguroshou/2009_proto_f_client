using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingCell : MonoBehaviour
{
    public Text targetText;


    public void SetText(int rank, string name, int score)
    {
        targetText.text = rank + "い　" + name + "　スコア:" + score;
    }

}
