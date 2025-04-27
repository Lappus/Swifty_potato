using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeRotating : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(Rotating), 4f, 8f);
    }

    void Rotating()
    {
        StartCoroutine(RotateCoroutine());
    }
    private IEnumerator RotateCoroutine()
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
