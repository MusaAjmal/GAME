using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool itemReceived;
    [SerializeField] private ItemSO item;
    public static Chest Instance { get; private set; }

    public delegate void SpecialItemReceived();
    public SpecialItemReceived SPcallback;
    private void Start()
    {
        Instance = this;
        itemReceived = false;
    }

    public void Interact()
    {
        if (item!= null)
        {
            if (Vector3.Distance(Player.Instance.GetPosition(), transform.position) < Player.Instance.pickupDistance)
            {
                itemReceived = true;
                
                Debug.Log("Collected Special Item : " + item.objectName);
                item = null;
                SPcallback?.Invoke();
            }
        }
        
        
        
    }
    public bool isPlayerClose()
    {
        return (Vector3.Distance(Player.Instance.GetPosition(), transform.position) < Player.Instance.pickupDistance);
        

        
    }

   /* private void Update()
    {
        if (Input.GetKey(KeyCode.X)) {
            Interact();
        }
    }*/
}
