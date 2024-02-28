using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 posOffset;
    public float smooth;

    Vector3 velocity;

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + posOffset, ref velocity, smooth);
    }

}
