using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject selectionbox;
    private CameraSwitchManager cameraSwitchManager;
    private ScoreManager scoreManager;
    private PlayerControl[] player;
    private HandleHeadUpDisplay handleHeadUpDisplay;
    private bool hasSwitched = false;

    private void Awake()
    {
        // Initialisiere cameraSwitchManager in Awake
        cameraSwitchManager = FindObjectOfType<CameraSwitchManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        player = FindObjectsOfType<PlayerControl>();
        handleHeadUpDisplay = FindObjectOfType<HandleHeadUpDisplay>();

        StartGameWithGameOverview();

        if (cameraSwitchManager == null)
        {
            Debug.LogError("CameraSwitchManager could not be found in the scene.");
        }
    }
    void Update()
    {
        CheckGameState();
    }

    public void CheckGameState()
    {
        if (CheckIfTurnIsOver())
        {
            PlayerGoalStatus.roundIsFinished = true;
            if (PlayerControl.playerOneReachedGoal && PlayerControl.playerTwoReachedGoal)
            {
                PlayerGoalStatus.playerOneReachedGoal = true;
                PlayerGoalStatus.playerTwoReachedGoal = true;
            }
            else if (PlayerControl.playerOneIsDead && PlayerControl.playerTwoIsDead)
            {
                PlayerGoalStatus.playerOneReachedGoal = false;
                PlayerGoalStatus.playerTwoReachedGoal = false;
            }
            else if (PlayerControl.playerOneReachedGoal && PlayerControl.playerTwoIsDead)
            {
                PlayerGoalStatus.playerOneReachedGoal = true;
                PlayerGoalStatus.playerTwoReachedGoal = false;
            }
            else if (PlayerControl.playerOneIsDead && PlayerControl.playerTwoReachedGoal)
            {
                PlayerGoalStatus.playerOneReachedGoal = false;
                PlayerGoalStatus.playerTwoReachedGoal = true;
            }

            StartCoroutine(DeactivatePlayerMovement(2));
            ChangeToScoreScreen();
            
            if (PlayerGoalStatus.gameFinished)
            {
                print("going into the CrownTheWInner");
                scoreManager.CrownTheWinner();
                print("changing the Scene");
                StartCoroutine(LoadEndMenuWithDelay(12));
            }
            else
            {
                ChangeToPlacingPhase();
            }
        }
        if (CheckIfSelectionIsOver() && !hasSwitched)
        {
            PlayerGoalStatus.gameplayStarted = true;
            ChangeToGameplayPhase();
        }
    }
    
    private IEnumerator LoadSceneWithDelay(float delay, string cameraScreen)
    {
        yield return new WaitForSeconds(delay);
        
        if (cameraScreen == "gameplay")
        {
            cameraSwitchManager.SwitchToGameplayCamera();
            EnableOrDisableSelectionBox("disable");
        }
        
        else if (cameraScreen == "score")
        {
            cameraSwitchManager.SwitchToScoreCamera();
        }
        
        else if (cameraScreen == "selection")
        {
            cameraSwitchManager.SwitchToSelectionCamera();
            handleHeadUpDisplay.ActivatePlacementUI();
            EnableOrDisableSelectionBox("enable");
            if (PlayerGoalStatus.gameIsCurrentlyStarting == true)
            {
                PlayerGoalStatus.gameIsCurrentlyStarting = false;
            }
        }
    }

    private IEnumerator ReactivatePlayerMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (PlayerControl playerino in player)
        {
            playerino.ResetPlayer();
        }
    }

    private IEnumerator DeactivatePlayerMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (PlayerControl playerino in player)
        {
            playerino.DeactivatePlayer();
        }
    }

    private IEnumerator LoadEndMenuWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }

    public bool CheckIfTurnIsOver()
    {
        if (PlayerControl.playerOneTurnIsOver && PlayerControl.playerTwoTurnIsOver)
        {
            return true;
        }
        else return false;
    }

    public bool CheckIfSelectionIsOver()
    {
        if (MouseDrag.selectionIsOver == true)
        {
            return true;
        }
        else return false;
    }

    public void ResetTurnIsOverToFalse()
    {
        PlayerControl.playerOneTurnIsOver = false;
        PlayerControl.playerTwoTurnIsOver = false;
        PlayerGoalStatus.roundIsFinished = false;
    }
    
    private void StartGameWithGameOverview()
     {
         MouseDrag.selectionIsOver = false;
         MouseDrag.placedObjects = 0;
         hasSwitched = false;
         cameraSwitchManager.SwitchToSelectionCamera();
         handleHeadUpDisplay.ActivatePlacementUI();
         EnableOrDisableSelectionBox("disable");
         foreach (PlayerControl playerino in player)
         {
             playerino.DeactivatePlayer();
         }
         PlayerGoalStatus.gameIsCurrentlyStarting = true;
     }
    private void ChangeToScoreScreen()
    {
        PlayerGoalStatus.gameplayStarted = false;
        //Deactivating Head Up Display
        handleHeadUpDisplay.DeactivatePlayerLifeUI();
        
        StartCoroutine(LoadSceneWithDelay(1,"score"));
        scoreManager.UpdateScoresBasedOnPlayerGoalStatus();
        ResetTurnIsOverToFalse();
    }
    private void ChangeToPlacingPhase()
    {
        MouseDrag.selectionIsOver = false;
        //Activate Head Up Display for placements
        StartCoroutine(LoadSceneWithDelay(7, "selection"));
        MouseDrag.placedObjects = 0;
        hasSwitched = false;
    }

    private void ChangeToGameplayPhase()
    {
        // Deactivate UI for placements and activate UI for lives
        handleHeadUpDisplay.DeactivatePlacementUI();
        handleHeadUpDisplay.ActivatePlayerLifeUI();
        StartCoroutine(ReactivatePlayerMovement(1));
        StartCoroutine(LoadSceneWithDelay(1,"gameplay"));
        hasSwitched = true;
    }

    private void EnableOrDisableSelectionBox(string instruction)
    {
        if (instruction == "enable")
        {
            selectionbox.GameObject().SetActive(true);
        }

        else if (instruction == "disable")
        {
            selectionbox.GameObject().SetActive(false);
        }

        else print("Selection box could not be enabled or disabled");
    }
}


