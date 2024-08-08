using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool itemReceived;
    [SerializeField] private ItemSO item;
    public GameObject chestEffect;
    public ChestDialogue ChestDialogue;
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
                SoundPlayer.PlayOneShotSound("chest");
                itemReceived = true;
                item = null;
                SPcallback?.Invoke();
              
                ChestDialogue.appear();
                GameObject chestowo = Instantiate(chestEffect, transform.position, Quaternion.identity);
                chestowo.transform.Rotate(90, 0, 0);
                Destroy(chestowo, 0.5f);
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
