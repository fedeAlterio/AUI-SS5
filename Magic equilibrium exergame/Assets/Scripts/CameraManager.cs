using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public float smoothingSpeed = 0.125f;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = target.position + offset;

        //Get to the desired position slowly so to avoid jittering movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothingSpeed * Time.smoothDeltaTime);
        transform.position = smoothedPosition;
    }
}
