using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableObjects : MonoBehaviour
{
    [SerializeField] private InventoryItemsSO itemSO;
    private IObjectsParent parent;

    public InventoryItemsSO getItems()
    {return itemSO; }



    public void setItemParent(IObjectsParent parent)
    {
        if(this.parent!= null)
        {
            this.parent.clearEquipableObjects(this.name);
        }

        this.parent = parent;
        parent.setEquipableObject(this);
        transform.parent = parent.GetTransform();

    }

    public InventoryItemsSO getItemSO()
    {
        return itemSO;
    }


}
