using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotating : MonoBehaviour
{
    private bool isPlacedOnGrid = false;
    private bool isCoroutineRunning = false; // To check if coroutine is already running

    private void Update()
    {
        bool previousPlacedOnGrid = isPlacedOnGrid;
        isPlacedOnGrid = CheckIfPlacedOnGrid();

        // Start coroutine when object is placed on the grid and not already rotating
        if (isPlacedOnGrid && !previousPlacedOnGrid && !isCoroutineRunning)
        {
            InvokeRepeating(nameof(Rotating), 4f, 8f);
        }
        else if (!isPlacedOnGrid && previousPlacedOnGrid)
        {
            // Stop rotating if it is moved off the grid
            CancelInvoke(nameof(Rotating));
            isCoroutineRunning = false;
        }
    }

    void Rotating()
    {
        StartCoroutine(RotateCoroutine());
    }
    private IEnumerator RotateCoroutine()
    {
        isCoroutineRunning = true;
        
        if (PlayerGoalStatus.gameplayStarted & isPlacedOnGrid)
        {
            // Graduelle Drehung bis zu -90 Grad
            float rotationDuration = 0.75f;
            float targetRotationDown = -90f;
            float startRotationDown = transform.rotation.eulerAngles.z;
            float t = 0f;
        
            while (t < 1f)
            {
                t += Time.deltaTime / rotationDuration;
                float currentRotation = Mathf.Lerp(startRotationDown, targetRotationDown, t * t);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotation);
                yield return null;
            }

            // Wartezeit von 3 Sekunden
            yield return new WaitForSeconds(3f);

            // Graduelle Drehung zurück auf 0 Grad
            t = 0f;
            float targetRotationUp = 0f;
            float startRotationUp = -90f;
            while (t < 1f)
            {
                t += Time.deltaTime / rotationDuration;
                float currentRotation = Mathf.Lerp(startRotationUp, targetRotationUp, t * t);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotation);
                yield return null;
            }
        }
    }
    
    private bool CheckIfPlacedOnGrid()
    {
        if (transform.position.x > -9 & transform.position.x < 80 & transform.position.y > -9 &
         transform.position.y < 40)
        {
            return true;
        }
        else return false;
    }
}
