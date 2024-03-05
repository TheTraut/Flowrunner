using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BirdController;

public class BirdMovment : MonoBehaviour
{
    public enum OccilationFuntion { Sine, Cosine }
    // Start is called before the first frame update
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
                transform.position = new Vector3(Mathf.Sin(Time.time) * scalar, 5f, 0);
            }
            else if (method == OccilationFuntion.Cosine)
            {
                transform.position = new Vector3(Mathf.Cos(Time.time) * scalar, 1f, 0);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
