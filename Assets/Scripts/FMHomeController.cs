using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class FMHomeController : MonoBehaviour
{
    [SerializeField]
    GameObject loginBonus;
    [SerializeField]
    TextMeshProUGUI labUser;
    [SerializeField]
    TextMeshProUGUI labCurrencyCA;
    [SerializeField]
    TextMeshProUGUI labCurrencyPC;
    [SerializeField]
    TextMeshProUGUI labLoginBonus;

    void Start(){
        //var request = new LoginWithCustomIDRequest { CustomId = "64646464", CreateAccount = false };
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        FMPlayfabLoginBonus.Instance.OnResult = DisplayLoginBuses;
        if (!FMPlayfabLogin.IsClientLoggedIn()) {
            FMPlayfabLogin.LoginCustomID("64646464", OnLoginSuccess);
            return;
        }
        DisplayInfo();
    }

    void DisplayInfo() {
        labUser.text = FMClientSessionData.Instance.UserName;

        labCurrencyCA.text = FMClientSessionData.Instance.currencyCA.ToString();
        labCurrencyPC.text = FMClientSessionData.Instance.currencyPC.ToString();

        //LOGIN BONUS            
        FMPlayfabLoginBonus.Instance.CheckLoginBonus();
    }

    void OnLoginSuccess(LoginResult res)
    {
        //get display name
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest()
        {
            PlayFabId = res.PlayFabId
        }, result =>
        {
            FMClientSessionData.Instance.UserName = result.AccountInfo.TitleInfo.DisplayName;
            labUser.text = result.AccountInfo.TitleInfo.DisplayName;
        }, error => { Debug.Log("error on get account info"); });

        //get currency
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), resInventory =>
        {
            int CO = 0;
            int PC = 0;
            resInventory.VirtualCurrency.TryGetValue("CO", out CO);
            resInventory.VirtualCurrency.TryGetValue("PC", out PC);
            labCurrencyCA.text = CO.ToString();
            labCurrencyPC.text = PC.ToString();
            FMClientSessionData.Instance.currencyCA = CO;
            FMClientSessionData.Instance.currencyPC = PC;
        }
        , error => { Debug.Log("error on get currency info"); });

        //statistics
        PlayfabUtils.Instance.GetPlayerStatistics(null, statRes => {
            FMPlayfabUserStatistics.StoreItemsFromJson(statRes);
            FMClientSessionData.Instance.Statistics = FMPlayfabUserStatistics.Items;
        }, error => { Debug.Log("error on getting statistics"); });

        //getting reward and achievement list
        PlayfabUtils.Instance.GetTitleData(new List<string>{ "fm_achievements", "fm_rewards" }, titleRes=>{
            FMPlayfabAchievements.Instance.StoreItemsFromJson(titleRes);
            FMPlayfabReward.StoreItemsFromJson(titleRes);

            FMClientSessionData.Instance.Achievements = FMPlayfabAchievements.Items;
            FMClientSessionData.Instance.Rewards = FMPlayfabReward.Items;
            Debug.Log("got achievments " + FMClientSessionData.Instance.Achievements.Count);
            Debug.Log("got rewards " + FMClientSessionData.Instance.Rewards.Count);

            //LOGIN BONUS            
            FMPlayfabLoginBonus.Instance.CheckLoginBonus();
        }, error => { Debug.Log("error on get title info"); });

        //get catalogItems
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), catalogRes =>
        {
            FMClientSessionData.Instance.CatalogItems = catalogRes.Catalog;
            //DisplayUserItems(catalogRes.Catalog);
            Debug.Log("got catalog"+ FMClientSessionData.Instance.CatalogItems.Count);
        }
        , error => { Debug.Log("error on get catalog info"); });

        //get user Achievement
        //PlayfabUtils.Instance.GetUserReadOnlyData(new List<string> { "fm_user_achievements" }, result =>{
        //    FMPlayfabUserAchievement.Instance.StoreItemsFromJson(result);
        //    ClientSessionData.Instance.UserAchievements = FMPlayfabUserAchievement.Items;
        //    Debug.Log("got user achivements " + ClientSessionData.Instance.UserAchievements.Count);
        //}, error => { Debug.Log("error on getting read only data"); });
        //var request = new GetUserDataRequest()
        //{
        //    Keys = new List<string>() { "fm_user_achievements"}
        //};
        //PlayFabClientAPI.GetUserReadOnlyData(request, result =>
        //{
        //    Debug.Log("los achievements " + result.ToJson().ToString());
        //    //ClientSessionData.Instance.UserAchievements = 
        //}, OnLoginFailure);
    }

    void DisplayLoginBuses(FMPlayfabLoginBonusResult result) {
        for (int i = 0; i < result.Bonuses.Count; i++) {
            FMLoginBonusItem item = result.Bonuses[i];
            GameObject achievementPrefab = Instantiate(Resources.Load("FMLoginBonusItemUI")) as GameObject;
            FMLoginBonusItemUI itemUI = achievementPrefab.GetComponent<FMLoginBonusItemUI>();

            itemUI.SetData(item);

            itemUI.gameObject.transform.parent = loginBonus.transform;
            itemUI.gameObject.transform.localScale = Vector3.one;
        }

        //loginBonus.Reposition();

        //login bonus label
        labLoginBonus.text = result.GetTodayBonusMessage();
    }
        

    void OnLoginFailure(PlayFabError error){
        Debug.Log("login error");
    }

    //void DisplayUserItems(List<CatalogItem> items) {
    //    for (int i = 0; i < items.Count; i++) {
    //        CatalogItem item = items[i];
    //        GameObject itemPrefab = Instantiate(Resources.Load("Prefabs/InventoryItemUI")) as GameObject;
    //        InventoryItemUI itemUI = itemPrefab.GetComponent<InventoryItemUI>();

    //        itemUI.SetData(item.DisplayName, item.ItemImageUrl);

    //        itemUI.gameObject.transform.parent = itemGrid.transform;
    //        itemUI.gameObject.transform.localScale = Vector3.one;            
    //    }

    //    itemGrid.Reposition();
    //}

    void Update(){

    }


    public void GoToAchievements(){
        SceneManager.LoadScene("PlayfabAchivements");
    }

    public void GoToStore(){
        SceneManager.LoadScene("Store");
    }

    public void GoToInventory(){
        SceneManager.LoadScene("Inventory");
    }

    public void GoToStageBuilder() {
        SceneManager.LoadScene("StageBuilder");
    }

    public void GoToBuilderTest() {
        SceneManager.LoadScene("BuilderTest");
    }

    public void GoTo3DPlayTest(){
        SceneManager.LoadScene("Test3D");
    }
}