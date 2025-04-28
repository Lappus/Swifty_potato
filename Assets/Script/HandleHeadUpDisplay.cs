using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Manages the Head-Up Display (HUD) elements for player lives and placements
public class HandleHeadUpDisplay : MonoBehaviour
{
    // UI elements representing player one’s lives
    public Image playerOneLifeOne; // Image for first life
    public Image playerOneLifeTwo; // Image for second life
    public Image playerOneLifeThree; // Image for third life
    // UI elements representing player two’s lives
    public Image playerTwoLifeOne; // Image for first life
    public Image playerTwoLifeTwo; // Image for second life
    public Image playerTwoLifeThree; // Image for third life

    // UI elements for player tags
    public TextMeshProUGUI playerOneTag; // Tag for player one
    public TextMeshProUGUI playerTwoTag; // Tag for player two
    
    // UI elements for placement information
    public Image placementOne; // First placement indication
    public Image placementTwo; // Second placement indication
    public TextMeshProUGUI placementTag; // Text tag for placements

    private void Start()
    {
        // Initialize the UI by hiding placement elements and activating player life UI
        DeactivatePlacementUI();
        ActivatePlayerLifeUI();
    }

    // Deactivates the placement UI elements
    public void DeactivatePlacementUI()
    {
        placementTag.gameObject.SetActive(false); // Hide placement tag
        placementOne.gameObject.SetActive(false); // Hide first placement
        placementTwo.gameObject.SetActive(false); // Hide second placement
    }

    // Activates the placement UI elements
    public void ActivatePlacementUI()
    {
        placementTag.gameObject.SetActive(true); // Show placement tag
        placementOne.gameObject.SetActive(true); // Show first placement
        placementTwo.gameObject.SetActive(true); // Show second placement
    }

    // Deactivates all player life UI elements
    public void DeactivatePlayerLifeUI()
    {
        playerOneLifeOne.gameObject.SetActive(false); // Hide first life image
        playerOneLifeTwo.gameObject.SetActive(false); // Hide second life image
        playerOneLifeThree.gameObject.SetActive(false); // Hide third life image
        playerOneTag.gameObject.SetActive(false); // Hide player one tag
        playerTwoLifeOne.gameObject.SetActive(false); // Hide first life image for player two
        playerTwoLifeTwo.gameObject.SetActive(false); // Hide second life image for player two
        playerTwoLifeThree.gameObject.SetActive(false); // Hide third life image for player two
        playerTwoTag.gameObject.SetActive(false); // Hide player two tag
    }

    // Activates all player life UI elements
    public void ActivatePlayerLifeUI()
    {
        playerOneLifeOne.gameObject.SetActive(true); // Show first life image
        playerOneLifeTwo.gameObject.SetActive(true); // Show second life image
        playerOneLifeThree.gameObject.SetActive(true); // Show third life image
        playerOneTag.gameObject.SetActive(true); // Show player one tag
        playerTwoLifeOne.gameObject.SetActive(true); // Show first life image for player two
        playerTwoLifeTwo.gameObject.SetActive(true); // Show second life image for player two
        playerTwoLifeThree.gameObject.SetActive(true); // Show third life image for player two
        playerTwoTag.gameObject.SetActive(true); // Show player two tag
    }

    // Updates the life UI for player one based on remaining lives
    public void HandleLiveUIPlayerOne(int lives)
    {
        if (lives == 2)
        {
            playerOneLifeThree.gameObject.SetActive(false); // Hide third life when 2 lives left
        }
        if (lives == 1)
        {
            playerOneLifeTwo.gameObject.SetActive(false); // Hide second life when 1 life left
        }
        if (lives == 0)
        {
            playerOneLifeOne.gameObject.SetActive(false); // Hide first life when out of lives
        }
    }
    
    // Updates the life UI for player two based on remaining lives
    public void HandleLiveUIPlayerTwo(int lives)
    {
        if (lives == 2)
        {
            playerTwoLifeThree.gameObject.SetActive(false); // Hide third life when 2 lives left
        }
        if (lives == 1)
        {
            playerTwoLifeTwo.gameObject.SetActive(false); // Hide second life when 1 life left
        }
        if (lives == 0)
        {
            playerTwoLifeOne.gameObject.SetActive(false); // Hide first life when out of lives
        }
    }

    // Updates placementicon UI for placement phase --> How many placements to you have left
    public void HandlePlacementsLeft(int placements)
    {
        if (placements == 0)
        {
            placementOne.gameObject.SetActive(false); // Hide first placement icon
        }
        if (placements == 1)
        {
            placementTwo.gameObject.SetActive(false); // Hide second placement icon
        }
    }
}
