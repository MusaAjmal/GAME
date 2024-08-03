using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameOverScript gameOverScript;
    [SerializeField] private GameObject touchPanel;
    [SerializeField] private GameObject Inventory;
    //[SerializeField] private GameObject Player;
    [SerializeField] private LevelComplete levelComplete;
    [SerializeField] private GameObject[] images;
    [SerializeField] private Checkpoint Checkpoint;
   // [SerializeField] private Checkpoint Checkpoint2;

    [SerializeField] public PlayerTraverse points;
    public static LevelManager Instance { get; private set; }
    Chest Chest;

    public bool[] Stars;
    private int starNumber;

    private void Awake()
    {
        Stars = new bool[4];
        Instance = this;
    }
    private void Start()
    {
        // gameOverScript = GetComponent<GameOverScript>();
        starNumber = 0;
        InitialStars();
        Chest = Chest.Instance;
        Chest.SPcallback += initiateStarRetrieval;
        // points = GetComponent<PlayerTraverse>();

        if (Checkpoint != null)
        {
            Debug.Log("checkpoint exists and player reached : " + Checkpoint.checkpointReached);
        }

    }
    /* private void Update()
     {
         if (Input.GetKeyDown(KeyCode.LeftControl))
         {
             ShowStars();
         }
     }*/
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
    public void LevelComplete()
    {

        Inventory.SetActive(false);
        touchPanel.SetActive(false);
        initiateStarRetrieval();
        levelComplete.set();
        DisplayStars();
    }
    private void DisplayStars()
    {
        for (int i = 0; i < starNumber; i++)
        {
            images[i].SetActive(true);
        }
    }
    private void ShowStars()
    {

        Debug.Log("Number of Stars for this Level " + starNumber);
        // starNumber = 0;
    }
    public void Retry()
    {
        if (Checkpoint != null)
        {
            if (Checkpoint.checkpointReached)
            {
                Respawn.instance.RespawnPlayer();
                points.OnPlayerRespawn();
                
                gameOverScript.setDown();
                Inventory.SetActive(true);
                touchPanel.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }


        }
        
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /////GPT
        /*f (Checkpoint2 != null && Checkpoint2.checkpointReached)
        {
            // Player has reached the second checkpoint; respawn at Checkpoint2.
            Respawn.instance.RespawnPlayer();
            points.OnPlayerRespawn();
            gameOverScript.setDown();
            Inventory.SetActive(true);
            touchPanel.SetActive(true);
        }
        else if (Checkpoint != null && Checkpoint.checkpointReached)
        {
            // Player has reached the first checkpoint but not the second; respawn at Checkpoint1.
            Respawn.instance.RespawnPlayer();
            points.OnPlayerRespawn();
            gameOverScript.setDown();
            Inventory.SetActive(true);
            touchPanel.SetActive(true);
        }
        else
        {
            // No checkpoints reached; reset the scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }*/
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
