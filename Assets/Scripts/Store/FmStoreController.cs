using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum FMCurrencyType
{
    CA,
    PC
}

/// <summary>
/// TODO remove ANY equip shenanigans
/// </summary>
public class FmStoreController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CAlab;
    [SerializeField]
    TextMeshProUGUI PClab;
    [SerializeField]
    GameObject detailPanel;
    [SerializeField]
    FMStoreObjectManager ItemManager;
    [SerializeField]
    GameObject itemGridContentStore;
    [SerializeField]
    GameObject itemPreview;

    FMStoreItemUI selectedItemUI;
    FMStoreObject currentEquipedObject;
    FMCurrencyType selectedCurrency;
    //FMEquipmentSlotsType selectedSlot = FMEquipmentSlotsType.None;

    Image itemDetailImage;
    TextMeshProUGUI itemDetailName;
    TextMeshProUGUI itemDetailDescription;
    //TextMeshPro itemEffect;
    Button buttonBuyCA;
    Button buttonBuyPC;

    //bool isFirstWeapon = true;
    //bool isSceneLoaded = false;

    void Start()
    {
        CAlab.text = "" + FMClientSessionData.Instance.currencyCA;
        PClab.text = "" + FMClientSessionData.Instance.currencyPC;

        //panel Detail Right
        //itemDetailImage = detailPanel.GetComponentsInChildren<Image>()[1];
        itemDetailName = detailPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        itemDetailDescription = detailPanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        //itemDetailDescription = detailPanel.GetComponentsInChildren<TextMeshPro>()[2];
        //itemEffect = detailPanel.GetComponentsInChildren<TextMeshPro>()[0];

        buttonBuyCA = detailPanel.GetComponentsInChildren<Button>()[0];
        buttonBuyPC = detailPanel.GetComponentsInChildren<Button>()[1];

        if (FMClientSessionData.Instance.CatalogItems.Count == 0) {
            Debug.Log("no data");
            return;
        }

        initValues();
    }

    void initValues()
    {
        displayItems();

        //create itemanager pool
        ItemManager.SetData(FMClientSessionData.Instance.CatalogItems);


        //if (selectedItemUI)
        //{
        //    Debug.Log("first selected item by default - " + selectedItemUI.Title);
        //    selectedItemUI.OnItemSelected();
        //}
    }


    void displayItems()
    {
        //itemsUI.Add(itemNoneUI);
        List<CatalogItem> cItems = FMClientSessionData.Instance.CatalogItems;
        //Debug.Log("FMSTORE: Numero de items equipados (ClientSessionData): " + FMClientSessionData.Instance.InventoryItems.FindAll(x => x.IsEquipped).Count);
        //Debug.Log("FMSTORE: Numero de items equipados (SIPlayFabInventory): " + FMPlayFabInventory.Items.FindAll(x => x.IsEquipped).Count);
        //adding from Catalog

        for (int i = 0; i < cItems.Count; i++)
        {
            CatalogItem item = cItems[i];
            //Debug.Log("Agregando: " + item.DisplayName);
            //TODO check premium items
            if (!item.ItemClass.Equals("Premium"))
            {
                switch (item.ItemClass)
                {
                    case "Equipment":
                    //case "Consumable":
                        GameObject equipmentPrefab = Instantiate(Resources.Load("Prefabs/UI/FMStoreItemUI")) as GameObject;
                        //Debug.Log(equipmentPrefab);
                        FMStoreItemUI equipmentUI = equipmentPrefab.GetComponent<FMStoreItemUI>();

                        uint CA;
                        uint PC;

                        item.VirtualCurrencyPrices.TryGetValue(FMCurrencyType.CA.ToString(), out CA);
                        item.VirtualCurrencyPrices.TryGetValue(FMCurrencyType.PC.ToString(), out PC);

                        equipmentUI.SetData(item.ItemId,item.DisplayName, item.Description,item.ItemImageUrl,CA,PC);
                        equipmentUI.OnSelected = DisplaySelectedItem;


                        /*
                        itemUI.gameObject.transform.SetParent(itemGridContent);
                        itemUI.gameObject.transform.localScale = Vector3.one;
                        */

                        // itemUI.transform.SetParent(itemGridContentStore.transform, false);
                        equipmentUI.gameObject.transform.SetParent(itemGridContentStore.transform, false);
                        equipmentUI.gameObject.transform.localScale = Vector3.one;
                        equipmentUI.gameObject.transform.localPosition = Vector3.zero;
                        break;
                    default:
                        break;
                }

            }
            //bundles (real money) bundles on premium grid
            else
            {
                //GameObject premiumPrefab = Instantiate(Resources.Load("ItemPremiumUI")) as GameObject;
                ////Debug.Log("item ui "+itemPrefab);
                //SIStoreitemPremiumUI itemUI = premiumPrefab.GetComponent<SIStoreitemPremiumUI>();

                //itemUI.SetData(item);
                //itemUI.OnPurchased = PurchasePremium;
                ////itemUI.OnConfirmed = ConfirmPremiumPurchase;

                //itemUI.gameObject.transform.SetParent(premiumGridContent.transform, false);
                //itemUI.gameObject.transform.localScale = Vector3.one;
                //itemUI.gameObject.transform.localPosition = Vector3.zero;
                // OHO CON LA LISTA
            }

        }

        //TODO, for now we will divide them in weird ways until more slots are implemented
        FilterByWeapon();
    }


    //UBody_HandL,
    //UBody_HandR,
    public void FilterByWeapon()
    {
        filter("Weapon");
    }

    //Head_Hair,
    //Head_UpperFace,
    //Head_LowerFace,
    //UBody_Clothes
    public void FilterByArmor()
    {
        filter("Armor");
    }

    //Consumable
    public void FilterByMisc()
    {
        filter("Misc");
    }

    void filter(string iType)
    {
        for (int i = 0; i < itemGridContentStore.transform.GetComponentsInChildren<Transform>(true).Length; i++)
        {
            FMStoreItemUI item = itemGridContentStore.transform.GetComponentsInChildren<Transform>(true)[i].GetComponent<FMStoreItemUI>();
            if (item != null)
            {
                //Debug.Log("filtrando " + item.Title);
                bool show = false;
                FMEquipmentSlotsType slotType = FMEquipmentSlotsType.None;
                CatalogItem cItem = FMPlayFabInventory.GetCatalogItemFromID(item.ItemID);
                if (cItem.ItemClass.Equals("Equipment")) {
                    slotType = FMPlayFabInventory.GetSlotType(cItem);
                }

                switch (iType) {
                    case "Weapon":
                        //show = slotType.Equals(FMEquipmentSlotsType.UBody_HandL | FMEquipmentSlotsType.UBody_HandR);
                        show = slotType.Equals(FMEquipmentSlotsType.UBody_HandL) || slotType.Equals(FMEquipmentSlotsType.UBody_HandR);
                        break; 
                    case "Armor":
                        //show = slotType.Equals(FMEquipmentSlotsType.Head_Hair | FMEquipmentSlotsType.Head_UpperFace 
                        //    | FMEquipmentSlotsType.Head_LowerFace | FMEquipmentSlotsType.UBody_Clothes);
                        show = slotType.Equals(FMEquipmentSlotsType.Head_Hair) || slotType.Equals(FMEquipmentSlotsType.Head_UpperFace) 
                            || slotType.Equals(FMEquipmentSlotsType.Head_LowerFace) || slotType.Equals(FMEquipmentSlotsType.UBody_Clothes);
                        break;
                    case "Misc":
                        break;
                }

                item.gameObject.SetActive(show);




                ////Debug.Log("Slot a comparar: "+ slot+" con "+BDPlayFabInventory.GetSlotType(item.Item).ToString());
                //item.gameObject.SetActive(FMPlayFabInventory.GetSlotType(item.Item).ToString().Equals(iType));
                //if (iType.Equals("Consumable") || iType.Equals("Weapon"))
                //{
                //    item.gameObject.SetActive(item.Item.ItemClass.Equals(iType));
                //}
            }
        }
        //Se resetea la seleccion segun el filtro actual
        //if (selectedItemUI != null) selectedItemUI.Deselect();
        //selectedItemUI = null;
        //ClearDetail();
    }

    /// <summary>
    /// Equipar del inventario con catalogID
    /// </summary>
    /// <param name="catalogObjects"></param>
    //public void EquipWithCatalogItem()
    //{
    //    List<FMInventoryItem> invItems = FMClientSessionData.Instance.InventoryItems.FindAll(x => x.IsEquipment());
    //    List<CatalogItem> catalogItems = FMClientSessionData.Instance.CatalogItems.FindAll(x => x.ItemClass.Equals("Equipment"));
    //    List<FMInventorySlot> slots = FMClientSessionData.Instance.Slots;
    //    for (int i = 0; i < slots.Count; i++)
    //    {
    //        if (!slots[i].CurrentItem.Equals("None"))
    //        {
    //            FMInventoryItem invItem = invItems.Find(x => x.InstanceID.Equals(slots[i].CurrentItem));
    //            //Si el usuario tiene el item cruzarlo con la informacion de catalog item
    //            if (invItem != null)
    //            {
    //                CatalogItem catalogItem = catalogItems.Find(x => x.ItemId.Equals(invItem.CatalogID));
    //                //Equipar el catalog item de item manager
    //                FMStoreEquipmentObject selectedItem = ItemManager.GetItem(catalogItem);
    //                //Debug.Log("Equipando " + selectedItem.DisplayName);
    //                avatarController.EquipStoreSlot(selectedItem);

    //            }
    //        }
    //    }
    //}

    void DisplaySelectedItem(FMStoreItemUI itemUI)
    {
        //selectedItemUI = itemUI;

        //TODO add the item spinning
        CatalogItem cItem = FMPlayFabInventory.GetCatalogItemFromID(itemUI.ItemID);
        
        //Sprite sprite = Resources.Load<Sprite>("Textures/Inventory/" + itemUI.ImageUrl);
        //itemDetailImage.sprite = sprite;



        itemDetailName.text = itemUI.Title;
        itemDetailDescription.text = itemUI.Description;

        bool hasCA = itemUI.CA > 0;
        bool hasPC = itemUI.PC > 0;



        buttonBuyCA.gameObject.SetActive(hasCA);
        buttonBuyPC.gameObject.SetActive(hasPC);

        if (hasCA)
        {
            setPriceButton(buttonBuyCA, FMCurrencyType.CA.ToString(),cItem);
        }
        if (hasPC)
        {
            setPriceButton(buttonBuyPC, FMCurrencyType.PC.ToString(), cItem);
        }


        ////Equipando Item
        ////if (itemUI.IsEquipment())
        ////{
        ////TODO Revisar todos los casos por que ya no existe el None
        FMStoreObject newStoreObject = ItemManager.GetItem(cItem);






        ////FMStoreEquipmentObject currentEquipedObject = avatarController.GetStoreItemFromLocalSlot(selectedLocalSlot);

        //Debug.Log("selected item -" + currentEquipedObject);
        if (selectedItemUI == null)
        {
            //Caso1: Entrando al filtro, ningun item seleccionado en UI del slot (equipar item)
            //if (currentEquipedObject == null)
            //{
                //TODO Considerar que este caso no deberia ocurrir nunca ya que siempre deberia tener algo equipado
                Debug.Log("Caso 1: ningun item seleccionado y nada equipado, equipando " + itemUI.Title);
                //selectedItemUI.Deselect();
                DisplayItemInPreview(newStoreObject);
            currentEquipedObject = newStoreObject;
            //}
            //Caso 1 A con ascendensia a capricornio: ningun item seleccionado en el slot UI
            //Pero existe un item equipado en el slot (desequipar item anterior y equipar item actual)
            //else
            //{
            //    Debug.Log("Caso 1 A prima: ningun item seleccionado pero algo equipado, equipando " + itemUI.Title);
            //    ItemManager.ResetItem(currentEquipedObject);
            //}
            //selectedItemUI = itemUI;

        }

        //Caso2: selecciona item, si el slot del item seleccionado estEocupado
        //(desequipar item actual en el slot y equipar nuevo)
        else if (selectedItemUI != null && newStoreObject != null)
        {
            Debug.Log("Caso 2: desequipar y equipar nuevo ");
            if (currentEquipedObject != null)
            {
                Debug.Log("Desequipando " + currentEquipedObject.DisplayName + " y equipando " + newStoreObject.DisplayName);
                ItemManager.ResetItem(currentEquipedObject);
                DisplayItemInPreview(newStoreObject);
                 currentEquipedObject = newStoreObject;
            }
            //Caso 3? consumables por prmiera vez (nada equipado en ese slot)
            //TODO decidir si toggle entre weapons y consumables (1 cosa en las manos a la vez)
            //else if (currentEquipedObject == null)
            //{
            //    Debug.Log("Caso 3? nada equipado en consumables, equipando");
            //    avatarController.EquipStoreSlot(newStoreObject);
            //}
        }
        selectedItemUI = itemUI;
    }

    void DisplayItemInPreview(FMStoreObject storeObt) {
        storeObt.transform.parent = itemPreview.transform;
        storeObt.transform.localPosition = Vector3.zero;

        //weird hot fix to make them rotate on center
        storeObt.ApplyRotationOffset();

    }

    public void BuyCA()
    {
        selectedCurrency = FMCurrencyType.CA;
        HasEnoughCurrency();
    }
    public void BuyDP()
    {
        selectedCurrency = FMCurrencyType.PC;
        HasEnoughCurrency();
    }

    public void HasEnoughCurrency()
    {
        string currency = selectedCurrency.ToString();
        uint price;
        //bool hasCurrency = selectedEquipItem.Item.VirtualCurrencyPrices.TryGetValue(cType, out price);
        int currentMoney = currency.Equals(FMCurrencyType.CA.ToString()) ? FMClientSessionData.Instance.currencyCA : FMClientSessionData.Instance.currencyPC;
        bool hasCurrency;

        //hasCurrency = selectedItemUI.Item.VirtualCurrencyPrices.TryGetValue(currency, out price);
        hasCurrency = currency.Equals(FMCurrencyType.CA.ToString()) ? selectedItemUI.CA > 0: selectedItemUI.PC > 0;
        price = currency.Equals(FMCurrencyType.CA.ToString()) ? (uint)selectedItemUI.CA: (uint)selectedItemUI.PC;

        if (hasCurrency && currentMoney >= price)
        {
            //uiOvers.goToConfirmation();
            PurchaseSelectedItem(price);
        }
        else
        if (hasCurrency && currentMoney < price)
        {
            Debug.Log("FMSTORE: Error al comprar, saldo insuficiente");
            //TODO add "go to premium currency" dialog, ohno for now
            //Cuando no hay saldo suficiente se envia a la seccion Premium
            //y se muestra el dialogo "OhNo"
            //Asi cuando el usuario apreta "volver", ya estEen la pantalla de premium

            //if (currency.Equals("SI"))
            //{
            //    uiSectionController.goToSection(6);
            //    uiOvers.goToOhNo();
            //}
            //else
            //{
            //    uiSectionController.goToSection(6);
            //    uiOvers.goToOhNo();
            //}
        }
    }

    void PurchaseSelectedItem(uint price)
    {
        string cType = selectedCurrency.ToString();
        //uint price;
        //bool hasCurrency = selectedEquipItem.Item.VirtualCurrencyPrices.TryGetValue(cType, out price);
        //int currentMoney = cType.Equals("SI") ? ClientSessionData.Instance.currencySI : ClientSessionData.Instance.currencyPC;
        //bool hasCurrency;

        //selectedItemUI.Item.VirtualCurrencyPrices.TryGetValue(cType, out price);
        if (cType.Equals(FMCurrencyType.CA.ToString()))
        {
            FMClientSessionData.Instance.currencyCA -= (int)price;
        }
        else if (cType.Equals(FMCurrencyType.PC.ToString()))
        {
            FMClientSessionData.Instance.currencyPC -= (int)price;
        }
        CatalogItem cItem = FMPlayFabInventory.GetCatalogItemFromID(selectedItemUI.ItemID);

        if (cItem != null) {
            PlayfabUtils.Instance.PurchaseItem(cItem, cType, OnPurchased, error => { OnPurchasedError(error); });
        }
    }

    void OnPurchasedError(PlayFabError error)
    {
        Debug.Log("FMSTORE: Error al comprar: " + error);
    }

    //TODO agregar evento para mostrar que ya se comprE(sonido, etc)
    void OnPurchased(PurchaseItemResult res)
    {
        //TODO Mostrar compra exitosa
        //uiOvers.goToBuy();
        Debug.Log("FMSTORE: ITEM PURCHASED (" + res.Items.Count + ") " + res.Items[0].DisplayName);

        //updating UI
        CAlab.text = FMClientSessionData.Instance.currencyCA.ToString();
        PClab.text = FMClientSessionData.Instance.currencyPC.ToString();

        //Update price UI
        //selectedItemUI.SetButtonPurchased(currencyCA);
        //selectedItemUI.SetButtonPurchased(currencyPC);

        AddItemToInventory(res.Items[0]);

        //Si es ticket se agrega al inventario del cliente y ademas se agrega la custom data al ticket del cliente
        //SIPlayfabUtils.Instance.InitializeCustomTicket(res.Items[0].ItemInstanceId, result =>
        //{
        //    SIInventoryItem invItem = AddItemToInventory(res.Items[0]);
        //    if (JSON.Parse(result.FunctionResult.ToString())["status"] != null && JSON.Parse(result.FunctionResult.ToString())["status"].Equals("success"))
        //    {
        //        JSONNode userCustomJSONData = JSON.Parse(result.FunctionResult.ToString())["custom_data"];
        //        Debug.Log(userCustomJSONData);
        //        SIPlayFabInventory.UpdateTicketData(invItem, userCustomJSONData);
        //    }
        //}, error => { Debug.Log("SISTORE: Error al inicializar el ticket"); });
    }

    FMInventoryItem AddItemToInventory(ItemInstance itemInventory)
    {
        //add to inventory
        FMInventoryItem inviItem = FMClientSessionData.Instance.InventoryItems.Find(x => x.CatalogID.Equals(itemInventory.ItemId));

        //TODO CHECK ---- if there is already an instance id, we just add 1
        if (inviItem != null && inviItem.IsStackable)
        {
            inviItem.Amount += 1;
            Debug.Log("New amount of item: " + inviItem.Amount);
            //labOwned.text = inviItem.Amount.ToString();
            Debug.Log("FMSTORE: inventory new count (stack)" + FMClientSessionData.Instance.InventoryItems.Count);
            return null;
        }
        //

        //if the item is new or not stackable, we add it to the inventory
        CatalogItem cItem = FMClientSessionData.Instance.CatalogItems.Find(x => x.ItemId.Equals(itemInventory.ItemId));

        //TODO Tirar initializePurchasedItem a un nuevo metodo y llamar al dialogo
        //que pregunte al usuario si desea equipar el item
        bool equip = false;
        if (inviItem == null)
        {
            //06/02/22 add inventory item customdata (is_equipped) to server for the first time            
            inviItem = FMPlayFabInventory.AddInventoryItem(itemInventory, equip, FMClientSessionData.Instance.InventoryItems, cItem);
            Debug.Log("FMSTORE: inventory new count " + FMClientSessionData.Instance.InventoryItems.Count);
        }


        //Probando actualizacion de currentItem de slot recien comprado CHEQUEAR ESTO TAMBIEN porque no hemos agregado nada de avatarcontroller etc aun        
        //FMInventorySlot slotClient = FMClientSessionData.Instance.Slots.Find(x => x.SlotType.Equals(currentSlot));
        //slotClient.UpdateCurrentItem(cItem);
        return inviItem;
    }

    void setPriceButton(Button priceButton, string currency, CatalogItem cItem)
    {
        //if item was purchased
        if (FMPlayFabInventory.IsItemOnInventory(cItem) && !cItem.IsStackable)
        {
            priceButton.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Purchased";
            priceButton.interactable = false;
            return;
        }

        priceButton.interactable = true;

        uint priceValue;
        cItem.VirtualCurrencyPrices.TryGetValue(currency, out priceValue);
        priceButton.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "" + priceValue;

        //priceButton.GetComponentsInChildren<Image>()[2].color = hasSI ? new Color(0f, 1f, 0.96f) : new Color(0.858f, 0.831f, 0.011f);
    }

    //public void SetButtonPurchased(Button priceButton)
    //{
    //    if (!Item.IsStackable)
    //    {
    //        priceButton.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Comprado";
    //        priceButton.interactable = false;
    //    }

    //}

    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
