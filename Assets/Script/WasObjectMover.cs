using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public static bool placingPhase = false;
    private GameObject selectedObject;
    private GameObject copiedObject;
    private Camera mainCamera;
    private bool selected = false;

    public static int selections = 0;
    private int maxSelections = 2;

    public static bool selectionIsOver = false;

    void Start()
    {
        mainCamera = Camera.main;

    }

    void Update()
    {
        if (placingPhase && !selectionIsOver)
        {
            if (Input.GetMouseButtonDown(0) && !selected && copiedObject == null) // Linksklick für Auswahl
            {
                selected = true;
                HandleSelection();
                print("selected the object");
            }

            if (copiedObject != null)
            {
                MoveObject();
            }

            if (Input.GetMouseButtonDown(1) && copiedObject != null) // Rechtsklick für Platzierung
            {
                selected = true;
                PlaceObject();
                selected = false;
            }
        }
    }

    private void HandleSelection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Selectable"))
        {
            if (selections < maxSelections)
            {
                selectedObject = hit.collider.gameObject;
                copiedObject = Instantiate(selectedObject, selectedObject.transform.position, Quaternion.identity);
                copiedObject.SetActive(true); // Sicherstellen, dass das Objekt aktiv ist
            }
        }
    }

    private void MoveObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.z = copiedObject.transform.position.z; // Z-Position beibehalten
            copiedObject.transform.position = newPosition;
        }
    }

    private void PlaceObject()
    {
        selections++;
        Destroy(copiedObject); // Das kopierte Objekt bei der Platzierung entfernen
        copiedObject = null; // Kopie auf null setzen, damit eine neue erstellt werden kann

        // Überprüfen, ob die Auswahlphase vorbei ist
        if (selections >= maxSelections)
        {
            selectionIsOver = true;
            // Hier könntest du eventuell die Kamera für die nächste Phase umstellen
            Debug.Log("Auswahlphase beendet!");
        }
    }
}
