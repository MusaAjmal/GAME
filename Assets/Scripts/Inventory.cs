using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

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
        Debug.Log("Displaying Stone Stack Items:");
        foreach (var item in stoneStack)
        {
            Debug.Log("Item: " + item.objectName + ", Count: " + item.objectCount);
        }

        Debug.Log("Displaying Other Items Stack:");
        foreach (var item in otherItemsStack)
        {
            Debug.Log("Item: " + item.objectName + ", Count: " + item.objectCount);
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
            OnItemRemovedcallBack?.Invoke();
            Debug.Log("Removed stone item: " + removedItem.objectName);
        }
        else if (otherItemsStack.Count > 0)
        {
            ItemSO removedItem = otherItemsStack.Pop();
            OnItemRemovedcallBack?.Invoke();
            Debug.Log("Removed other item: " + removedItem.objectName);
        }
        else
        {
            Debug.LogWarning("Item not found or stack is empty: " + item.objectName);
        }
    }

    public void CycleItems()
    {
        Stack<ItemSO> tempStack = new Stack<ItemSO>();

        if (defaultItem == null && (stoneStack.Count > 0 || otherItemsStack.Count > 0))
        {
            defaultItem = stoneStack.Count > 0 ? stoneStack.Peek() : otherItemsStack.Peek();
            Debug.Log("Set default item to: " + defaultItem.objectName);
            return;
        }

        if (stoneStack.Contains(defaultItem))
        {
            while (stoneStack.Count > 0)
            {
                ItemSO item = stoneStack.Pop();
                tempStack.Push(item);
                if (item == defaultItem)
                {
                    break;
                }
            }

            while (tempStack.Count > 0)
            {
                stoneStack.Push(tempStack.Pop());
            }

            if (stoneStack.Count > 0)
            {
                defaultItem = stoneStack.Peek();
            }
            else if (otherItemsStack.Count > 0)
            {
                defaultItem = otherItemsStack.Peek();
            }
        }
        else if (otherItemsStack.Contains(defaultItem))
        {
            while (otherItemsStack.Count > 0)
            {
                ItemSO item = otherItemsStack.Pop();
                tempStack.Push(item);
                if (item == defaultItem)
                {
                    break;
                }
            }

            while (tempStack.Count > 0)
            {
                otherItemsStack.Push(tempStack.Pop());
            }

            if (otherItemsStack.Count > 0)
            {
                defaultItem = otherItemsStack.Peek();
            }
            else if (stoneStack.Count > 0)
            {
                defaultItem = stoneStack.Peek();
            }
        }

        Debug.Log("Set default item to next in cycle: " + defaultItem.objectName);
    }

    public List<ItemSO> GetAllItems()
    {
        List<ItemSO> allItems = new List<ItemSO>();
        allItems.AddRange(stoneStack);
        allItems.AddRange(otherItemsStack);
        return allItems;
    }
}
