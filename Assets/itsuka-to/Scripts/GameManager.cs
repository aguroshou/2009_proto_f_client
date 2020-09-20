using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private float shootingTimeLimit = 20f;

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
        POKER_PHASE,  // ポーカー
        SHOOTING_PHASE,  // シューティング
        STATUS_POWERUP_PHASE,  // ステータスあげる
        RESULT_PHASE,  // 結果画面
        GAMEOVER,  // ゲームオーバー
    }


    /// <summary>
    /// ボス戦かどうか
    /// </summary>
    public ReactiveProperty<bool> IsBoss = new ReactiveProperty<bool>(false);

    /// <summary>
    /// 
    /// </summary>
    public ReactiveProperty<EGamePhase> Phase = new ReactiveProperty<EGamePhase>(EGamePhase.POKER_PHASE);

    /// <summary>
    /// ボス戦クリア以外の画面の遷移に移動する関数
    /// </summary>
    public void ChangePhase()
    {
        if (Phase.Value == EGamePhase.POKER_PHASE)
        {
            Phase.Value = EGamePhase.SHOOTING_PHASE;
        }
        else if (Phase.Value == EGamePhase.SHOOTING_PHASE)
        {
            if (IsBoss.Value)
            {
                // ボス戦の場合，ステータス強化画面にはいかず，ポーカーフェーズに向かう
                Phase.Value = EGamePhase.POKER_PHASE;
            }
            else
            {
                Phase.Value = EGamePhase.STATUS_POWERUP_PHASE;
            }
        }
        else if (Phase.Value == EGamePhase.STATUS_POWERUP_PHASE)
        {
            if (IsBoss.Value)  // ボス戦
            {
                //　ボス戦ではステータス強化画面は存在しない
                Debug.LogError("IsBoss Value Error");
            }
            else if(Wave.Value < maxPhase)
            {
                Phase.Value = EGamePhase.POKER_PHASE;
                Wave.Value += 1;
            }
            else
            {
                // ボス戦へ
                Phase.Value = EGamePhase.POKER_PHASE;
                IsBoss.Value = true;
            }
        }
    }

    /// <summary>
    /// ボス戦でクリアした時に呼ぶ関数
    /// </summary>
    public void BossClear()
    {
        // ボス戦かつシューティング面か？（バグ対策）
        if(IsBoss.Value && Phase.Value == EGamePhase.SHOOTING_PHASE)
        {
            Phase.Value = EGamePhase.RESULT_PHASE;
        }
        else
        {
            Debug.LogError("ボスじゃない or シューティングフェーズではないのでクリアできません");
        }
    }

    /// <summary>
    /// GameOver，phaseの特殊遷移
    /// </summary>
    public void GameOver()
    {
        if(Phase.Value == EGamePhase.SHOOTING_PHASE)
        {
            Phase.Value = EGamePhase.GAMEOVER;
        }
        else
        {
            Debug.LogError("シューティングフェーズではないのにゲームオーバーできません");
        }
    }

    public ReactiveProperty<float> ShootingTime = new ReactiveProperty<float>(0f);

    public ReactiveProperty<int> Chip = new ReactiveProperty<int>(0);

    private void Start()
    {
        Phase.Subscribe((phase) =>
        {
            if (phase == EGamePhase.SHOOTING_PHASE)
            {
                ShootingTime.Value = shootingTimeLimit;
            }
        });
    }

    private void Update()
    {
        if (Phase.Value == EGamePhase.SHOOTING_PHASE)
        {
            float time = ShootingTime.Value;
            time -= Time.deltaTime;
            if (time < 0f)
            {
                time = 0f;
                ChangePhase();
            }

            ShootingTime.Value = time;
        }
    }

}
