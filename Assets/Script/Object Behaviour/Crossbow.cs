using System;
using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab of arrow
    public float fireInterval = 2.0f; // shooting interval of arrow
    public bool isPlacedOnGrid = false;
    public Vector3 fireDirection; // direction in which arrow is to be shooted.

    void Start()
    {
        StartCoroutine(FireArrows());
    }

    private void Update()
    {
        // Crossbow only shoots while being in the active placement area
        isPlacedOnGrid = CheckIfPlacedOnGrid();
    }

    private IEnumerator FireArrows()
    {
        while (true)
        {
            if (PlayerGoalStatus.gameplayStarted & isPlacedOnGrid)
            {
                FireArrow();
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }
    
    void FireArrow()
    {
        // Positioning of arrow
        Vector3 spawnPosition = transform.position + fireDirection.normalized * 0.5f; 

        // create the arrow object
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        // set the direction (Velocity) of the arrow
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDirection(fireDirection);
        
            // Calculate the rotation based on the fireDirection
            Vector3 direction = fireDirection.normalized;
            float zRotation = 0f;

            // Calculate the Y-rotation based on fireDirection
            if (direction.y == 1)
            {
                zRotation = 180f; // up
            }

            // Calculate the X-rotation based on fireDirection
            if (direction.x == 1)
            {
                zRotation = 90f; // to the right
            }
            else if (direction.x == -1)
            {
                zRotation = -90f; // to the left
            }

            // set the rotation of the arrow
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }
    }

    bool CheckIfPlacedOnGrid()
    {
        // active placement area
        if (transform.position.x > -9 & transform.position.x < 80 & transform.position.y > -9 &
            transform.position.y < 40)
        {
            return true;
        }
        else return false;
    }
}