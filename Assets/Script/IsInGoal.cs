using UnityEngine;
using UnityEngine.SceneManagement;

public class IsInGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (other.CompareTag("Player") && player != null)
        {
            player.reachedGoal = true; // Halte den Spieler an
            // Setze den Status für das Erreichen des Ziels
            if (player.playerNumber == 1)
            {
                print("Player 1 reached Goal");
                PlayerControl.playerOneReachedGoal = true;
                PlayerControl.playerOneTurnIsOver = true;
            }
            else if (player.playerNumber == 2)
            {
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
