using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMInventoryObject : MonoBehaviour
{
    public FMInventoryItem ItemModel;
    //public bool IsEquiped = false;
    public string DisplayName;

    Vector3 OriginalRotation;

    public void SetData(FMInventoryItem _model, Transform parent)
    {
        if (_model != null)
        {
            ItemModel = _model;
            DisplayName = _model.DisplayName;

            //skins do not have a physical object for now
            //TODO confirmar que solo ocurra cuando no sea equipment
            if (!_model.ItemClass.Equals("Equipment"))
            {
                GameObject itemMesh = Instantiate(Resources.Load("Inventory/" + _model.SpriteName)) as GameObject;
                itemMesh.transform.SetParent(transform);
                itemMesh.transform.localPosition = Vector3.zero;
                OriginalRotation = transform.localRotation.eulerAngles;
            }

        }
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
}
