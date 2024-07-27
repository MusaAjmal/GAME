using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipButton : MonoBehaviour
{
    public delegate void EquipbuttonPressed();
    public EquipbuttonPressed BTCallback;
    public static EquipButton Instance { get; private set; }
    public GameObject SlingShot;
    public Player player;
    Chest chest;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        chest = Chest.Instance;
    SlingShot = GameObject.FindGameObjectWithTag("SlingShot");
    }

    public void buttonPressed()
    {
        SlingShot.SetActive(false);
        BTCallback?.Invoke();
        if (chest.isPlayerClose())
        {
            chest.Interact();
        }
        
    }



}
