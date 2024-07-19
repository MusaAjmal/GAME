using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;
public class Items : MonoBehaviour
{
    [SerializeField] public ItemSO itemSO;
    public bool isOn;
   

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
        isOn = true;


    }



    private void Update()
    {
        
        if (Input.GetMouseButtonDown(2)) // 2 corresponds to the middle mouse button
        {
            OnMiddleMouseDown();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerToggleItem();
        }
       
    }
    private void toggle()
    {
        if (itemSO.canbeToggled)
        {
            if (isActive()) {
                isOn = false;
            }
            else
            {
                isOn = true; 
            }
           
        }
    }
    public bool isActive()
    {
        return isOn;
    }
    private void playerToggleItem()
    {
        if (Vector3.Distance(Player.Instance.GetPosition(), transform.position) < Player.Instance.pickupDistance)
        {
            
            toggle();
            Debug.Log("Torch State: " + isOn);

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
