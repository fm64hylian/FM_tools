using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class FMClientSessionData : MonoBehaviour
{
    public string PlayfabID;
    public string UserName;
    public List<FMUserStatistic> Statistics = new List<FMUserStatistic>();
    public List<FMAchievementItem> Achievements = new List<FMAchievementItem>();
    public List<FMUserAchievement> UserAchievements = new List<FMUserAchievement>();
    public List<FMRewardItem> Rewards = new List<FMRewardItem>();
    public List<CatalogItem> CatalogItems = new List<CatalogItem>();
    public List<FMInventoryItem> InventoryItems = new List<FMInventoryItem>();
    public List<FMInventorySlot> Slots = new List<FMInventorySlot>();

    public int currencyCA;
    public int currencyPC;
    static FMClientSessionData instance;    

    public static FMClientSessionData Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ClientData");
                instance = obj.AddComponent<FMClientSessionData>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
