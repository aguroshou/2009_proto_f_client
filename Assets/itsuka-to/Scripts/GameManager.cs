using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private int maxPhase = 10;  // 最大のウェーブ数 

    /// <summary>
    /// Wave : 現在のウェーブ数
    /// Wave.Valueで値が読める
    /// </summary>
    public ReactiveProperty<int> Wave = new ReactiveProperty<int>(1);

    /// <summary>
    /// GamePhase : 現在のフェーズ定義
    /// </summary>
    public enum EGamePhase
    {
        POKER_PHASE,
        SHOOTING_PHASE,
        STATUS_POWERUP_PHASE,
        RESULT_PHASE,
    }

    /// <summary>
    /// 
    /// </summary>
    public ReactiveProperty<EGamePhase> Phase = new ReactiveProperty<EGamePhase>(EGamePhase.POKER_PHASE);

    public void ChangePhase()
    {
        if (Phase.Value == EGamePhase.POKER_PHASE)
        {
            Phase.Value = EGamePhase.SHOOTING_PHASE;
        }
        else if (Phase.Value == EGamePhase.SHOOTING_PHASE)
        {
            Phase.Value = EGamePhase.STATUS_POWERUP_PHASE;
        }
        else if (Phase.Value == EGamePhase.STATUS_POWERUP_PHASE)
        {
            if(Wave.Value < maxPhase)
            {
                Phase.Value = EGamePhase.POKER_PHASE;
                Wave.Value += 1;
            }
            else
            {
                Phase.Value = EGamePhase.RESULT_PHASE;
            }
        }
    }
}
