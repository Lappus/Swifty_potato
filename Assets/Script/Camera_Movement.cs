using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    // date nur nach x und y up und nicht nach z
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = target.transform.position + offset;
            transform.position = newPosition;    
        }
    }
}
