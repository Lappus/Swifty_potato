using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rotation for the trapdoor
public class rotating : MonoBehaviour
{
    private bool isPlacedOnGrid = false;
    private bool isCoroutineRunning = false; // To check if coroutine is already running

    private void Update()
    {
        bool previousPlacedOnGrid = isPlacedOnGrid;
        // bool for checking if the trapdoor is placed on the active placement area
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
            // Gradual rotation down to -90 degrees
            float rotationDuration = 0.75f; // Duration of the downward rotation
            float targetRotationDown = -90f; // Target angle for downward rotation
            float startRotationDown = transform.rotation.eulerAngles.z; // Starting angle
            float t = 0f;
        
            while (t < 1f)
            {
                t += Time.deltaTime / rotationDuration;
                float currentRotation = Mathf.Lerp(startRotationDown, targetRotationDown, t * t);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotation);
                yield return null;
            }

            // wait for 3 seconds
            yield return new WaitForSeconds(3f);

            // Gradually rotate back to 0 degrees
            // Gradually rotate the trapdoor back up to 0 degrees
            t = 0f; // Reset time multiplier for the return rotation
            float targetRotationUp = 0f; // Target angle for return rotation
            float startRotationUp = -90f; // Starting angle for return
            while (t < 1f)
            {
                t += Time.deltaTime / rotationDuration; // Increment time factor
                float currentRotation = Mathf.Lerp(startRotationUp, targetRotationUp, t * t); // Same smoothing technique
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, currentRotation);
                yield return null; // Continue coroutine to next frame
            }
        }
    }
    
    private bool CheckIfPlacedOnGrid()
    {
        // Active placement area
        if (transform.position.x > -9 & transform.position.x < 80 & transform.position.y > -9 &
         transform.position.y < 40)
        {
            return true;
        }
        else return false;
    }
}
