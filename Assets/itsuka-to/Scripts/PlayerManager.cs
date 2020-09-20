using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    // プレイヤーの体力
    public ReactiveProperty<int> Hp;


    /// <summary>
    /// (int) PlayerManager.Instance.MaxHp
    /// </summary>
    // プレイヤーの最大体力
    public int MaxHp
    {
        get { return playerController.maxHp; }
        set { playerController.maxHp = value; }
    }

    public int AttackPoint
    {
        get { return playerShooting.attackPoint; }
        set { playerShooting.attackPoint = value; }
    }

    public float CircleCollidorRadius
    {
        get { return playerCircleCollidor2D.radius; }
        set { playerCircleCollidor2D.radius = value; }
    }
    

    // コンポーネント
    private PlayerShooting playerShooting;
    private PlayerController playerController;
    private CircleCollider2D playerCircleCollidor2D;

    override protected void Awake()
    {
        base.Awake();
        var player = GameObject.Find("Player");
        playerShooting = player.GetComponent<PlayerShooting>();
        playerController = player.GetComponent<PlayerController>();
        playerCircleCollidor2D = player.GetComponent<CircleCollider2D>();

        Hp = playerController.hp;
    }

    ///// <summary>
    ///// 球のレベルを更新
    ///// </summary>
    ///// <param name="level"></param>
    //public void UpdateBulletLevel(int level)
    //{
    //    playerShooting.attackPoint = value;
    //}


    ///// <summary>
    ///// プレイヤー当たり判定のレベルを更新
    ///// </summary>
    ///// <param name="level"></param>
    //public void UpdateCollisionRangeLevel(int level)
    //{
    //    playerCircleCollidor2D.radius -= delta * level;
    //}

    ///// <summary>
    ///// プレイヤーHPのレベルを更新
    ///// </summary>
    ///// <param name="level"></param>
    //public void UpdateHpLevel(int level)
    //{

    //}

}
