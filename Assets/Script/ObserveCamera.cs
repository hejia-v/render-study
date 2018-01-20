using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObserveCamera : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;
    public float zoomSpeed = 1;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    void Start()
    {
        UpdateCamera();
    }

    void Update()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        dstFromTarget += distance;
        transform.position = target.position - transform.forward * dstFromTarget;
    }

    public void UpdateCamera()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }


}
