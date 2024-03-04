using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
