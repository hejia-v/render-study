using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointControl : MonoBehaviour, IDragHandler
{
    public int objectNumber;
    public GameObject controlObject;
    public GameObject controlObject2;

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = HairStage.cam.ScreenToViewportPoint(Input.mousePosition);
        transform.position = Input.mousePosition;
    }
}
