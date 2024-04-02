using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovment : MonoBehaviour
{
    public enum OccilationFuntion { Sine, Cosine }
    // Start is called before the first frame update
    public float speed = 0.1f;
    public float velocity1 = 1f;
    public float velocity2 = 1f;
    void Start()
    {
        //to start at zero
        StartCoroutine(Oscillate(OccilationFuntion.Sine, 5f));
        //to start at scalar value
        //StartCoroutine (Oscillate (OccilationFuntion.Cosine, 1f));
    }

    private IEnumerator Oscillate(OccilationFuntion method, float scalar)
    {
        while (true)
        {
            if (method == OccilationFuntion.Sine)
            {
                transform.position = new Vector3((Mathf.Sin(Time.time) * scalar) + velocity1, 5f, 0);
            }
            else if (method == OccilationFuntion.Cosine)
            {
                transform.position = new Vector3((Mathf.Cos(Time.time) * scalar) + velocity2, 1f, 0);
            }
            yield return new WaitForEndOfFrame();
            velocity1 -= speed;
            velocity2 -= speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
