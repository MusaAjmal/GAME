using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Master : MonoBehaviour
{
    public static Master Instance;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private OptionMenu optionMenu;
    [SerializeField] private CreditsMenu creditsMenu;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;   
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public  void LoadInitialLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void OptionsPopup()
    {
        mainMenu.unset();
        optionMenu.set();
        creditsMenu.unset();
    }
    public void CreditsPopup()
    {
        mainMenu.unset();
        optionMenu.unset();
        creditsMenu.set();
    }

    public void backButtonPressed()
    {
        mainMenu.set();
        optionMenu.unset();
        creditsMenu.unset();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
