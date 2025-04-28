using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

// Manages the game state, transitions, and phases
public class GameManager : MonoBehaviour
{
    public GameObject selectionbox; // UI for selecting items
    private CameraSwitchManager cameraSwitchManager; // Manages camera transitions
    private ScoreManager scoreManager; // Manages scoring logic
    private PlayerControl[] player; // Array holding player objects
    private HandleHeadUpDisplay handleHeadUpDisplay; // Manages the HUD
    private bool hasSwitched = false; // Tracks if a phase switch has occurred

    private void Awake()
    {
        // Initialise components and manager
        cameraSwitchManager = FindObjectOfType<CameraSwitchManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        player = FindObjectsOfType<PlayerControl>();
        handleHeadUpDisplay = FindObjectOfType<HandleHeadUpDisplay>();
        
        // Set up the game’s initial view and restart behavior
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
            // Check for who won and save it in PlayerGoalStatus
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
        
            // players are not allowed to move anymore
            StartCoroutine(DeactivatePlayerMovement(2));
            // and change to score screen
            ChangeToScoreScreen();
            
            // Determine if the game has concluded
            if (PlayerGoalStatus.gameFinished)
            {
                print("going into the CrownTheWInner");
                scoreManager.CrownTheWinner(); // Announce winner
                StartCoroutine(LoadEndMenuWithDelay(12)); // Load end menu with delay
            }
            else
            {
                // if no one won, go to the next placingphase
                ChangeToPlacingPhase();
            }
        }
        // Check if selection phase is complete and gameplay can start
        if (CheckIfSelectionIsOver() && !hasSwitched)
        {
            PlayerGoalStatus.gameplayStarted = true;
            ChangeToGameplayPhase(); // Transition to gameplay
        }
    }
    
    private IEnumerator LoadSceneWithDelay(float delay, string cameraScreen)
    {
        // Delay before switching to allow for viewing of screens
        yield return new WaitForSeconds(delay);
        
        // Determine which camera view to switch to
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
            
            // If the game is at the initial start, update the status
            // In that way we skip the starting "act" for the next rounds
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
        
        // Reset each player's state
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
        // reset TurnIsOver to false, so we can play again actively
        PlayerControl.playerOneTurnIsOver = false;
        PlayerControl.playerTwoTurnIsOver = false;
        PlayerGoalStatus.roundIsFinished = false;
    }
    
    private void StartGameWithGameOverview()
     {
         // This is going to be the start. Therefor we want a short sight of the map. 
         // We take the selection camera, because that is where you see the most

         MouseDrag.selectionIsOver = false;
         MouseDrag.placedObjects = 0;
         hasSwitched = false;
         cameraSwitchManager.SwitchToSelectionCamera();
         
         // Important for restart. Normally the selection screen is only active, if turnIsOver == true
         PlayerControl.playerOneTurnIsOver = true;
         PlayerControl.playerTwoTurnIsOver = true;
         
         // we disable unnecessary extras
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
        //Deactivating Head Up Display. We only want to see the players lives in the gameplayphase
        handleHeadUpDisplay.DeactivatePlayerLifeUI();
        // Go to score screen
        StartCoroutine(LoadSceneWithDelay(1,"score"));
        // Update scores
        scoreManager.UpdateScoresBasedOnPlayerGoalStatus();
        // prepare for playing again
        ResetTurnIsOverToFalse();
    }
    private void ChangeToPlacingPhase()
    {
        // activate Mouse drag to select and drag items on the map
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
        // Reactivate players and start Gameplay phase
        StartCoroutine(ReactivatePlayerMovement(1));
        StartCoroutine(LoadSceneWithDelay(1,"gameplay"));
        hasSwitched = true;
    }

    private void EnableOrDisableSelectionBox(string instruction)
    {
        // enable or disable the selectionbox with all selectable items
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


