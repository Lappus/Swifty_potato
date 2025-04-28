using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private GameManager gameManager;
    
    // UI elements for displaying scores and messages
    public TextMeshProUGUI playerOneScoreText;
    public TextMeshProUGUI playerTwoScoreText;
    public TextMeshProUGUI tooEasyTag;
    public TextMeshProUGUI tooHardTag;
    public TextMeshProUGUI winnerTag;

    // UI elements for displaying winning and losing indicators
    public Image winningCrownP1;
    public Image winningCrownP2;
    public Image deadEyeP1;
    public Image deadEyeP2;

    // Scores for Player One and Player Two
    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    
    // Text messages for different game outcomes
    private string easyText = "Come on ... Too easy!";
    private string hardText = "Next time  just try harder!";
    private string resetText = "";
    private string winningText = "We got a Winner";

    private void Awake()
    {
        instance = this;
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        // Initialize UI elements with default values
        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();
        tooEasyTag.text = resetText;
        tooHardTag.text = resetText;
        winnerTag.text = resetText;
        ResetImages(); // Reset all images to inactive
        
        // Update scores based on the initial player goal status (for restart to 0 - 0)
        UpdateScoresBasedOnPlayerGoalStatus();
    }

    public void UpdateScoresBasedOnPlayerGoalStatus()
    {
        // Check if the round is finished and the game is not just starting
        if (PlayerGoalStatus.roundIsFinished && !PlayerGoalStatus.gameIsCurrentlyStarting)
        {
            // Update scores based on which player(s) reached the goal
            if (PlayerGoalStatus.playerOneReachedGoal && !PlayerGoalStatus.playerTwoReachedGoal)
            {
                ResetText();
                AddPointPlayerOne();
            }
            if (!PlayerGoalStatus.playerOneReachedGoal && PlayerGoalStatus.playerTwoReachedGoal)
            {
                ResetText();
                AddPointPlayerTwo();
            }
            // If both player reached the Goal, no points are given --> too easy
            if (PlayerGoalStatus.playerOneReachedGoal && PlayerGoalStatus.playerTwoReachedGoal)
            {
                ResetText();
                NoPointsTooEasy();
            }
            // If no player reached the Goal, no points are given --> too hard
            if (!PlayerGoalStatus.playerOneReachedGoal && !PlayerGoalStatus.playerTwoReachedGoal)
            {
                ResetText();
                NoPointsTooHard();
            }

            CheckIfGameOver();
        }
    }

    public void CrownTheWinner()
    {
        ResetText();
        StartCoroutine(SetUpWinningScreenWithDelay());  // Set up the winning screen with a delay
    }
    
    public void AddPointPlayerOne()
    {
        playerOneScore += 1; // Increment Player One's score
        StartCoroutine(UpdatePlayerOneScoreTextWithDelay()); // Update the score text with a delay
    }

    public void AddPointPlayerTwo()
    {
        playerTwoScore += 1; // Increment Player two's score
        StartCoroutine(UpdatePlayerTwoScoreTextWithDelay()); // Update the score text with a delay
    }

    public void NoPointsTooEasy()
    {
        tooEasyTag.text = easyText; // Display the "Too Easy" message
    }

    public void NoPointsTooHard()
    {
        tooHardTag.text = hardText; // Display the "Too Hard" message
    }

    public void UpdateWinnerText()
    {
        winnerTag.text = winningText; // Display the "We got a Winner" message
    }

    public void ResetText()
    {
        tooEasyTag.text = resetText;
        tooHardTag.text = resetText;
    }

    private void ResetImages()
    {
        // Deactivate all winning and losing indicators
        winningCrownP1.gameObject.SetActive(false);
        winningCrownP2.gameObject.SetActive(false);
        deadEyeP1.gameObject.SetActive(false);
        deadEyeP2.gameObject.SetActive(false);
    }
    
    private IEnumerator UpdatePlayerOneScoreTextWithDelay()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        playerOneScoreText.text = playerOneScore.ToString(); // Update Player One's score text
    }
    
    private IEnumerator UpdatePlayerTwoScoreTextWithDelay()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        playerTwoScoreText.text = playerTwoScore.ToString(); // Update Player Two's score text
    }

    private IEnumerator SetUpWinningScreenWithDelay()
    {
        yield return new WaitForSeconds(6f); // Wait for 6 seconds
        CheckForWinner(); // Wait for 6 seconds
    }

    private void CheckIfGameOver()
    {
        // Check if either player has reached a score of 4 which is the winning score
        if (playerOneScore >= 4 || playerTwoScore >= 4)
        {
            PlayerGoalStatus.gameFinished = true; // Mark the game as finished
        }
    }

    private void CheckForWinner()
    {
        // Check and display the winner based on the scores
        if (playerOneScore >= 4)
        {
            print("playerOne Won");
            UpdateWinnerText();
            winningCrownP1.gameObject.SetActive(true);
            deadEyeP2.gameObject.SetActive(true);
            winningCrownP2.gameObject.SetActive(false);
            deadEyeP1.gameObject.SetActive(false);
        }
        else if (playerTwoScore >= 4)
        {
            print("playerTwo Won");
            UpdateWinnerText();
            winningCrownP2.gameObject.SetActive(true);
            deadEyeP1.gameObject.SetActive(true);
            winningCrownP1.gameObject.SetActive(false);
            deadEyeP2.gameObject.SetActive(false);
        }
    }

}
