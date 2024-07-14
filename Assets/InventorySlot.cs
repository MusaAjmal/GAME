using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    ItemSO item;
    public Image icon;
    public Image countIcon;
    public Text text;
    int count;
    
    public void addItem(ItemSO newItem)
    {
        count++;
        item = newItem;
        icon.sprite = item.sprite;
        icon.enabled = true;
        
     

        if (text != null)
        {
            text.text = count.ToString();
        }
        else
        {
            Debug.LogWarning("Count Text component not found in countIcon's children.");
        }
    }
    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
