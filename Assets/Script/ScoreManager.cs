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
    
    public TextMeshProUGUI playerOneScoreText;
    public TextMeshProUGUI playerTwoScoreText;
    public TextMeshProUGUI tooEasyTag;
    public TextMeshProUGUI tooHardTag;
    public TextMeshProUGUI winnerTag;

    public Image winningCrownP1;
    public Image winningCrownP2;
    public Image deadEyeP1;
    public Image deadEyeP2;

    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    
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
        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();
        tooEasyTag.text = resetText;
        tooHardTag.text = resetText;
        winnerTag.text = resetText;
        ResetImages();
        
        UpdateScoresBasedOnPlayerGoalStatus();
    }

    public void UpdateScoresBasedOnPlayerGoalStatus()
    {
        if (PlayerGoalStatus.roundIsFinished && !PlayerGoalStatus.gameIsCurrentlyStarting)
        {
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
            if (PlayerGoalStatus.playerOneReachedGoal && PlayerGoalStatus.playerTwoReachedGoal)
            {
                ResetText();
                NoPointsTooEasy();
            }
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
        StartCoroutine(SetUpWinningScreenWithDelay());
    }
    
    public void AddPointPlayerOne()
    {
        playerOneScore += 1;
        StartCoroutine(UpdatePlayerOneScoreTextWithDelay());
    }

    public void AddPointPlayerTwo()
    {
        if (instance != null) // Sicherstellen, dass die Instanz nicht null ist
        {
            playerTwoScore += 1;
            StartCoroutine(UpdatePlayerTwoScoreTextWithDelay());
        }
    }

    public void NoPointsTooEasy()
    {
        tooEasyTag.text = easyText;
    }

    public void NoPointsTooHard()
    {
        tooHardTag.text = hardText;
    }

    public void UpdateWinnerText()
    {
        winnerTag.text = winningText;
    }

    public void ResetText()
    {
        tooEasyTag.text = resetText;
        tooHardTag.text = resetText;
    }

    private void ResetImages()
    {
        winningCrownP1.gameObject.SetActive(false);
        winningCrownP2.gameObject.SetActive(false);
        deadEyeP1.gameObject.SetActive(false);
        deadEyeP2.gameObject.SetActive(false);
    }
    
    private IEnumerator UpdatePlayerOneScoreTextWithDelay()
    {
        yield return new WaitForSeconds(5f);
        playerOneScoreText.text = playerOneScore.ToString();
    }
    
    private IEnumerator UpdatePlayerTwoScoreTextWithDelay()
    {
        // Warte 5 Sekunden
        yield return new WaitForSeconds(5f);
        playerTwoScoreText.text = playerTwoScore.ToString();
    }

    private IEnumerator SetUpWinningScreenWithDelay()
    {
        yield return new WaitForSeconds(6f);
        CheckForWinner();
    }

    private void CheckIfGameOver()
    {
        if (playerOneScore >= 4 || playerTwoScore >= 4)
        {
            PlayerGoalStatus.gameFinished = true;
        }
    }

    private void CheckForWinner()
    {
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
