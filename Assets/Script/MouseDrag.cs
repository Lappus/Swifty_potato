using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles drag-and-drop mechanics for selectable objects
public class MouseDrag : MonoBehaviour
{
    private HandleHeadUpDisplay handleHeadUpDisplay; // Reference to the UI manager
    private float initialZ; // Stores the initial Z position for maintaining depth
    private Vector3 offset; // Offset between mouse position and object position
    public static int placedObjects = 0; // Counts the number of objects placed
    private int maxPlayableObjects = 2; // Maximum number of objects that can be placed
    public static bool selectionIsOver = false; // Indicates whether the selection phase is over
    
    private GameObject draggedObject; // The currently dragged object

    void Start()
    {
        initialZ = transform.position.z; // Initialize the initial Z position
        handleHeadUpDisplay = FindObjectOfType<HandleHeadUpDisplay>();
    }

    private void OnMouseDown()
    {
        RaycastHit hit; // Store information about what was hit by the ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from the camera to the mouse position

        // Perform a raycast to detect if an object was clicked
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the maximum number of objects has not been placed
            if (placedObjects < maxPlayableObjects)
            {
                // Check if the object is selectable
                if (hit.collider.CompareTag("Selectable"))
                {
                    // Create a copy of the object being dragged
                    draggedObject = Instantiate(gameObject, transform.position, transform.rotation);
                    draggedObject.tag = "SelectableAndMovable"; // Change the tag for the copied object
                    offset = draggedObject.transform.position - GetMouseWorldPosition(); // Calculate the offset
                }
                // If the clicked object is already selectable and movable
                else if (hit.collider.CompareTag("SelectableAndMovable"))
                {
                    draggedObject = hit.collider.gameObject;
                    offset = draggedObject.transform.position - GetMouseWorldPosition(); // calculate the offset
                }
                // Update the UI to show the remaining placements
                handleHeadUpDisplay.HandlePlacementsLeft(placedObjects);
            }
        }
    }

    private void OnMouseDrag()
    {
        // While dragging, move the object if it exists and placements are available
        if (draggedObject != null && placedObjects < maxPlayableObjects)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + offset; // Get target position based on mouse
            targetPosition.z = initialZ; // Maintain the initial Z position
            draggedObject.transform.position = targetPosition; // only move the copy
        }
    }
    
    private void OnMouseUp()
    {
        // When the mouse button is released, finish dragging the object
        if (draggedObject != null)
        {
            // Increase the number of placed objects
            placedObjects++;
            print($"Placed objects = {placedObjects}");
            draggedObject = null; // Reset draggedObject to null after placement
        }

        // Check if the maximum number of usable objects has been reached
        if (placedObjects >= maxPlayableObjects)
        {
            // If so, mark selection as completed
            selectionIsOver = true;
        }
    }
    
    // Converts the mouse position on the screen to a point in the game world
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition; // Get mouse position in screen space
        mousePosition.z = -Camera.main.transform.position.z + initialZ; // Calculate correct Z value for the world position
        return Camera.main.ScreenToWorldPoint(mousePosition); // Convert to world space
    }
}