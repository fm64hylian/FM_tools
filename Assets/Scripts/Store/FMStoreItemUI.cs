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
    bool isSelected = false;
    void Start()
    {
        
    }

    public void SetData(string imageUrl, int caPrice, int pcPrice) {
        Sprite sprite = Resources.Load<Sprite>("Textures/Inventory/" + imageUrl);
        itemImage.sprite = sprite;

        bool hasCA = caPrice > 0;
        bool hasPC = pcPrice > 0;

        priceCA.text = hasCA ? caPrice.ToString() : "--";
        pricePC.text = hasPC ? caPrice.ToString() : "--";
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
