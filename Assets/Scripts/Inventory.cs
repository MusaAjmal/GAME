using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{

    public string equippedItem;

    [SerializeField] private Dictionary<string, int> items = new Dictionary<string, int>();
    

    public static Inventory Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
       
            equippedItem = GetFirstItem();
           Debug.Log("HI MOM " + equippedItem);
        
        
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
    public Dictionary<string, int> SortItems()
    {
        // Convert dictionary to a list of key-value pairs
        List<KeyValuePair<string, int>> sortedItemsList = new List<KeyValuePair<string, int>>(items);

        // Sort the list based on value first, then key
        sortedItemsList.Sort((pair1, pair2) =>
        {
            int countComparison = pair1.Value.CompareTo(pair2.Value);
            if (countComparison == 0)
            {
                return pair1.Key.CompareTo(pair2.Key);
            }
            return countComparison;
        });

        // Convert the sorted list back to a dictionary
        Dictionary<string, int> sortedItems = new Dictionary<string, int>();
        foreach (var pair in sortedItemsList)
        {
            sortedItems.Add(pair.Key, pair.Value);
        }

        return sortedItems;
    }

    public string GetFirstItem()
    {
        if (items.Count > 0)
        {
            var enumerator = items.GetEnumerator();
            enumerator.MoveNext(); // Move to the first element
            var firstItem = enumerator.Current;
            Debug.Log("HI MOM !" + firstItem.Key);
            return firstItem.Key;
        }
        return "No items in inventory.";
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
        items = SortItems();
    }
    public bool HasItem(string itemName)
    {
        return items.ContainsKey(itemName);
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
