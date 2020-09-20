using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySkillSystem : MonoBehaviour
{
    GameObject playerManagerObject;
    [SerializeField] PlayerManager playerManager;

    enum SkillNumber : int
    {
        AttackUp = 0,
        MaxHitPointUp = 1,
        CollisionRange = 2
    }

    const int SKILL_TYPE = 10;

    //スキルを購入するための選択肢ボタンの数
    const int BUY_SKILL_BUTTON_NUMBER = 3;

    [SerializeField] GameObject[] buySkillButtonObjects = new GameObject[BUY_SKILL_BUTTON_NUMBER];

    //これから購入するスキルのレベルをそれぞれ配列で保存(現在の購入した数 == 現在のスキルのレベル == buySkillLevel[これから買うスキルの番号] - 1 )
    public int[] buySkillLevel = new int[SKILL_TYPE];

    //スキルの番号
    [SerializeField] int buttonSkillNumber;
    //0：通常弾の攻撃力10増加、300チップ
    //1：最大体力10増加、300チップ
    //2：自機当たり判定10％縮小、300チップ
    //↓未実装
    //3：発射する弾の移動速度20％増加、300チップ
    //4：(カードのペアで揃えた攻撃と通常弾の両方)攻撃速度10％増加、700チップ
    //5：体力が0になったときに1度だけ全回復の状態で復活(購入後使うまで持続し、1度使うと無くなる。3つまで所持できる)、3000チップ
    //6：カード入れ替え回数増加、3500チップ
    //7：常時カードのペア1ランクアップ、7000チップ
    //8：(没案)移動速度10％増加、300チップ

    //スキルを購入できなくなる売り切れ用の文字「SOLDOUT」を表示する代わりに、異常に値段を高くして買えなくする
    const int SOLDOUT_PRICE = 999999;

    //スキルは購入するごとに値段が上がる
    //skillPriceTable[スキルの種類][n回目に購入するときの値段]
    [SerializeField]
    int[,] skillPriceTable = new int[SKILL_TYPE, 10] {
        { 300, 450, 600, 750, 900, 1050, 1200, 1350, 1500, SOLDOUT_PRICE },
        { 300, 450, 600, 750, 900, 1050, 1200, 1350, 1500, SOLDOUT_PRICE },
        { 300, 450, 600, 750, 900, 1050, 1200, 1350, 1500, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE }
    };

    //スキルは購入するごとにパラメーターが上がる
    //skillPriceTable[スキルの種類][n回目に購入したときのスキルのパラメーター]
    //初期パラメーター値はこのテーブルに入っていないです
    //floatにしていますが、一部int型のスキルパラメーターがあるので、(int)のキャストしなければいけないです
    [SerializeField]
    float[,] skillParameterTable = new float[SKILL_TYPE, 10] {
        //FIXME: 初期値を井塚さんに聞いて直す
        { 100, 120, 500, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { 100, 120, 500, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { 100, 120, 500, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        //↓未実装
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE }
    };

    void Start()
    {
        for (int i = 0; i < BUY_SKILL_BUTTON_NUMBER; i++)
        {
            buySkillButtonObjects[i] = GameObject.Find("BuySkillButton" + i.ToString());
        }
        playerManagerObject = GameObject.Find("PlayerManager");
        playerManager = playerManagerObject.GetComponent<PlayerManager>();

        GameManager.Instance.Chip.Value = 1000;
    }

    //スキルレベルからパラメーターに変換する機能を追加する
    public void SkillButtonClicked(int buySkillButtonNumber)
    {
        //スキルの種類を3種類よりも増やすなら変更が必要です
        int buySkillNumber = buySkillButtonNumber;

        if (GameManager.Instance.Chip.Value >= skillPriceTable[buySkillNumber, buySkillLevel[buySkillNumber]])
        {
            GameManager.Instance.Chip.Value -= skillPriceTable[buySkillNumber, buySkillLevel[buySkillNumber]];

            //買ったスキルのパラメーターを反映させる
            switch (buySkillNumber)
            {
                case (int)SkillNumber.AttackUp:
                    playerManager.AttackPoint = (int)skillParameterTable[buySkillNumber, buySkillLevel[buySkillNumber]];
                    break;
                case (int)SkillNumber.MaxHitPointUp:
                    playerManager.MaxHp = (int)skillParameterTable[buySkillNumber, buySkillLevel[buySkillNumber]];
                    break;
                case (int)SkillNumber.CollisionRange:
                    playerManager.CircleCollidorRadius = skillParameterTable[buySkillNumber, buySkillLevel[buySkillNumber]];
                    break;
                default:
                    break;
            }

            //次に購入するときのために購入するスキルレベルを1段階上げる
            buySkillLevel[buySkillNumber]++;
        }
    }
}
