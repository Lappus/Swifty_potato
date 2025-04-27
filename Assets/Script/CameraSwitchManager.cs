using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchManager : MonoBehaviour
{
    public GameObject gameplayCamera;
    public GameObject scoreCamera;
    public GameObject selectionCamera;
    void Start()
    //{   
     //   gameplayCamera.SetActive(true);
      //  scoreCamera.SetActive(false);
      //  selectionCamera.SetActive(false);
    //}
    {   
        gameplayCamera.SetActive(false);
        scoreCamera.SetActive(false);
        selectionCamera.SetActive(true);
    }

    public void SwitchToScoreCamera()
    {
        gameplayCamera.SetActive(false);
        scoreCamera.SetActive(true);
        selectionCamera.SetActive(false);
    }

    public void SwitchToGameplayCamera()
    {
        gameplayCamera.SetActive(true);
        scoreCamera.SetActive(false);
        selectionCamera.SetActive(false);
    }

    public void SwitchToSelectionCamera()
    {
        gameplayCamera.SetActive(false);
        scoreCamera.SetActive(false);
        selectionCamera.SetActive(true);
    }
}
