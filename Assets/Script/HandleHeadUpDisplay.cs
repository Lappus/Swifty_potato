using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleHeadUpDisplay : MonoBehaviour
{
    public Image playerOneLifeOne;
    public Image playerOneLifeTwo;
    public Image playerOneLifeThree;
    public Image playerTwoLifeOne;
    public Image playerTwoLifeTwo;
    public Image playerTwoLifeThree;

    public TextMeshProUGUI playerOneTag;
    public TextMeshProUGUI playerTwoTag;
    
    public Image placementOne;
    public Image placementTwo;
    public TextMeshProUGUI placementTag;

    private void Start()
    {
        DeactivatePlacementUI();
        ActivatePlayerLifeUI();
    }

    public void DeactivatePlacementUI()
    {
        placementTag.gameObject.SetActive(false);
        placementOne.gameObject.SetActive(false);
        placementTwo.gameObject.SetActive(false);
    }

    public void ActivatePlacementUI()
    {
        placementTag.gameObject.SetActive(true);
        placementOne.gameObject.SetActive(true);
        placementTwo.gameObject.SetActive(true);
    }

    public void DeactivatePlayerLifeUI()
    {
        playerOneLifeOne.gameObject.SetActive(false);
        playerOneLifeTwo.gameObject.SetActive(false);
        playerOneLifeThree.gameObject.SetActive(false);
        playerOneTag.gameObject.SetActive(false);
        playerTwoLifeOne.gameObject.SetActive(false);
        playerTwoLifeTwo.gameObject.SetActive(false);
        playerTwoLifeThree.gameObject.SetActive(false);
        playerTwoTag.gameObject.SetActive(false);
    }

    public void ActivatePlayerLifeUI()
    {
        playerOneLifeOne.gameObject.SetActive(true);
        playerOneLifeTwo.gameObject.SetActive(true);
        playerOneLifeThree.gameObject.SetActive(true);
        playerOneTag.gameObject.SetActive(true);
        playerTwoLifeOne.gameObject.SetActive(true);
        playerTwoLifeTwo.gameObject.SetActive(true);
        playerTwoLifeThree.gameObject.SetActive(true);
        playerTwoTag.gameObject.SetActive(true);
    }

    public void HandleLiveUIPlayerOne(int lives)
    {
        if (lives == 2)
        {
            playerOneLifeThree.gameObject.SetActive(false);
        }
        if (lives == 1)
        {
            playerOneLifeTwo.gameObject.SetActive(false);
        }
        if (lives == 0)
        {
            playerOneLifeOne.gameObject.SetActive(false);
        }
    }
    
    public void HandleLiveUIPlayerTwo(int lives)
    {
        if (lives == 2)
        {
            playerTwoLifeThree.gameObject.SetActive(false);
        }
        if (lives == 1)
        {
            playerTwoLifeTwo.gameObject.SetActive(false);
        }
        if (lives == 0)
        {
            playerTwoLifeOne.gameObject.SetActive(false);
        }
    }

    public void HandlePlacementsLeft(int placements)
    {
        if (placements == 0)
        {
            placementOne.gameObject.SetActive(false);
        }
        if (placements == 1)
        {
            placementTwo.gameObject.SetActive(false);
        }
    }
}
