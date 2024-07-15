using System.Collections.Generic;

using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory Inventory;
    public Transform objectsParent;
    public InventorySlot[] inventorySlots;
    public bool isStoneSet;
    public bool isBoneSet;
    public int iterator;
    public ItemSO boneItem;
    public ItemSO stoneItem;

    public void Start()
    {
        Inventory = Inventory.Instance;
        isBoneSet = false;
        isStoneSet = false;
        iterator = 0;
        Inventory.OnItemChangedcallBack += UpdateUIonEquip;
        Inventory.OnItemRemovedcallBack += UpdateUIonRemove;

        inventorySlots = objectsParent.GetComponentsInChildren<InventorySlot>();
    }

    private void UpdateUIonRemove() // Remove Item
    {
        List<ItemSO> allItems = Inventory.GetAllItems();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < allItems.Count)
            {
                ItemSO currentItem = allItems[i];
                InventorySlot slot = inventorySlots[i];

                if (slot.item != null && slot.item.objectName == currentItem.objectName)
                {
                    // Decrement the item count
                    if (currentItem.objectCount > 0)
                    {
                        currentItem.objectCount--;
                        Debug.Log("Decremented item count: " + currentItem.objectName + " to " + currentItem.objectCount);

                        if (currentItem.objectCount == 0)
                        {
                            // Remove the item from the slot if count reaches zero
                            slot.Clear();
                            Debug.Log("Removed item from inventory: " + currentItem.objectName);
                        }
                        else
                        {
                            // Update the slot count
                            slot.IncrementCount();
                        }
                    }
                }
            }
            else
            {
                inventorySlots[i].Clear();
            }
        }
    }

    public void Update()
    {

    }

    private void UpdateUIonEquip()  // Add Item
    {
        List<ItemSO> allItems = Inventory.GetAllItems();
        int stoneCount = Inventory.Instance.stoneStack.Count;
        int boneCount = Inventory.Instance.otherItemsStack.Count;
        Debug.Log("NUMBER OF STONES = " + stoneCount);
        Debug.Log("NUMBER OF BONES = " + boneCount);
       for(int i = 0; i<=1; i++) {
            if (inventorySlots[i].item != null)
            {
                if (inventorySlots[i].item.objectName == "Stone")
                {
                    inventorySlots[i].text.text = stoneCount.ToString();
                }
                if (inventorySlots[i].item.objectName == "Bone")
                {
                    inventorySlots[i].text.text = boneCount.ToString();
                }
            }
        }
            
        
         if (inventorySlots[iterator].item == null) 
        {
            
                {
                    for (int j = 0; j < allItems.Count; j++)
                    {
                        if (allItems[j].objectName == "Stone")
                        {
                            stoneItem = allItems[j];

                        }
                        if (allItems[j].objectName == "Bone")
                        {
                            boneItem = allItems[j];

                        }
                    }
                    if (stoneItem != null && !isStoneSet)
                    {
                        inventorySlots[iterator].addItem(stoneItem);
                        isStoneSet = true;
                        iterator++;
                    }
                    if (boneItem != null && !isBoneSet)
                    {
                        inventorySlots[iterator].addItem(boneItem);
                        isBoneSet = true;
                        iterator++;

                    }
                }
            }
        }
    }



