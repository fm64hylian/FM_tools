using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FMLoginBonusState {
    Unclaimed,
    Today,
    Claimed
}

public class FMLoginBonusItemUI : MonoBehaviour
{
    [HideInInspector]
    public FMLoginBonusState State = FMLoginBonusState.Unclaimed;
    [SerializeField]
    Sprite rewardSprite;
    [SerializeField]
    TextMeshPro rewardLab;    

    Sprite bgSprite;
    Color32 claimedColor = new Color32(60, 140,60, 255); //3C8C3C
    Color32 todayColor = new Color32(142, 142, 60, 255); //8E8E3C

    void Start()
    {
        bgSprite = GetComponent<Sprite>();
    }

    public void SetData(FMLoginBonusItem item) {
        string isCurrency = item.Reward.Type == FMRewardType.Currency ? "x " : "";
        rewardLab.text = isCurrency+ item.Reward.GetValue();
        rewardSprite.name = GetRewardSprite(item.Reward); //TODO revisar esto
        State = item.State;

        //sometimes start is called after this
        if (bgSprite == null) {
            bgSprite = GetComponent<Sprite>();
        }

        switch (State) {
            case FMLoginBonusState.Today:
                //TODO revisar como era en sprite
                //bgSprite.color = todayColor;
                break;
            case FMLoginBonusState.Claimed:
                //bgSprite.color = claimedColor;
                break;
        }
    }

    string GetRewardSprite(FMRewardItem reward)
    {
        switch (reward.RewardTypeValue)
        {
            case "co":
                return "coin_CO";
            case "pc":
                return "coin_PC";
            default:
                return "generic_item";
        }
    }

    void Update()
    {
        //TODO make color tween?
    }
}
