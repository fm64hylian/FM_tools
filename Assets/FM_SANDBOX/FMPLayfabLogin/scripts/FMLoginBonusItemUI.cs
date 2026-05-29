using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum FMLoginBonusState
{
    Unclaimed,
    Today,
    Claimed
}

/// <summary>
/// TODO chekc if this is being used
/// </summary>
public class FMLoginBonusItemUI : MonoBehaviour
{
    [HideInInspector]
    public FMLoginBonusState State = FMLoginBonusState.Unclaimed;
    [SerializeField]
    Image rewardSprite;
    [SerializeField]
    TextMeshPro rewardLab;

    Image bgSprite;
    Color32 claimedColor = new Color32(60, 140, 60, 255); //3C8C3C
    Color32 todayColor = new Color32(142, 142, 60, 255); //8E8E3C

    void Awake()
    {
        bgSprite = GetComponent<Image>();
    }

    public void SetData(FMLoginBonusItem item)
    {
        Debug.Log("checking loginbonusItem set data ---");

        string isCurrency = item.Reward.Type == FMRewardType.Currency ? "x " : "";
        Debug.Log("LOGIN BONUSUI checking reward.getValue");
        Debug.Log(item.Reward);
        Debug.Log(item.Reward.GetValue());
        //TODO item.Reward.GetValue(); is null
        //check bonusitem instead of rewards (rewards are fine)
        rewardLab.text = isCurrency + item.Reward.GetValue();

        //rewardSprite.name = GetRewardSprite(item.Reward); //TODO revisar esto
        State = item.State;

        // 1. Get the raw image string from PlayFab (e.g., "get_item")
        string spriteName = GetRewardSprite(item.Reward);

        // 2. Load the texture reference from your combined Resources folders
        Debug.Log("trying to get sprite " + spriteName);
        Sprite loadedSprite = Resources.Load<Sprite>(spriteName);
        Debug.Log(loadedSprite);
        rewardSprite.sprite = loadedSprite;

        if (bgSprite != null)
        {
            switch (State)
            {
                case FMLoginBonusState.Today:
                    bgSprite.color = todayColor;
                    break;
                case FMLoginBonusState.Claimed:
                    Debug.Log("checking bg sprite");
                    Debug.Log(bgSprite);
                    Debug.Log(bgSprite.color);
                    bgSprite.color = claimedColor;
                    break;
            }
        }

    }

    string GetRewardSprite(FMRewardItem reward)
    {
        Debug.Log("on GetRewardSprite, checking type: " + reward.RewardTypeValue);
        switch (reward.RewardTypeValue)
        {
            case "ca": //former name
                return "coin_CO";
            case "pc":
                return "coin_PC";
            default:
                //TODO replace static path
                return "Texture/UI/generic_item";
        }
    }

    void Update()
    {
        //TODO make color tween?
    }
}
