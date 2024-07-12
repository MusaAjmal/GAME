using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    private static string equippedItem;

    [SerializeField] private Dictionary<string, int> items = new Dictionary<string, int>();

    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<Inventory>();
                    singletonObject.name = "Inventory (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    public void GetAllItems()
    {
        foreach (KeyValuePair<string, int> item in items)
        {
            Debug.Log(item.Key + " x" + item.Value);
        }
    }

    public string GetItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            int count = items[itemName];
            return  "Got " + itemName + " x" + count;
        }
        else
        {
            return "Item " + itemName + " not found in inventory";
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
