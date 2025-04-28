using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Reset of game stats is important if you want to play again
        ResetAllGameStats();
        SceneManager.LoadScene(1); //Loading Default Level
    }

    public void ExitGame()
    {
        // quit game
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
