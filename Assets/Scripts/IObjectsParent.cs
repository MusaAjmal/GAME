using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectsParent 
{
    public void setEquipableObject(EquipableObjects item);

    public EquipableObjects GetEquipableObjects(string name);

    public void clearEquipableObjects(string name);

    public bool hasEquipableObject();

    public Transform GetTransform();

}
