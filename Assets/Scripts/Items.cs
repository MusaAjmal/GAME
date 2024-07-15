using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;
public class Items : MonoBehaviour
{
    [SerializeField] public ItemSO itemSO;
   
   

    private void pickup()
    {
        if (itemSO.IsEquipable)
        {
           
            Inventory.Instance.AddItem(itemSO);
            


            Destroy(gameObject); //instead of destroying we change the parent of the transform
           
        }
       
    }
    private void Start()
    {
        


    }



    private void Update()
    {
        
        if (Input.GetMouseButtonDown(2)) // 2 corresponds to the middle mouse button
        {
            OnMiddleMouseDown();
        }
       
    }


    private void OnMiddleMouseDown()
    {
        if(Vector3.Distance(Player.Instance.GetPosition(), transform.position)  < Player.Instance.pickupDistance){
            pickup();
        }
    
    }

    public override bool Equals(object obj)
    {
        if (obj is Items)
        {
            return this.itemSO.objectName.Equals(((Items)obj).itemSO.objectName, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return itemSO.objectName.ToLower().GetHashCode();
    }
}
