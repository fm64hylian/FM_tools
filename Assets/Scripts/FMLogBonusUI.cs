using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;


/// <summary>
/// this ons is not being used at in script? why
/// </summary>
public class FMLogBonusUI : MonoBehaviour
{
    [HideInInspector]
    public FMLoginBonusState State = FMLoginBonusState.Unclaimed;
    [SerializeField]
    TextMeshProUGUI rewardLab;
    [SerializeField]
    Image rewardImg;

    Image bgImage;
    Color32 claimedColor = new Color32(60, 140, 60, 255); //3C8C3C
    Color32 todayColor = new Color32(142, 142, 60, 255); //8E8E3C
    void Start()
    {
        bgImage = GetComponent<Image>();
    }

    public void SetData(FMLoginBonusItem item)
    {
        string isCurrency = item.Reward.Type == FMRewardType.Currency ? "x " : "";
        Debug.Log("LOG BONUSUI checking reward.getValue");
        Debug.Log(item.Reward);
        Debug.Log(item.Reward.GetValue());
        rewardLab.text = isCurrency + item.Reward.GetValue();

        State = item.State;

        //sometimes start is called after this
        if (rewardImg == null)
        {
            rewardImg = GetComponent<Image>();
        }

        // 1. Get the raw image string from PlayFab (e.g., "get_item")
        string spriteName = GetRewardSprite(item.Reward);

        // 2. Load the texture reference from your combined Resources folders
        Sprite loadedSprite = Resources.Load<Sprite>(spriteName);
        rewardImg.sprite = loadedSprite;
        

        switch (State)
        {
            case FMLoginBonusState.Today:
                bgImage.color = todayColor;
                //bgSprite.color = todayColor;
                break;
            case FMLoginBonusState.Claimed:
                bgImage.color = claimedColor;
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
                //TODO replace static path
                return "Texture/UI/generic_item";
        }
    }
}
