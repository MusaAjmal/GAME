using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
   [SerializeField] public ItemSO item; // Current item in the slot
    [SerializeField] public Image icon; // Icon image to display item sprite
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Image bkgrndImage;
    // Optional: Count icon
   // public Text text; // Optional: Text for count display
    int count = 1; // Count of items (optional)
    /* public Button button;
     private ColorBlock defaultColor;*/
    // Increment count of items (optional)
    public void IncrementCount()
    {
        if (text != null)
        {
            count++;
            text.text = count.ToString();
            //text.text = count.ToString();
        }
        else
        {
            Debug.LogWarning("Count Text component not found in countIcon's children.");
        }
    }
    public void Start()
    {
        /* button = GetComponentInChildren<Button>();
         if (button != null)
         {
             // Store the original ColorBlock
             defaultColor = button.colors;
         }
         else
         {
             Debug.LogError("Button component not found on the GameObject.");
         }*/
    }

    // Add an item to the slot
    public void addItem(ItemSO newItem)
    {
        item = newItem;
        icon.sprite = item.sprite;
        icon.enabled = true;
        text.enabled = true;
        text.text= count.ToString();
       /* text.enabled = true;
        text.text = count.ToString();*/
    }

    // Clear the slot (remove item)
    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        // Reset count if needed
        if (text != null)
        {
            text.enabled = false;
        }
    }
}
   /* public void changeButtonColor()
    {
       
        if (button != null)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = Color.red; // Colors use values between 0 and 1
            button.colors = cb;
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject." + this.name);
        }
    }*/
   /* public void ResetColor()
    {
        if (button != null)
        {
            button.colors = defaultColor; 
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject." + this.name);
        }
    }
}
*/