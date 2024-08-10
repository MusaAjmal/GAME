using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialContoller : MonoBehaviour
{
    [SerializeField] private Button SkipButton;
    [SerializeField] private Button NextButton;
    [SerializeField] private string[] Dialogues;
    [SerializeField] private GameObject[] referenceImages;
    [SerializeField] private Image referImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject finishButton;
    [SerializeField] private GameObject skiptutorial;

    string d1 = "LETS START WITH CONTROLS FIRST , THE WHITE DOTS ON THE GROUND INDICATE WHERE PLAYER CAN MOVE TOUCH ANYWHERE IN SCREEN TO MAKE THE PLAYER MOVE . THE RED BUTTON IS USED TO INTERACT WITH OBJECTS . THE GREEN BUTTON IS THE SLINGSHOT BUTTON THAT CAN HELP YOU SHOOT THE ITEM YOU HAVE IN YOUR INVENTORY.THE PURPLE BUTTON IS TO CYCLE TO THE NEXT ITEM IN THE INVENTORY.";
   string d2 = "DONOT GET CLOSE TO ENEMIES TO ESCAPE THE DUNGEON YOU NEED TO LOCATE THE ELAVATOR AND MAKE YOUR WAY TOWARDS IT . DISTRACT THE ENEMY WITH NOISE BY THROWING ITEMS AT VASES";
    string d3 = "THE CHEST CONTAINS SPECIAL ITEM : A STAR !!! BUT THE ITS RISKY TO REACH THE CHEST . ONLY THE BRAVE ONES COLLECT THE STAR AND THEN FINISH THE LEVEL ";
        public  int iterator;
    public void Start()
    {
        iterator = -1;
    }
    public string GetVariableType(object variable)
    {
        if (variable == null)
        {
            return "null";
        }

        Type type = variable.GetType();
        return type.ToString();
    }
    public void NextClick()
    {
        iterator++;
        

        if (iterator < referenceImages.Length)
        {


            //refresh Image
            Debug.Log(GetVariableType(referenceImages[iterator].name));
            Debug.Log("image id : "+referenceImages[iterator].name);
            Debug.Log("Iterator: " + iterator);
            if (iterator.ToString().Equals(referenceImages[iterator].name))
            {
                referenceImages[iterator].SetActive(true);
                text.text = Dialogues[iterator];
                if(iterator == 6)
                {
                    finishButton.SetActive(true);
                }
            }
            else
            {
                Debug.Log("NO YOU");
            }
        }
        else
        {
            finishButton.SetActive(true);
        }
    }
    public void LoadNewScene(string newSceneName)
    {
        // Get the currently loaded scenes
        int sceneCount = SceneManager.sceneCount;

        // Loop through all the loaded scenes and unload them
        for (int i = sceneCount - 1; i >= 0; i--)
        {
            // Get the scene at index i
            Scene scene = SceneManager.GetSceneAt(i);

            // Skip the active scene (current scene)
            if (scene.name != newSceneName)
            {
                // Unload the scene
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // Load the new scene
        SceneManager.LoadScene(newSceneName);
    }
    public void loadLevel()
    {
        LoadNewScene("Dev 1");
    }
    public void Skip()
    {
       // LoadNewScene("Main Menu");

       // SceneManager.LoadScene("Main Menu");
       skiptutorial.SetActive(true);
    }
    public void cross()
    {

    skiptutorial.SetActive(false); 
    }
    public void tick()
    {
        LoadNewScene("Dev 1");
    }

}
