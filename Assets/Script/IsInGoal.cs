using UnityEngine;
using UnityEngine.SceneManagement;

// Check if a Player reached the goal
public class IsInGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        // If player reached the goal, inactivate player movement
        if (other.CompareTag("Player") && player != null)
        {
            player.reachedGoal = true; // inactivate player movement
            
            // set status parameter for reaching the goal
            if (player.playerNumber == 1)
            {
                // player 1
                print("Player 1 reached Goal");
                PlayerControl.playerOneReachedGoal = true;
                PlayerControl.playerOneTurnIsOver = true;
            }
            else if (player.playerNumber == 2)
            {
                // player 2
                print("Player 2 reached Goal");
                PlayerControl.playerTwoReachedGoal = true;
                PlayerControl.playerTwoTurnIsOver = true;
            }
            
            if (gameManager != null)
            {
                gameManager.CheckGameState();
            }
            else
            {
                print("Warning: GameManager is null!");
            }
        }
    }
}
