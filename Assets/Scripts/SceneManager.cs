using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    Chest Chest;

    public bool[] Stars;
    private  int starNumber;

    private void Awake()
    {
        Stars= new bool[4];
        Instance = this;
    }
    private void Start()
    {
        starNumber = 0;
        InitialStars();
        Chest = Chest.Instance;
        Chest.SPcallback += initiateStarRetrieval;
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ShowStars();
        }
    }
    private void InitialStars()
    {
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i] = false;
        }
    }
    private void ShowStars()
    {
       
        Debug.Log("Number of Stars for this Level " +  starNumber);
       // starNumber = 0;
    }
    public void initiateStarRetrieval()
    {
        for (int i = 0; i < Stars.Length; i++) {
            if (Stars[i] == false)
            {
                Stars[i] = true;
                starNumber++;
               // Debug.Log(Stars[i]);
                break;
                
            }
        }
    }
}
