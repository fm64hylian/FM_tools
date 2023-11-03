using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMStoreObjectManager : MonoBehaviour
{
    /// <summary>
    /// Clase que se encagara de manejar el modelo 3D del objeto y sus parametros
    /// </summary>
    /// 
    public List<FMStoreObject> StoreObjects = new List<FMStoreObject>();


    public void SetData(List<CatalogItem> items)
    {
        //TODO confirmar que todos los catalogItems serán instanciados en el pool de items
        List<CatalogItem> _models = items; //.FindAll(x => x.ItemClass.Equals("Equipment"));
        //SIStoreEquipmentObject
        for (int i = 0; i < _models.Count; i++)
        {
            CatalogItem cItem = _models[i];
            GameObject itemPrefab = Instantiate(Resources.Load("Prefabs/FMStoreObject")) as GameObject;
            //Debug.Log(itemPrefab);
            FMStoreObject itemObject = itemPrefab.GetComponent<FMStoreObject>();
            //Debug.Log(itemObject);

            string iId = cItem.ItemId;
            string iName = cItem.DisplayName;
            string iclass = cItem.ItemClass;
            string iImageURL = cItem.ItemImageUrl;

            itemObject.SetData(iId, iName, iclass, iImageURL, transform);
            StoreObjects.Add(itemObject);
            SetAllChildrenParentLayer(itemPrefab.transform);

            //itemObject.transform.SetParent(transform);
            //itemObject.transform.localPosition = Vector3.zero;
            //2nd shoe
            //if (FMPlayFabInventory.GetSlotType(_models[i]) == FMEquipmentSlotsType.Foot_Shoes)
            //{
            //    GameObject itemPrefabshoe = Instantiate(Resources.Load("SIStoreEquipmentPrefab")) as GameObject;
            //    SIStoreEquipmentObject itemObject2 = itemPrefabshoe.GetComponent<SIStoreEquipmentObject>();
            //    itemObject2.SetData(_models[i], transform);
            //    StoreObjects.Add(itemObject2);
            //}
        }

        void SetAllChildrenParentLayer(Transform trans)
        {
            foreach (Transform child in trans)
            {
                child.gameObject.layer = gameObject.layer;
                SetAllChildrenParentLayer(child);
            }
        }
    }

    public FMStoreObject GetItem(CatalogItem _item)
    {
        for (int i = 0; i < StoreObjects.Count; i++)
        {
            FMStoreObject SIitem = StoreObjects[i];
            if (SIitem.ItemID.Equals(_item.ItemId))
            {
                return SIitem;
            }
        }
        Debug.Log("Objeto no encontrado");
        return null;
    }

    public void ResetItem(FMStoreObject _item)
    {
        //if (FMPlayFabInventory.GetSlotType(_item.ItemModel) == FMEquipmentSlotsType.Foot_Shoes)
        //{
        //    FMStoreEquipmentObject[] shoes = GetShoes(_item.ItemModel);
        //    shoes[0].transform.SetParent(transform);
        //    shoes[0].transform.localPosition = Vector3.zero;
        //    shoes[0].ResetRotation();

        //    shoes[1].transform.SetParent(transform);
        //    shoes[1].transform.localPosition = Vector3.zero;
        //    shoes[1].ResetRotation();
        //    return;
        //}

        _item.transform.SetParent(transform);
        _item.transform.localPosition = Vector3.zero;
        _item.ResetRotation();
    }
}
