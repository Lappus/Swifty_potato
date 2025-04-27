using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(1); //Loading Default Level
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0); //Loading Main Menu
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
