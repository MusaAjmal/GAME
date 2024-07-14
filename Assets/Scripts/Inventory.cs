using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Globalization;
using System;

public class Inventory : MonoBehaviour
{

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedcallBack;

    public List<ItemSO> items = new List<ItemSO>();
    public ItemSO defaultItem;

    public static Inventory Instance;
    public void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {

            Inventory.Instance.CycleItems();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            Inventory.Instance.RemoveItem(Inventory.Instance.defaultItem);
        }
    }
    public void DisplayItems()
    {
        Debug.Log("Displaying Inventory Items:");
        foreach (var item in items)
        {
            Debug.Log("Item: " + item.objectName + ", Quantity: " + item.objectCount);
        }
    }

    public void add(ItemSO item)
    {

        if (item.IsEquipable)
        {
            if (items.Contains(item))
            {
                IncrementItem(item.objectName);
                OnItemChangedcallBack?.Invoke();
                Debug.Log("ADDED ITEM " + item.objectName);
            }
            else
            {
                items.Add(item);
                OnItemChangedcallBack?.Invoke();
                Debug.Log("ADDED ITEM " + item.objectName);
            }
        }
       
        
    }

    private void IncrementItem(string name)
    {

        ItemSO existingItem = items.Find(itemso => itemso.objectName == name);

        if (existingItem != null)
        {
            existingItem.objectCount += 1;
            Debug.Log("Modified item: " + existingItem.objectName + " to quantity " + existingItem.objectCount);
        }
        else
        {
            Debug.LogWarning("Item not found: " + name);
        }
    }

    public void RemoveItem(ItemSO item)
    {
        if (items!= null)
        {
            ItemSO existingItem = items.Find(itemso => itemso.objectName == item.objectName);
            if (existingItem != null)
            {
                // Decrement the quantity
                existingItem.objectCount--;
                OnItemChangedcallBack?.Invoke();

                // If the quantity becomes zero, remove the item from the list
                if (existingItem.objectCount <= 0)
                {
                    items.Remove(existingItem);
                    OnItemChangedcallBack?.Invoke();
                    Debug.Log("Removed item: " + item.objectName);
                }
                else
                {
                    Debug.Log("Decremented item: " + item.objectName + " to quantity " + existingItem.objectCount);
                }
            }
        }
        

        
        else
        {
            Debug.LogWarning("Item not found: " + item.objectName);
        }
        
    }
    public void CycleItems()
    {
        if (defaultItem == null && items.Count > 0)
        {
            defaultItem = items[0]; // Set the default item to the first item in the list
            Debug.Log("Set default item to: " + defaultItem.objectName);
        }
        else if (items.Count > 1)
        {
            int currentIndex = items.IndexOf(defaultItem);
            int nextIndex = (currentIndex + 1) % items.Count; // Get the next item index, wrapping around using modulus
            defaultItem = items[nextIndex];
            Debug.Log("Set default item to next in cycle: " + defaultItem.objectName);
        }
        else if(items.Count == 1)
        {
            defaultItem = items[0];
            Debug.Log("Set default item to: " + defaultItem.objectName);
        }
        else
        {
            Debug.LogWarning("Inventory does not contain enough items to cycle.");
        }
       
    }

}


