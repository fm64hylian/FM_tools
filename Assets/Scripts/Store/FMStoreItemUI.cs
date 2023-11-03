using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FMStoreItemUI : MonoBehaviour
{
    [SerializeField]
    Image itemImage;
    [SerializeField]
    TextMeshProUGUI priceCA;
    [SerializeField]
    TextMeshProUGUI pricePC;

    public Action<FMStoreItemUI> OnSelected;
    
    public string ItemID;
    public string Title;
    public string Description;
    public string ImageUrl;
    public int CA = -1;
    public int PC = -1;
    bool isSelected = false;

    void Start()
    {
        
    }

    public void SetData(string itemID, string title, string description, string imageUrl, uint caPrice, uint pcPrice) {
        ItemID = itemID;
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        CA = (int)caPrice;
        PC = (int)pcPrice;

        Sprite sprite = Resources.Load<Sprite>("Textures/Inventory/" + ImageUrl+ itemID);
        itemImage.sprite = sprite;

        bool hasCA = CA > 0;
        bool hasPC = PC > 0;

        priceCA.text = hasCA ? CA.ToString() : "--";
        pricePC.text = hasPC ? PC.ToString() : "--";
    }

    public void OnItemSelected()
    {
        if (OnSelected != null)
        {
            isSelected = true;
            OnSelected(this);
        }
    }

    public void Deselect()
    {
        isSelected = false;
        //TODO remove selection in UI
    }

    //public bool IsEquipment()
    //{
    //    return Item.ItemClass.Equals("Equipment");
    //}
}