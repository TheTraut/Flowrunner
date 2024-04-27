using System.Collections;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float minAmplitude = 3f; // Minimum amplitude of the oscillation
    public float maxAmplitude = 7f; // Maximum amplitude of the oscillation
    public float minFrequency = 0.5f; // Minimum frequency of the oscillation
    public float maxFrequency = 2f; // Maximum frequency of the oscillation
    public bool useSine = true; // Use sine function if true, cosine if false

    private const float startPositionX = 0f; // Constant start position for the bird
    private float amplitude; // Actual amplitude for this instance
    private float frequency; // Actual frequency for this instance
    private Vector3 startPosition;
    private float pauseTime; // Time when the game was paused
    private float elapsedTimePaused; // Time elapsed while the game was paused
    private float lastOscillationTime; // Last recorded time within the oscillation cycle

    private Coroutine oscillationCoroutine; // Coroutine reference

    void Start()
    {
        // Set random amplitude and frequency for this instance
        amplitude = Random.Range(minAmplitude, maxAmplitude);
        frequency = Random.Range(minFrequency, maxFrequency);

        startPosition = new Vector3(startPositionX, transform.position.y, transform.position.z);
        oscillationCoroutine = StartCoroutine(Oscillate());
    }

    private IEnumerator Oscillate()
    {
        while (true)
        {
            if (!PauseManager.isPaused)
            {
                float startTime = Time.time - elapsedTimePaused - lastOscillationTime; // Calculate start time for current oscillation cycle
                while (!PauseManager.isPaused)
                {
                    float currentTime = Time.time - elapsedTimePaused; // Calculate current time since the game was unpaused
                    float elapsedTime = currentTime - startTime; // Calculate elapsed time for the current oscillation cycle
                    float oscillation = useSine ? Mathf.Sin((currentTime - startTime) * frequency) : Mathf.Cos((currentTime - startTime) * frequency);
                    transform.position = startPosition + new Vector3(oscillation * amplitude, 0f, 0f);
                    lastOscillationTime = elapsedTime;
                    yield return null;
                }
            }
            else
            {
                pauseTime = Time.time; // Store the time when the game is paused
                yield return null;
            }
        }
    }

    public void StopOscillation()
    {
        if (oscillationCoroutine != null)
            StopCoroutine(oscillationCoroutine);
    }

    public void ResumeOscillation()
    {
        if (oscillationCoroutine == null)
        {
            elapsedTimePaused += Time.time - pauseTime;
            oscillationCoroutine = StartCoroutine(Oscillate());
        }
    }
}
