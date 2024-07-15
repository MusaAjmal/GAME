using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemSO item; // Current item in the slot
    public Image icon; // Icon image to display item sprite
    // Optional: Count icon
    public Text text; // Optional: Text for count display
    int count = 1; // Count of items (optional)

    // Increment count of items (optional)
    public void IncrementCount()
    {
        if (text != null)
        {
            count++;
            text.text = count.ToString();
        }
        else
        {
            Debug.LogWarning("Count Text component not found in countIcon's children.");
        }
    }

    // Add an item to the slot
    public void addItem(ItemSO newItem)
    {
        item = newItem;
        icon.sprite = item.sprite;
        icon.enabled = true;
        text.text = count.ToString();
    }

    // Clear the slot (remove item)
    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        count = 0; // Reset count if needed
        if (text != null)
        {
            text.text = "0";
        }
    }
}
