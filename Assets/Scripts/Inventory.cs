using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Analytics;
using Unity.VisualScripting.Dependencies.Sqlite;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedcallBack;

    public delegate void OnItemRemoved();
    public OnItemRemoved OnItemRemovedcallBack;

    public Stack<ItemSO> stoneStack = new Stack<ItemSO>();
    public Stack<ItemSO> otherItemsStack = new Stack<ItemSO>();

    public ItemSO defaultItem;

    public static Inventory Instance;

    public void Awake()
    {
        Instance = this;
    }
    private void setDefaultItem()
    {
        List<ItemSO> allItems = GetAllItems();
        if (defaultItem == null && allItems.Count > 0) 
        {
            defaultItem = allItems[0];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Inventory.Instance.CycleItems();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Instance.defaultItem != null) {
                Inventory.Instance.RemoveItem(Inventory.Instance.defaultItem);
            }
        }
            
        setDefaultItem();
    }

    public void DisplayItems()
    {
        Debug.Log("Displaying Stone Stack Items:");
        foreach (var item in stoneStack)
        {
            Debug.Log("Item: " + item.objectName);
        }

        Debug.Log("Displaying Other Items Stack:");
        foreach (var item in otherItemsStack)
        {
            Debug.Log("Item: " + item.objectName);
        }
    }

    public void AddItem(ItemSO item)
    {
        if (item.IsEquipable)
        {
            if (item.objectName == "Stone")
            {
                stoneStack.Push(item);
                OnItemChangedcallBack?.Invoke();
                Debug.Log("Added stone item: " + item.objectName);
            }
            else
            {
                otherItemsStack.Push(item);
                OnItemChangedcallBack?.Invoke();
                Debug.Log("Added other item: " + item.objectName);
            }
        }
    }

    public void RemoveItem(ItemSO item)
    {
        if (item.objectName == "Stone" && stoneStack.Count > 0)
        {
            ItemSO removedItem = stoneStack.Pop();
            if (removedItem == defaultItem && stoneStack.Count == 0)
            {
                defaultItem = null;
                CycleItems();// Reset defaultItem if it's removed
            }
            OnItemRemovedcallBack?.Invoke();
            Debug.Log("Removed stone item: " + removedItem.objectName);
        }
        else if (otherItemsStack.Contains(item))
        {
            ItemSO removedItem = otherItemsStack.Pop();
            if (removedItem == defaultItem && otherItemsStack.Count== 0)
            {
                defaultItem = null;
                CycleItems();// Reset defaultItem if it's removed
            }
            OnItemRemovedcallBack?.Invoke();
            Debug.Log("Removed other item: " + removedItem.objectName);
        }
        else
        {
            Debug.LogWarning("Item not found or stack is empty: " + item.objectName);
        }
    }

    public void DisplayList(List<ItemSO> allItems)
    {
        
        foreach (ItemSO item in allItems)
        {
            Debug.Log(item.objectName);
        }
    }

    public void CycleItems()
    {
        List<ItemSO> allItems = GetAllItems();
        List<ItemSO> uniqueItems = allItems
            .GroupBy(item => item.objectName)
            .Select(group => group.First())
            .ToList();

        if (defaultItem == null && uniqueItems.Count > 0)
        {
            defaultItem = uniqueItems[0];
            Debug.Log("Set default item to: " + defaultItem.objectName);
            return;
        }

        if (uniqueItems.Contains(defaultItem))
        {
            int currentIndex = -1;

            // Manually find the index of defaultItem
            for (int i = 0; i < uniqueItems.Count; i++)
            {
                if (uniqueItems[i].objectName.Equals(defaultItem.objectName))
                {
                    currentIndex = i;
                    break;
                }
            }

            // If the item was found in the list, cycle to the next item
            if (currentIndex != -1)
            {
                int nextIndex = (currentIndex + 1) % uniqueItems.Count;
                defaultItem = uniqueItems[nextIndex];
                Debug.Log("Cycled to next item: " + defaultItem.objectName);
            }
        }
    }


    public List<ItemSO> GetAllItems()
    {
        List<ItemSO> allItems = new List<ItemSO>();
        allItems.AddRange(stoneStack);
        allItems.AddRange(otherItemsStack);
        return allItems;
    }
}
