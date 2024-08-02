using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverScript : MonoBehaviour
{
    public void SetUp()
    {
    gameObject.SetActive(true); 
    }
    public void setDown()
    {
        gameObject.SetActive(false);
    }
}
