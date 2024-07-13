using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ground : MonoBehaviour, IObjectsParent
{
    private EquipableObjects[] items;
    public void setEquipableObject(EquipableObjects item)
    {
        items.Append(item);
    }

    public EquipableObjects GetEquipableObjects(string name) {

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getItemSO().name == name)
            {
                return items[i];
            }
        }
        return null;

    }

    public void clearEquipableObjects(string name) {

        for (int i = 0; i < items.Length; i++) {
            if (items[i].getItemSO().name == name)
            {
                items[i] = null; 
            }
         }
    
    }

    public bool hasEquipableObject() {
    
    return items != null && items.Length > 0;
    }

    public Transform GetTransform() { return transform; }
}
