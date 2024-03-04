using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRise : MonoBehaviour
{
    public float riseSpeed = 0.1f; // Speed at which the water rises

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the water object upwards at the specified rise speed
        transform.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
    }
}
