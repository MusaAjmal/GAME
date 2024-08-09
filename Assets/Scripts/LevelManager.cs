using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameOverScript gameOverScript;
    [SerializeField] private GameObject touchPanel;
    [SerializeField] private GameObject inventory;
    //[SerializeField] private GameObject Player;
    [SerializeField] private LevelComplete levelComplete;
    [SerializeField] private GameObject[] images;
    [SerializeField] private Checkpoint Checkpoint;
    [SerializeField] private Checkpoint Checkpoint2;
    private float elapsedTime = 0f;
    private float interval = 60f;


    [SerializeField] public PlayerTraverse points;
    public bool startRecevied;
    public static LevelManager Instance { get; private set; }
    Chest Chest;

    public bool[] Stars;
    private int starNumber;

    private void Awake()
    {
        Stars = new bool[4];
        if (Instance == null ) {
            Instance = this;
        }
        else
        {
            Destroy( Instance );    
        }
       
    }
    private void Start()
    {
        startRecevied = true;
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
     private void Update()
     {
        elapsedTime += Time.deltaTime;

        // Check if one minute has elapsed
        if (elapsedTime >= interval)
        {
            // Log a message to the console
            startRecevied = false;

            
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
        SoundPlayer.PlayOneShotSound("death");

        inventory.SetActive(false);
        touchPanel.SetActive(false);
        gameOverScript.SetUp();
    }
    public void LevelComplete()
    {
        if (startRecevied) {
        initiateStarRetrieval();
        }
        inventory.SetActive(false);
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
    public void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Retry()
    {
        if (Checkpoint != null)
        {
            if (Checkpoint.checkpointReached)
            {
               Checkpoint.LoadInventory();
                Respawn.instance.RespawnPlayer();
                points.OnPlayerRespawn();
                InventoryUI.instance.UpdateUI();
                gameOverScript.setDown();
                inventory.SetActive(true);
                touchPanel.SetActive(true);
                starNumber = 0;


            }
            
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                starNumber = 0;
            }


        }
        else if(Checkpoint2!= null)
        {
            if (Checkpoint2.checkpointReached)
            {
                Checkpoint2.LoadInventory();
                Respawn.instance.RespawnPlayer();
                points.OnPlayerRespawn();
                InventoryUI.instance.UpdateUI();
                gameOverScript.setDown();
                inventory.SetActive(true);
                touchPanel.SetActive(true);
                starNumber = 0;

            }

            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                starNumber = 0;
            }
        }
        
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            starNumber = 0;
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
