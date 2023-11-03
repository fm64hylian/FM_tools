using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMAvatarSlot : MonoBehaviour
{
    public FMInventoryObject CurrentInventoryItem;
    public FMInventorySlot InventorySlotModel;
    public FMEquipmentSlotsType SlotType;


    public void EquipInventory(FMInventoryObject _item, EquipMode equipMode)
    {
        Debug.Log("AVATARSLOT Defining Current inventory: " + _item.DisplayName);
        CurrentInventoryItem = _item;
        _item.transform.SetParent(transform);
        _item.transform.localPosition = Vector3.zero;
        _item.transform.localRotation = Quaternion.identity;
        Debug.Log("AVATARSLOT " + equipMode + "-Equipando " + _item.DisplayName + ", pos: " + transform.position);

        if (equipMode.Equals(EquipMode.Preview)) return;
        CurrentInventoryItem.ItemModel.IsEquipped = true;
        Debug.Log("AVATARSLOT - " + CurrentInventoryItem.ItemModel.DisplayName + " isEquipped = " + CurrentInventoryItem.ItemModel.IsEquipped);
        //model
        //InventorySlotModel.CurrentItem = CurrentInventoryItem.ItemModel.InstanceID;
        InventorySlotModel.UpdateCurrentItem(_item.ItemModel);
    }

    public void UnEquipInventory()
    {
        //TODO ESTA DESEQUIPANDO EL ITEM QUE SE ESTABA SELECCIONADO NO EL QUE SE APRETO EQUIPAR
        //Parece que CurrentInventoryItem tiene el item nuevo y Model tine el que queremos sobreescribir
        //Debug.Log("BDAVATARSLOT - Desequipando " + CurrentInventoryItem.DisplayName+" / model "+InventorySlotModel.CurrentItem);
        //if es equipment

        CurrentInventoryItem = null;


        //model
        InventorySlotModel.CurrentItem = "None";
    }

    public bool IsEquipedInventory()
    {
        return CurrentInventoryItem != null;
    }
}
