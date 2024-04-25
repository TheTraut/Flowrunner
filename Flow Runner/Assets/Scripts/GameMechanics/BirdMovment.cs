using System.Collections;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float minAmplitude = 3f; // Minimum amplitude of the oscillation
    public float maxAmplitude = 7f; // Maximum amplitude of the oscillation
    public float minFrequency = 0.5f; // Minimum frequency of the oscillation
    public float maxFrequency = 2f; // Maximum frequency of the oscillation
    public bool useSine = true; // Use sine function if true, cosine if false

    private float amplitude; // Actual amplitude for this instance
    private float frequency; // Actual frequency for this instance
    private Vector3 startPosition;
    private Coroutine oscillationCoroutine; // Coroutine reference

    /// <summary>
    /// Handles the oscillating movement of the bird.
    /// </summary>
    void Start()
    {
        // Set random amplitude and frequency for this instance
        amplitude = Random.Range(minAmplitude, maxAmplitude);
        frequency = Random.Range(minFrequency, maxFrequency);

        startPosition = transform.position;
        oscillationCoroutine = StartCoroutine(Oscillate());
    }

    /// <summary>
    /// Coroutine for oscillating the bird's position.
    /// </summary>
    private IEnumerator Oscillate()
    {
        while (true)
        {
            if (!PauseManager.isPaused)
            {
                float oscillation = useSine ? Mathf.Sin(Time.time * frequency) : Mathf.Cos(Time.time * frequency);
                transform.position = startPosition + new Vector3(oscillation * amplitude, 0f, 0f);
            }
            yield return null;
        }
    }

    /// <summary>
    /// Stops the oscillation coroutine.
    /// </summary>
    public void StopOscillation()
    {
        if (oscillationCoroutine != null)
            StopCoroutine(oscillationCoroutine);
    }

    /// <summary>
    /// Resumes the oscillation coroutine.
    /// </summary>
    public void ResumeOscillation()
    {
        if (oscillationCoroutine == null)
            oscillationCoroutine = StartCoroutine(Oscillate());
    }
}
