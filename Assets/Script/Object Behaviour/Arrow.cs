using UnityEngine;


public class Arrow : MonoBehaviour
{
    private Vector3 direction;
    public float speed = 10f; // Geschwindigkeit des Pfeils

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized; // Normalisiere die Richtung
    }

    void Update()
    {
        // Bewege den Pfeil in die gewählte Richtung
        transform.position += direction * speed * Time.deltaTime;

        // Überprüfe, ob der Pfeil die Grenzen erreicht
        if (transform.position.x < -9 || transform.position.x > 80 || transform.position.y < -9 || transform.position.y > 40)
        { 
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Crossbow"))
        {
            //Debug.Log($"Arrow triggered with: {other.gameObject.name}");
            // Zerstöre den Pfeil bei Trigger-Kollision
            Destroy(gameObject);
        }
    }
}