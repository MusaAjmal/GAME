using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    public SphereCollider collider;
    public bool checkpointReached;
    public Vector3 lastPoint;
    private List<ItemSO> savedStoneStack;  // Store stone stack items here
    private List<ItemSO> savedOtherItemsStack;  // Store other items stack here
    public delegate void oncheckpointReached();
    public oncheckpointReached callback;
    private void Start()
    {
        checkpointReached = false;
        collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveInventory();
            
            Respawn.instance.transform.position = transform.position;
            Debug.Log("Checkpoint Added!");
            checkpointReached = true;
            collider.enabled = false;
        }
    }

    private void SaveInventory()
    {
        // Save current state of the inventory stacks
        savedStoneStack = new List<ItemSO>(Inventory.Instance.stoneStack);
        savedOtherItemsStack = new List<ItemSO>(Inventory.Instance.otherItemsStack);
    }

    public void LoadInventory()
    {
        // Clear current inventory
        Inventory.Instance.stoneStack.Clear();
        Inventory.Instance.otherItemsStack.Clear();

        // Restore saved inventory state
        foreach (var item in savedStoneStack)
        {
            Inventory.Instance.stoneStack.Push(item);
        }

        foreach (var item in savedOtherItemsStack)
        {
            Inventory.Instance.otherItemsStack.Push(item);
        }

        // Update the default item if needed
        Inventory.Instance.CycleItems();
    }
}
