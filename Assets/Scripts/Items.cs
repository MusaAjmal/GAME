using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Items : MonoBehaviour
{
    [SerializeField] public ItemSO itemSO;
    public bool isOn;

    EquipButton bt;

    private void pickup()
    {
        if (itemSO.IsEquipable)
        {

            Inventory.Instance.AddItem(itemSO);



            Destroy(gameObject); //instead of destroying we change the parent of the transform

        }

    }
    private void Start()
    {
        isOn = true;
        bt = EquipButton.Instance;
        bt.BTCallback += OnMiddleMouseDown;
        bt.BTCallback += playerToggleItem;

    }



    private void Update()
    {

        if (Input.GetMouseButton(2)) // 2 corresponds to the middle mouse button
        {
            OnMiddleMouseDown();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerToggleItem();
        }

    }
    private void toggle()
    {
        if (itemSO.canbeToggled)
        {
            if (isActive())
            {
                isOn = false;
            }
            else
            {
                isOn = true;
            }

        }
    }
    public bool isActive()
    {
        return isOn;
    }
    private void playerToggleItem()
    {
        if(this != null && gameObject != null)
        {
            if (Vector3.Distance(Player.Instance.GetPosition(), transform.position) < Player.Instance.pickupDistance)
            {

                toggle();



            }
        }
        
    }


    private void OnMiddleMouseDown()
    {
        if (this!=null && gameObject!=null)
        {
            if (Vector3.Distance(Player.Instance.GetPosition(), transform.position) < Player.Instance.pickupDistance)
            {
                pickup();
            }
        }
        

    }

    public override bool Equals(object obj)
    {
        if (obj is Items)
        {
            return this.itemSO.objectName.Equals(((Items)obj).itemSO.objectName, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return itemSO.objectName.ToLower().GetHashCode();
    }
    /*  private void OnCollisionEnter(Collision collision)
      {
          Destroy(this);
      }*/
}