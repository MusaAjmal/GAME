using System.Collections.Generic;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
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

    public void Start()
    {
        Inventory = Inventory.Instance;
        isBoneSet = false;
        isStoneSet = false;
        iterator = 0;
        Inventory.OnItemChangedcallBack += UpdateUIonEquip;
        Inventory.OnItemRemovedcallBack += UpdateUIonRemove;

        inventorySlots = objectsParent.GetComponentsInChildren<InventorySlot>();
        ClearCount();
    }

    private void ClearCount()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {

           
            
                inventorySlots[i].Clear();
            

        }
    }

    private void UpdateUIonRemove() // Remove Item
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
       for(int i = 0; i< inventorySlots.Length; i++) {
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



