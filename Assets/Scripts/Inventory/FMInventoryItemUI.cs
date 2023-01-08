using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FMInventoryItemUI : MonoBehaviour
{
    [SerializeField]
    Image itemImage;
    [SerializeField]
    TextMeshProUGUI itemNameLab;
    [SerializeField]
    Image isEquippedImg;


    public Action<FMInventoryItemUI> OnSelected;
    bool isSelected = false;
    void Start()
    {

    }

    public void SetData(string imageUrl, string itemName)
    {
        itemNameLab.text = itemName;
        Sprite sprite = Resources.Load<Sprite>("Textures/Inventory/" + imageUrl);
        itemImage.sprite = sprite;

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
