using UnityEngine;
using System.Collections.Generic;


public class Checkpoint : MonoBehaviour
{
    public SphereCollider collider;
    public bool checkpointReached;
    public ChestDialogue dialogue;
    public Vector3 lastPoint;
   [SerializeField] List<ItemSO> savedStoneStack;  // Store stone stack items here
    [SerializeField] List<ItemSO> savedOtherItemsStack;  // Store other items stack here
    public delegate void oncheckpointReached();
    public oncheckpointReached callback;
    public GameObject checkpointEffect;
    public Vector3 effectPoint;
    [SerializeField] public GameObject[] items;

    public void resetItems()
    {
        foreach (var item in items)
        {
            item.SetActive(true);
        }
    }
    private void Start()
    {
        checkpointReached = false;
        collider = GetComponent<SphereCollider>();
        effectPoint = transform.GetChild(0).transform.position;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundPlayer.PlayOneShotSound("checkpoint");
            checkpointEffect = Instantiate(checkpointEffect, effectPoint, Quaternion.identity);
            dialogue.appear();
            checkpointEffect.transform.Rotate(90, 0, 0);
            Destroy(checkpointEffect , 0.5f);

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
        Debug.Log(" method Called");
    }
}
