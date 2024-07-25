using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameOverScript gameOverScript;
    [SerializeField] private GameObject touchPanel;
    [SerializeField] private GameObject Inventory;
    public static LevelManager Instance { get; private set; }
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
       // gameOverScript = GetComponent<GameOverScript>();
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
    public void GameOverScreen()
    {
        Inventory.SetActive(false);
        touchPanel.SetActive(false);    
        gameOverScript.SetUp();
    }
    private void ShowStars()
    {
       
        Debug.Log("Number of Stars for this Level " +  starNumber);
       // starNumber = 0;
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Quit()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        Application.Quit(); 
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
