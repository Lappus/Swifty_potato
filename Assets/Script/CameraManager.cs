using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject gameplayCanvas;
    public CinemachineVirtualCamera gameplayCamera;
    public CinemachineVirtualCamera selectionCamera;

    private CinemachineVirtualCamera currentCamera;
    void Start()
    {
        //currentCamera = selectionCamera;
        currentCamera = gameplayCamera;
        gameplayCanvas.SetActive(true);
    }
    
    void Update()
    {
        if (currentCamera != GetActiveCamera())
        {
            if (GetActiveCamera() == gameplayCamera)
            {
                gameplayCanvas.SetActive(true); // Aktiviere das Canvas
            }
            else
            {
                gameplayCanvas.SetActive(false); // Deaktiviere das Canvas
            }
            currentCamera = GetActiveCamera();
        }
    }
    private CinemachineVirtualCamera GetActiveCamera()
    {
        return FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
    }
    
}
