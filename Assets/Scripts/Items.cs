using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] private bool IsEquipable;
    [SerializeField] private bool IsDecoration;
    [SerializeField] private bool canbeToggled;

    private void pickup()
    {
        if (IsEquipable)
        {
            Player.Instance.inventory.AddItem(gameObject.tag);
            Destroy(gameObject);
            Player.Instance.inventory.SortItems();
            Player.Instance.inventory.equippedItem = Player.Instance.inventory.GetFirstItem();

        }
       
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2)) // 2 corresponds to the middle mouse button
        {
            OnMiddleMouseDown();
        }
    }


    private void OnMiddleMouseDown()
    {
        if(Vector3.Distance(Player.Instance.GetPosition(), transform.position)  < Player.Instance.pickupDistance){
            pickup();
        }
    
    }

}
