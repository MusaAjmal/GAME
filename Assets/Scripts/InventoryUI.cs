using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;
using UnityEngine.UI;

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
    public Stack<ItemSO> stoneStack;
    public Stack<ItemSO> boneStack;
    int commonSlotforStone;
    int commonSlotforBone;
    public static InventoryUI instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    public void restoreItems(Inventory inventory)
    {
        Inventory = inventory;
    }
    public void addItem()
    {
        // First, set items from the inventory to keep track of current items.
        setItemsfromInventory();

        // Get the count of stones and bones in the inventory stacks.
        int stoneCount = Inventory.Instance.stoneStack.Count;
        int boneCount = Inventory.Instance.otherItemsStack.Count;

        // Check if there is an empty slot available.
        int emptySlot = CheckClearSlot();

        // Find slots containing stone or bone items, if they exist.
        commonSlotforStone = getCommonSlot("Stone");
        commonSlotforBone = getCommonSlot("Bone");

        // If the inventory is full (no empty slots available)
        if (emptySlot == -1)
        {
            if (commonSlotforStone != -1)
            {
                // Update the existing stone item slot with the current count.
                inventorySlots[commonSlotforStone].text.text = stoneCount.ToString();
            }
            if (commonSlotforBone != -1)
            {
                // Update the existing bone item slot with the current count.
                inventorySlots[commonSlotforBone].text.text = boneCount.ToString();
            }
        }
        else // There is at least one empty slot available.
        {
            // Handle cases where stone or bone items need to be added.
            if (stoneCount > 0 && commonSlotforStone == -1)
            {
                // Add stone to an empty slot if no common slot is found.
                inventorySlots[emptySlot].addItem(stoneItem);
                inventorySlots[emptySlot].text.text = stoneCount.ToString();
            }
            else if (commonSlotforStone != -1)
            {
                // Update stone count if the common slot already exists.
                inventorySlots[commonSlotforStone].text.text = stoneCount.ToString();
            }

            // Move to next empty slot for the next item type.
            emptySlot = CheckClearSlot();

            if (boneCount > 0 && commonSlotforBone == -1)
            {
                // Add bone to an empty slot if no common slot is found.
                inventorySlots[emptySlot].addItem(boneItem);
                inventorySlots[emptySlot].text.text = boneCount.ToString();
            }
            else if (commonSlotforBone != -1)
            {
                // Update bone count if the common slot already exists.
                inventorySlots[commonSlotforBone].text.text = boneCount.ToString();
            }
        }
    }

    public int CheckClearSlot()
    {
        // Iterate over the inventory slots
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Check if the item in the slot is null (i.e., the slot is empty)
            if (inventorySlots[i].item == null)
            {
                // Return the index of the first empty slot found
                return i;
            }
        }
        // Return -1 if no empty slot is found
        return -1;
    }

    public int getCommonSlot(string name)
    {

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item != null)
            {
                if (inventorySlots[i].item.objectName == name)
                {
                    return i;
                }
            }

        }
        return -1;
    }
    public void setItemsfromInventory()
    {
        List<ItemSO> allItems = Inventory.GetAllItems();

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

    }
    /* public void removeItem()
     {
         // First, set items from the inventory to keep track of current items.
         setItemsfromInventory();

         // Get the count of stones and bones in the inventory stacks.
         int stoneCount = Inventory.Instance.stoneStack.Count;
         int boneCount = Inventory.Instance.otherItemsStack.Count;

         // Find slots containing stone or bone items, if they exist.
         commonSlotforStone = getCommonSlot("Stone");
         commonSlotforBone = getCommonSlot("Bone");

         // Update the UI for stone items
         if (commonSlotforStone != -1)
         {
             // Check if there are any stones in the inventory stack.
             if (stoneCount > 0)
             {
                 // Decrease the displayed count for stone items by 1.
                 inventorySlots[commonSlotforStone].text.text = (stoneCount - 1).ToString();
             }

             // If no stones are left, clear the slot.
             if (stoneCount == 1)
             {
                 inventorySlots[commonSlotforStone].Clear();
                 isStoneSet = false; // Reset flag as the slot is now empty.
             }
         }

         // Update the UI for bone items
         if (commonSlotforBone != -1)
         {
             // Check if there are any bones in the inventory stack.
             if (boneCount > 0)
             {
                 // Decrease the displayed count for bone items by 1.
                 inventorySlots[commonSlotforBone].text.text = (boneCount - 1).ToString();
             }

             // If no bones are left, clear the slot.
             if (boneCount == 1)
             {
                 inventorySlots[commonSlotforBone].Clear();
                 isBoneSet = false; // Reset flag as the slot is now empty.
             }
         }


     }*/
    public void Start()
    {
        Inventory = Inventory.Instance;
        stoneStack = Inventory.stoneStack;
        boneStack = Inventory.otherItemsStack;
        isBoneSet = false;
        isStoneSet = false;
        iterator = 0;
        Inventory.OnItemChangedcallBack += addItem;
        //  Inventory.OnItemRemovedcallBack += removeItem;
        // Inventory.OnItemChangedcallBack += UpdateUIonEquip;
        Inventory.OnItemRemovedcallBack += UpdateUIonRemove;

        //objectsParent.GetComponentsInChildren<InventorySlot>();
        ClearCount();
    }

    private void ClearCount()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {



            inventorySlots[i].Clear();


        }
    }

    private void Update()
    {
        // changeUIonSelectedItem();
    }
    private void UpdateUIonRemove()
    {
        // Retrieve current item counts.
        int stoneCount = Inventory.Instance.stoneStack.Count;
        int boneCount = Inventory.Instance.otherItemsStack.Count;

        Debug.Log("NUMBER OF STONES = " + stoneCount);
        Debug.Log("NUMBER OF BONES = " + boneCount);

        // Iterate through each inventory slot.
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item != null)
            {
                if (inventorySlots[i].item.objectName == "Stone")
                {
                    // Update stone item count display.
                    inventorySlots[i].text.text = stoneCount.ToString();

                    // Clear the slot if there are no stones left.
                    if (stoneCount == 0)
                    {
                        inventorySlots[i].Clear();
                        //iterator = 0; // Resetting iterator is not necessary here.
                    }
                }
                else if (inventorySlots[i].item.objectName == "Bone")
                {
                    // Update bone item count display.
                    inventorySlots[i].text.text = boneCount.ToString();

                    // Clear the slot if there are no bones left.
                    if (boneCount == 0)
                    {
                        inventorySlots[i].Clear();
                        //iterator = 0; // Resetting iterator is not necessary here.
                    }
                }
            }
        }

        // Move any remaining items to fill cleared slots.
        RearrangeItems();
    }

    // New method to rearrange items after removal.
    private void RearrangeItems()
    {
        // Start at the first slot and move items forward.
        for (int i = 0; i < inventorySlots.Length - 1; i++)
        {
            // If the current slot is empty and the next one is not, move the item forward.
            if (inventorySlots[i].item == null && inventorySlots[i + 1].item != null)
            {
                inventorySlots[i].addItem(inventorySlots[i + 1].item); // Move the item forward.
                inventorySlots[i].text.text = inventorySlots[i + 1].text.text; // Update the text count.

                // Clear the next slot after moving the item.
                inventorySlots[i + 1].Clear();
            }
        }
    }


    /* private void UpdateUIonRemove() // Remove Item
     {
         List<ItemSO> allItems = Inventory.GetAllItems();
         int stoneCount = Inventory.Instance.stoneStack.Count;
         int boneCount = Inventory.Instance.otherItemsStack.Count;
         Debug.Log("NUMBER OF STONES = " + stoneCount);
         Debug.Log("NUMBER OF BONES = " + boneCount);
         for (int i = 0; i < inventorySlots.Length; i++)
         {
             if (inventorySlots[i].item != null)
             {
                 if (inventorySlots[i].item.objectName == "Stone")
                 {
                     inventorySlots[i].text.text = stoneCount.ToString();
                     if (inventorySlots[i].text.text.Equals("0"))
                     {
                         inventorySlots[i].Clear();
                         isStoneSet = false; 
                         iterator = 0;

                     }
                 }
               else if (inventorySlots[i].item.objectName == "Bone")
                 {
                     inventorySlots[i].text.text = boneCount.ToString();
                     if (inventorySlots[i].text.text.Equals("0"))
                     {
                         inventorySlots[i].Clear();
                         isBoneSet = false;
                         iterator = 0;

                     }
                 }
             }
         }



     }*/



    private void UpdateUIonEquip()  // Add Item
    {
        List<ItemSO> allItems = Inventory.GetAllItems();
        int stoneCount = Inventory.Instance.stoneStack.Count;
        int boneCount = Inventory.Instance.otherItemsStack.Count;
        Debug.Log("NUMBER OF STONES = " + stoneCount);
        Debug.Log("NUMBER OF BONES = " + boneCount);
        for (int i = 0; i < inventorySlots.Length; i++)
        {
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
                    stoneItem = null;
                    iterator++;
                }
                if (boneItem != null && !isBoneSet)
                {
                    inventorySlots[iterator].addItem(boneItem);
                    isBoneSet = true;
                    boneItem = null;
                    iterator++;

                }
            }
        }
        else
        {
            iterator++;
        }

    }
}



