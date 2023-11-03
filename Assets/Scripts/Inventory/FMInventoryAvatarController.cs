using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FMEquipmentSlotsType
{
    None,
    //HeadGear
    Head_Hair,
    Head_UpperFace,
    Head_LowerFace,
    //UpperBody
    UBody_Clothes,
    UBody_HandL,
    UBody_HandR,
    Skin_Color
}



public enum EquipMode
{
    Preview,
    Save
}

public class FMInventorySlot : MonoBehaviour
{

    public string CurrentItem = "None";
    public FMEquipmentSlotsType SlotType = FMEquipmentSlotsType.None;

    /// <summary>
    /// Esto se llama cuando se está equipando o comprando (No al asignarse a none y desequiparse)
    /// </summary>
    /// <param name="_item"></param>
    //public void UpdateCurrentItem(CatalogItem _item)
    //{
    //    FMInventoryItem invItem = FMPlayFabInventory.GetInventoryItemFromCatalogID(_item);

    //    //Comprobar si el item se encuentra en el inventario
    //    bool isInInventory = FMPlayFabInventory.IsItemOnInventory(_item);

    //    string instanceId = invItem != null ? invItem.InstanceID : "";
    //    //si parte con "Store", significa que no ha sido comprado aun
    //    CurrentItem = isInInventory ? instanceId : "Store_" + _item.ItemId;
    //    FMInventorySlot clientSlot = FMClientSessionData.Instance.Slots.Find(x => x.SlotType.Equals(SlotType));
    //    clientSlot.CurrentItem = CurrentItem;
    //}

    public void UpdateCurrentItem(FMInventoryItem _item)
    {
        CurrentItem = _item.InstanceID;
        FMInventorySlot clientSlot = FMClientSessionData.Instance.Slots.Find(x => x.SlotType.Equals(SlotType));
        clientSlot.CurrentItem = CurrentItem;
    }
}





public class FMInventoryAvatarController : MonoBehaviour
{
    [SerializeField]
    Transform[] SlotsPositions;

    [SerializeField]
    SkinnedMeshRenderer FaceMat;

    public List<FMAvatarSlot> AvatarSlots = new List<FMAvatarSlot>();

    List<Material> triedMaterials = new List<Material>();
    Material currentFace;

    /// <summary>
    /// inicializa slots de avatar
    /// </summary>
    public void Init()
    {
        for (int i = 0; i < SlotsPositions.Length; i++)
        {
            FMAvatarSlot avatarSlot = SlotsPositions[i].GetComponent<FMAvatarSlot>();
            if (avatarSlot != null)
            {
                FMInventorySlot slotModel = FMClientSessionData.Instance.Slots.Find(x => x.SlotType.Equals(avatarSlot.SlotType));
                //Debug.Log("AVATARCONTROLLER COUNT: " + BDClientSessionData.Instance.Slots.Count);
                if (slotModel != null && !slotModel.SlotType.Equals(FMEquipmentSlotsType.None))
                {
                    //Debug.Log("AVATARCONTROLLER current: " + slotModel.CurrentItem);
                    //assign default_skin instanceID instead of "None" 
                    //if (slotModel.SlotType.Equals(FMEquipmentSlotsType.Skin_Color) && slotModel.CurrentItem.Equals("None"))
                    //{
                    //    slotModel.CurrentItem = FMPlayFabInventory.GetDefaultSkinInstanceId(FMEquipmentSlotsType.Skin_Color);
                    //}

                    avatarSlot.InventorySlotModel = slotModel;
                    AvatarSlots.Add(avatarSlot);
                }
                else
                {
                    slotModel = new FMInventorySlot();
                    avatarSlot.InventorySlotModel = slotModel;
                    AvatarSlots.Add(avatarSlot);
                }

            }
        }


        currentFace = FaceMat.materials[0];

        triedMaterials.Add(currentFace);
        //avatarSlot.Model = slotModel;
    }


    /// <summary>
    /// Esto solo ocurre al equipar o desequipar definitivamente (save)
    /// </summary>
    /// <param name="_item"></param>
    public void UnEquipInventorySlot(FMInventoryObject _item) ///BDInventoryObject
    {
        //Debug.Log("que wea es null ah "+ _item);
        if (_item.ItemModel.ItemClass.Equals("Equipment"))
        {
            //FMEquipmentSlotsType slotType = BDPlayFabInventory.GetSlotType(_item.ItemModel);
            FMAvatarSlot slot = AvatarSlots.Find(x => x.InventorySlotModel.SlotType.Equals(_item.ItemModel.SlotType));
            if (slot != null && (slot != null && _item.ItemModel.InstanceID != slot.CurrentInventoryItem.ItemModel.InstanceID))
            {
                ChangeSkin(_item.ItemModel.SpriteName, slot.SlotType);

                slot.UnEquipInventory();

            }
        }


    }


    public bool IsSlotEquiped(FMEquipmentSlotsType _slotType)
    {
        FMAvatarSlot slot = AvatarSlots.Find(x => x.InventorySlotModel.SlotType.Equals(_slotType));
        return slot != null && slot.IsEquipedInventory();
    }

    public FMInventoryObject GetInventoryItemFromSlot(FMEquipmentSlotsType _slotType)
    {
        //Debug.Log("on GetInventorySlot - selected slot "+_slotType);
        FMAvatarSlot slot = AvatarSlots.Find(x => x.InventorySlotModel.SlotType.Equals(_slotType));
        //NO ESTA DEVOLVIENDO NADA porque CurrentInventoryItem no tiene nada, Model.CurrentIte SI
        //Debug.Log("checking equipped slots, selected  slot "+slot);
        //AvatarSlots.ForEach(x => Debug.Log(x.InventorySlotModel.SlotType+" - "+x.InventorySlotModel.CurrentItem+" -(inv)- "+ x.CurrentInventoryItem));
        //Debug.Log("Item equipado encontrado en slot: " + slot.CurrentInventoryItem.DisplayName);
        return slot.CurrentInventoryItem;
    }



    //////////////////STORE ITEMS


    public FMAvatarSlot GetAvatarSlot(FMEquipmentSlotsType _slotType)
    {
        return AvatarSlots.Find(x => x.InventorySlotModel.SlotType.Equals(_slotType));
    }

    /// <summary>
    /// itemPath = catalogItem.ItemImageUrl
    /// </summary>
    /// <param name="itemPath"></param>
    void ChangeSkin(string itemPath, FMEquipmentSlotsType slotType)
    {
        //getting skin name (ItemImageUrl = "Equipment/SkinColor/skinName") + (Instance)
        string skinName = itemPath.Split('/')[2] + " (Instance)";
        //GameObject skinObject;
        Material newSkin;

        //if it hasn't been tried out yet, add to list
        if (triedMaterials.Find(x => x.name.Equals(skinName)) == null)
        {
            Debug.Log("Itempath: " + itemPath);
            newSkin = Instantiate(Resources.Load("Inventory/" + itemPath)) as Material;
            //newSkin=skinObject.GetComponent<MeshRenderer>().materials[0];
            Debug.Log("Asignando skin: " + newSkin.name);
            triedMaterials.Add(newSkin);
            //Destroy(skinObject);
        }
        else
        {
            newSkin = triedMaterials.Find(x => x.name.Equals(skinName));
        }

        //TODO arreglar aca
        switch (slotType)
        {
            case FMEquipmentSlotsType.Head_Hair:
                break;
            case FMEquipmentSlotsType.UBody_Clothes:
                break;
            case FMEquipmentSlotsType.Skin_Color:
                changeMat(FaceMat.materials, FaceMat, newSkin);
                break;
            default:
                break;
        }


        //else if (slotType.Equals(EquipmentSlotsType.Skin_Face)) {
        //    Material[] mats = FaceMat.materials;
        //    mats[0] = newSkin;
        //    FaceMat.materials = mats;
        //}

        //Debug.Log("ON CHANGE SKIN  new assigned - " + SkinMat.materials[0].name+ "len " + triedSkins.Count);
    }

    void changeMat(Material[] mats, MeshRenderer SkinMat, Material newSkin)
    {
        mats[0] = newSkin;
        SkinMat.materials = mats;
    }

    void changeMat(Material[] mats, SkinnedMeshRenderer SkinMat, Material newSkin)
    {
        mats[0] = newSkin;
        SkinMat.materials = mats;
    }
}
