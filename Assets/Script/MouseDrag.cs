using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private HandleHeadUpDisplay handleHeadUpDisplay;
    private float initialZ;
    private Vector3 offset;
    public static int placedObjects = 0;
    private int maxPlayableObjects = 2;
    public static bool selectionIsOver = false;
    
    private GameObject draggedObject; // Das aktuell von der Maus gezogene Objekt

    void Start()
    {
        initialZ = transform.position.z;
        handleHeadUpDisplay = FindObjectOfType<HandleHeadUpDisplay>();
    }

    private void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (placedObjects < maxPlayableObjects)
            {
                if (hit.collider.CompareTag("Selectable"))
                {
                    draggedObject = Instantiate(gameObject, transform.position, transform.rotation);
                    draggedObject.tag = "SelectableAndMovable";
                    offset = draggedObject.transform.position - GetMouseWorldPosition();
                }
                else if (hit.collider.CompareTag("SelectableAndMovable"))
                {
                    draggedObject = hit.collider.gameObject; // Wähle das bewegbare Objekt
                    offset = draggedObject.transform.position - GetMouseWorldPosition();
                }
                handleHeadUpDisplay.HandlePlacementsLeft(placedObjects);
            }
        }
    }

    private void OnMouseDrag()
    {
        if (draggedObject != null && placedObjects < maxPlayableObjects)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + offset;
            targetPosition.z = initialZ; // Setze die Z-Koordinate auf den ursprünglichen Wert
            draggedObject.transform.position = targetPosition; // Bewege nur die Kopie
        }
    }
    
    private void OnMouseUp()
    {
        if (draggedObject != null)
        {
            placedObjects++;
            print($"Placed objects = {placedObjects}");

            // Hier kannst du zusätzliche Logik hinzufügen, um das Kopierte-Objekt zu aktivieren oder zu konfigurieren
            draggedObject = null; // Setze draggedObject zurück
        }

        if (placedObjects >= maxPlayableObjects)
        {
            selectionIsOver = true;
            print($"Selection Done");
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z + initialZ;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}