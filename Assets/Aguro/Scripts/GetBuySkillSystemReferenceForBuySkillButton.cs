using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetBuySkillSystemReferenceForBuySkillButton : MonoBehaviour
{
    //左から順番に0〜2のスキル購入ボタンの番号
    [SerializeField] int buySkillButtonNumber;

    GameObject buySkillSystemObject;

    void Start()
    {
        buySkillSystemObject = GameObject.Find("BuySkillSystemGameObject");
        Button button = GetComponent<Button>();
        BuySkillSystem buySkillSystem = buySkillSystemObject.GetComponent<BuySkillSystem>();
        button.onClick.AddListener(() => buySkillSystem.SkillButtonClicked(buySkillButtonNumber));
    }
}
