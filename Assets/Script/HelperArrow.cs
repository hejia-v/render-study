using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperArrow : MonoBehaviour
{
    public Transform target;
    public float dstFromTarget = 2;

    void Start()
    {
        transform.position = target.position - transform.forward * dstFromTarget;
    }

    void Update()
    {
        transform.position = target.position - transform.forward * dstFromTarget;
    }
}
