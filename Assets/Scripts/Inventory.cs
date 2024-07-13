using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{

    public string equippedItem = "Bone";

    [SerializeField] private Dictionary<string, int> items = new Dictionary<string, int>();
    

    public static Inventory Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }


    public void GetAllItems()
    {
        foreach (KeyValuePair<string, int> item in items)
        {
            Debug.Log(item.Key + " x" + item.Value);
        }
    }

    public bool GetItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            int count = items[itemName];
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetItem(string itemName, int count)
    {
        items[itemName] = count;
        Debug.Log("Set " + itemName + " x" + count);
    }

    public void EquipItem(string itemName) 
    {
        equippedItem = itemName;
    }
    

   
    public string GetEquipedItem()
    {
        return equippedItem;
    }

    public void UseItem()
    {

       
        if (items[equippedItem] > 1)
        {
            int count = items[equippedItem];
     
                count--;
            if(items[equippedItem] == 0)  //remove that item from dictionary
            {
                items.Remove(equippedItem);
            }
                items[equippedItem] = count;
                Debug.Log("Used " + equippedItem + " x1");
        }
        else
        {
            Debug.Log("Item " + equippedItem + " not found in inventory");
        }
        
    }
   

    public void AddItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName]++;
        }
        else
        {
            items[itemName] = 1;
        }
        Debug.Log("Added " + itemName + " x1");
    }
}
