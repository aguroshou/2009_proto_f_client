using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private float shootingTimeLimit = 20f;

    [SerializeField]
    private float shootingReadyTimeLimit = 3f;

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
        SHOOTING_READY_PHASE,  // シューティング前の準備期間
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
            Phase.Value = EGamePhase.SHOOTING_READY_PHASE;
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
        else
        {
            Debug.LogError("フェーズ遷移出来ないフェーズです");
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

    /// <summary>
    /// ポーカーの役
    /// </summary>
    public enum EPokerHand : int
    {
        NONE = -1, // 初期値
        HIGH_CARD = 0,  // ブタ
        ONE_PAIR = 1,  // ワンペア
        TWO_PAIR = 2,  // ツーペア
        THREE_CARD = 3,  // スリーカード
        FULL_HOUSE = 4,  // フルハウス
        FOUR_CARD = 5,  // フォーカード
        FIVE_CARD = 6  // ファイブカード
    }

    /// <summary>
    /// プレイヤーが勝ったか（プレイヤーが基準）
    /// </summary>
    public enum EPokerWin : int
    {
        PLAYER_WIN = 1,
        DRAW = 0,
        ENEMY_WIN = -1,
    }

    ReactiveProperty<EPokerHand> playerPokerHand = new ReactiveProperty<EPokerHand>(EPokerHand.NONE);
    ReactiveProperty<EPokerHand> enemyPokerHand = new ReactiveProperty<EPokerHand>(EPokerHand.NONE);
    ReactiveProperty<EPokerWin> pokerWin = new ReactiveProperty<EPokerWin>(EPokerWin.DRAW);
    /// <summary>
    /// PokerSystemオブジェクトから味方・敵のポーカー役をセーブする
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    public void SavePokerHand(EPokerHand player, EPokerHand enemy)
    {
        playerPokerHand.Value = player;
        enemyPokerHand.Value = enemy;

        if(player > enemy)
        {
            pokerWin.Value = EPokerWin.PLAYER_WIN;
        }else if(player < enemy)
        {
            pokerWin.Value = EPokerWin.ENEMY_WIN;
        }
        else
        {
            pokerWin.Value = EPokerWin.DRAW;
        }

        // プレイヤー射撃間隔変更
        PlayerManager.Instance.ShootingInterval = 0.7f - 0.1f * (int)player;
        // 敵側スポーン数変更
        EnemyManager.Instance.EnemyCount = 1 + (int)enemy;
    }

    public ReactiveProperty<float> ShootingReadyTime = new ReactiveProperty<float>(0f);
    public ReactiveProperty<float> ShootingTime = new ReactiveProperty<float>(0f);

    public ReactiveProperty<int> Chip = new ReactiveProperty<int>(10000);

    private void Start()
    {
        Phase.Subscribe((phase) =>
        {
            // シューティングレディーフェーズ　タイマースタート
            if (phase == EGamePhase.SHOOTING_READY_PHASE)
            {
                ShootingReadyTime.Value = shootingReadyTimeLimit;
            }
            // シューティングフェーズ　タイマースタート
            if (phase == EGamePhase.SHOOTING_PHASE)
            {
                ShootingTime.Value = shootingTimeLimit;
            }
        });
    }

    private void Update()
    {
        // シューティング前に時間間隔を開ける
        if (Phase.Value == EGamePhase.SHOOTING_READY_PHASE)
        {
            float time = ShootingReadyTime.Value;
            time -= Time.deltaTime;
            if (time < 0f)
            {
                time = 0f;
                // ChangePhase();
                Phase.Value = EGamePhase.SHOOTING_PHASE;
            }

            ShootingReadyTime.Value = time;
        }

        // シューティングフェーズ　タイマー管理
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
