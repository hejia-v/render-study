using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObserveCamera : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;
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
        transform.position = target.position - transform.forward * dstFromTarget;
    }

    private void UpdateCamera()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateCamera();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //float s = Input.GetAxis("Mouse X");
        //Debug.Log("22    " + s);
        //Debug.Log("-----------" );

        UpdateCamera();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateCamera();
    }
}
