using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FMIntroLoaderController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI labLoading;
    [SerializeField]
    TextMeshProUGUI percentLab;
    [SerializeField]
    Slider progressBar;
    float progress = 0.0f;
    float total = 0.0f;
    bool loadComplete = false;

    void Start()
    {
        progressBar.value = 0.0f;
        labLoading.text = "... Logging in ...";
        FMPlayfabLogin.LoginCustomID("64646464", OnLoginSuccess);
    }

    /// <summary>
    /// nesting so many callbacks is not that good practice, but for the sake of
    /// keeping it sinchronous, we'll do it just this one time
    /// </summary>
    /// <param name="logResult"></param>
    void OnLoginSuccess(LoginResult logResult)
    {
        List<ItemInstance> inventoryItems = new List<ItemInstance>();
        //get display name
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest()
        {
            PlayFabId = logResult.PlayFabId
        }, result =>
        {
            FMClientSessionData.Instance.PlayfabID = logResult.PlayFabId;
            FMClientSessionData.Instance.UserName = result.AccountInfo.TitleInfo.DisplayName;
            labLoading.text = "... Loading user info ...";
            progress += 0.20f;
            total += progress;

            //get currency
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), resInventory =>
            {
                //currency
                int CA = 0;
                int PC = 0;
                resInventory.VirtualCurrency.TryGetValue("CA", out CA);
                resInventory.VirtualCurrency.TryGetValue("PC", out PC);
                FMClientSessionData.Instance.currencyCA = CA;
                FMClientSessionData.Instance.currencyPC = PC;

                //inventory
                inventoryItems = resInventory.Inventory;
                labLoading.text = "... Loading Inventory ...";
                progress += 0.20f;
                //total += progress;

                //statistics
                PlayfabUtils.Instance.GetPlayerStatistics(null, statRes =>
                {
                    FMPlayfabUserStatistics.StoreItemsFromJson(statRes);
                    FMClientSessionData.Instance.Statistics = FMPlayfabUserStatistics.Items;
                    labLoading.text = "... Loading User Statistics ...";
                    progress += 0.20f;
                    total += progress;

                    //get title Data
                    PlayfabUtils.Instance.GetTitleData(new List<string> { "fm_achievements", "fm_rewards" }, titleRes =>
                    {
                        FMPlayfabAchievements.Instance.StoreItemsFromJson(titleRes);
                        FMPlayfabReward.StoreItemsFromJson(titleRes);

                        FMClientSessionData.Instance.Achievements = FMPlayfabAchievements.Items;
                        FMClientSessionData.Instance.Rewards = FMPlayfabReward.Items;

                        labLoading.text = "... Loading Title Data ...";
                        progress += 0.20f;
                        total += progress;

                        //get catalogItems
                        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), catalogRes =>
                        {
                            FMClientSessionData.Instance.CatalogItems = catalogRes.Catalog;

                            //crossing CatalogItem and ItemInstance items
                            FMPlayFabInventory.StoreItemsFromPlayfab(catalogRes.Catalog, inventoryItems);
                            FMClientSessionData.Instance.InventoryItems = FMPlayFabInventory.Items;

                            //get user equipped items (from ReadOnlyData)
                            PlayfabUtils.Instance.GetUserReadOnlyData(new List<string>() { "fm_user_equipment" },
                                useDataRes => {
                                    //if created ,assign to client
                                    if (useDataRes.Data.ContainsKey("fm_user_equipment"))
                                    {
                                        Debug.Log("read only data");
                                        FMPlayFabInventory.StoreSlotsFromJson(useDataRes);
                                        SceneManager.LoadScene("Home", LoadSceneMode.Single);
                                    }
                                    //if not created, assign for the first time
                                    else
                                    {
                                        Debug.Log("no equippment found, creating");
                                        FMPlayFabInventory.CreateUserEquipment(slotRes => {

                                            FMPlayFabInventory.StoreSlotsFromJson(slotRes);

                                            Debug.Log("getting Equip slots for first time" + FMClientSessionData.Instance.Slots.Count);
                                            SceneManager.LoadScene("Home", LoadSceneMode.Single);

                                        }, error => {
                                            Debug.Log("error on get userEquipment");
                                            //end get userEquipment
                                        });
                                    }

                                }, useDataError => { Debug.Log("error on get userEquipment using getUserdata"); });
                            //end get userEquipment with GetUserData

                            labLoading.text = "... Loading Catalog Items ...";
                            progress += 0.20f;
                            total += progress;
                        }
                        , error => { Debug.Log("error on get catalog info"); });
                        //end catalog

                    }, error => { Debug.Log("error on get title info"); });
                    //end get title

                }, error => { Debug.Log("error on getting statistics"); });
                //end get Statistics
            }
            , error => { Debug.Log("error on get currency info"); });
            //end get currency

        }, error => { Debug.Log("error on get account info"); });
        //end get account info
    }

    void Update()
    {
        if (loadComplete)
        {
            return;
        }

        if (progress >= 1f && !loadComplete)
        {
            loadComplete = true;
            GoToHome();
            return;
        }
        progressBar.value = (progress * 100);//Mathf.Lerp(progress, total, Time.deltaTime);
        percentLab.text = (progress * 100).ToString() + "%";//Mathf.Lerp(progress, total, Time.deltaTime);
    }

    void GoToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
