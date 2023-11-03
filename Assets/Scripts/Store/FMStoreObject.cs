using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMStoreObject : MonoBehaviour
{
    public string ItemID;
    public string DisplayName;
    public string ItemClass;
    public string ImageURL;
    public FMEquipmentSlotsType Slot;

    Vector3 OriginalRotation;
    GameObject itemMesh;

    public void SetData(string itemID, string displayName, string itemClass, string imageurl, Transform parent)
    {
        ItemID = itemID;
        DisplayName = displayName;
        ItemClass = itemClass;
        ImageURL = imageurl;
        Slot = FMPlayFabInventory.GetSlotType(FMPlayFabInventory.GetCatalogItemFromID(ItemID));
        //Debug.Log("3DModels/Inventory/" + ImageURL + itemID);

        //if (!ItemClass.Equals("Equipment"))
        //{
        itemMesh = Instantiate(Resources.Load("3DModels/Inventory/" + ImageURL+ ItemID)) as GameObject;
        //Debug.Log(itemMesh);
        itemMesh.transform.SetParent(transform);
        itemMesh.transform.localPosition = Vector3.zero;
        OriginalRotation = transform.localRotation.eulerAngles;
        //}

        //TODO add other types
        //else {
        //    DisplayName = "None";
        //}

        //if _model == null it's "NONE"        
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }

    public void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
        transform.localRotation = Quaternion.Euler(OriginalRotation);
    }
    /// <summary>
    /// apply small offset to mesh to spin correctly
    /// </summary>
    public void ApplyRotationOffset() {

        switch (Slot) {
            case FMEquipmentSlotsType.UBody_HandL:
            case FMEquipmentSlotsType.UBody_HandR:
                itemMesh.transform.localPosition = (Vector3.forward * 0.255f) + Vector3.right * 0.05f;
                break;
            case FMEquipmentSlotsType.Head_Hair:
                itemMesh.transform.localPosition = (Vector3.right * 0.1f);
                break;
        }

    }

}
