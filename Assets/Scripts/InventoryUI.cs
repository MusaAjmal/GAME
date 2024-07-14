using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory Inventory;
    public Transform objectsParent;
    public InventorySlot[] inventorySlots;
    public void Start()
    {
        Inventory = Inventory.Instance;
        
        Inventory.OnItemChangedcallBack += UpdateUI;

        inventorySlots = objectsParent.GetComponentsInChildren<InventorySlot>();    
    }
    public void Update()
    {
        
    }
    private void UpdateUI()
    {
        
    }
}
