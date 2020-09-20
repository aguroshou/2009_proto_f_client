using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySkillSystem : MonoBehaviour
{

    [SerializeField] PlayerManager playerManager;

    enum SkillNumber : int
    {
        AttakUp = 0,
        MaxHitPointUp = 1,
        CollisionRange = 2
    }

    const int SKILL_TYPE = 10;

    //スキルを購入するための選択肢ボタンの数
    const int BUY_SKILL_BUTTON_NUMBER = 3;

    [SerializeField] GameObject[] buySkillButtonObjects = new GameObject[BUY_SKILL_BUTTON_NUMBER];

    //シューティングに渡す取得したスキル一覧の配列と購入した数=スキルのレベル
    public int[] boughtSkillLevel = new int[SKILL_TYPE];

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
        { 300, 450, 500, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { 300, 450, 500, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
        { 300, 450, 500, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE, SOLDOUT_PRICE },
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
            buySkillButtonObjects[i] = GameObject.Find("SkillButton" + i.ToString());
        }
    }

    //スキルレベルからパラメーターに変換する機能を追加する
    public void ClickSkillButton()
    {
        switch (buttonSkillNumber)
        {
            case (int)SkillNumber.AttakUp:
                //if (GameManager.Instance.Chip.Value >= skillPriceTable[buttonSkillNumber][])
                {
                    //playerManager.UpdateLevel(buttonSkillNumber);
                }
                break;
            default:
                break;
        }
    }
}
