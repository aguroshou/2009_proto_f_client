using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //テキストはPrefabのInspectorから参照を付ける
    [SerializeField] Text[] priceTexts = new Text[BUY_SKILL_BUTTON_NUMBER];
    [SerializeField] Text[] parameterTexts = new Text[BUY_SKILL_BUTTON_NUMBER];

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
        //FIXME: パラメーター調整
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
    //最後の10番目の値は使わない
    [SerializeField]
    float[,] skillParameterTable = new float[SKILL_TYPE, 10] {
        //FIXME: パラメーター調整
        { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, },
        { 10, 12, 14, 16, 18, 20, 22, 24, 26, 26 },
        { 0.60f, 0.55f, 0.5f, 0.45f, 0.4f, 0.35f, 0.3f, 0.25f, 0.20f, 0.20f },
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

        parameterTexts[0].text = "Lv0¥n攻撃力:1";
        parameterTexts[1].text = "Lv0¥nHP:10";
        parameterTexts[2].text = "Lv0¥n当たり判定:0.66";
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

            //スキル購入画面のテキストを変更する
            string newParameter = "";
            string parameterJapaneseName = "";
            
            switch (buySkillNumber)
            {
                case (int)SkillNumber.AttackUp:
                    newParameter = playerManager.AttackPoint.ToString();
                    parameterJapaneseName = "攻撃力";
                    break;
                case (int)SkillNumber.MaxHitPointUp:
                    newParameter = playerManager.MaxHp.ToString();
                    parameterJapaneseName = "HP";
                    break;
                case (int)SkillNumber.CollisionRange:
                    newParameter = playerManager.AttackPoint.ToString();
                    parameterJapaneseName = "当たり判定";
                    break;
                default:
                    break;
            }

            parameterTexts[buySkillNumber].text = "Lv" + buySkillLevel[buySkillNumber] + "¥n" + ":" + parameterJapaneseName;

        }
    }
}