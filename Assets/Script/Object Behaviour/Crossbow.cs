using System;
using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab des Pfeils
    public float fireInterval = 2.0f; // Schussintervall in Sekunden
    public bool isPlacedOnGrid = false;
    public Vector3 fireDirection; // Die Richtung, in die geschossen wird.

    void Start()
    {
        StartCoroutine(FireArrows());
    }

    private void Update()
    {
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
        Vector3 spawnPosition = transform.position + fireDirection.normalized * 0.5f; // Positionierung des Pfeils

        // Erstelle das Arrow-Objekt
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        // Setze die Richtung (Velocity) des Pfeils
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDirection(fireDirection);
        
            // Berechne die Rotation basierend auf der fireDirection
            Vector3 direction = fireDirection.normalized;
            float zRotation = 0f;

            // Bestimme die Y-Rotation basierend auf fireDirection
            if (direction.y == 1)
            {
                zRotation = 180f; // nach oben
            }

            // Bestimme die X-Rotation basierend auf fireDirection
            if (direction.x == 1)
            {
                zRotation = 90f; // nach rechts
            }
            else if (direction.x == -1)
            {
                zRotation = -90f; // nach links
            }

            // Setze die Rotation des Pfeils
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }
    }

    bool CheckIfPlacedOnGrid()
    {
        if (transform.position.x > -9 & transform.position.x < 80 & transform.position.y > -9 &
            transform.position.y < 40)
        {
            return true;
        }
        else return false;
    }
}