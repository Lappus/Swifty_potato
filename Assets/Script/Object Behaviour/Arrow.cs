using UnityEngine;


public class Arrow : MonoBehaviour
{
    private Vector3 direction;
    // Speed of the Arrow
    public float speed = 10f; 

    public void SetDirection(Vector3 dir)
    {
        // Normalize the direction
        direction = dir.normalized; 
    }

    void Update()
    {
        // move the arrow in the selected direction
        transform.position += direction * speed * Time.deltaTime;

        // destroy the arrow if it reaches the border of the map
        if (transform.position.x < -9 || transform.position.x > 80 || transform.position.y < -9 || transform.position.y > 40)
        { 
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // arrow is spawning in the crossbow object. Dont destroy. Otherwise destroy it on trigger-collision
        if (other.gameObject.layer != LayerMask.NameToLayer("Crossbow"))
        {
            Destroy(gameObject);
        }
    }
}