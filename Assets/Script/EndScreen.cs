using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void RestartGame()
    {
        // Reset of game stats is important if you want to play again
        ResetAllGameStats();
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
    
    private void ResetAllGameStats()
    {
        PlayerGoalStatus.playerOneReachedGoal = false;
        PlayerGoalStatus.playerTwoReachedGoal = false;
        PlayerGoalStatus.roundIsFinished = true;
        PlayerGoalStatus.gameplayStarted = false;
        PlayerGoalStatus.gameFinished = false;
        PlayerGoalStatus.gameIsCurrentlyStarting = true;
    }
}
