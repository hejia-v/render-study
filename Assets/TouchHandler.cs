using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public ObserveCamera observeCamera;

    void Start()
    {

    }

    void Update()
    {

    }

    void UpdateCamera()
    {
        if (observeCamera!=null)
        {
            observeCamera.UpdateCamera();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateCamera();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateCamera();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateCamera();
    }
}
