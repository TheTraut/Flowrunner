using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 posOffset;
    public float smooth;

    Vector3 velocity;

    public Image pauseButtonImage; // Reference to the pause button's Image component
    public Sprite pauseSprite; // Sprite for the pause button
    public Sprite playSprite; // Sprite for the play (resume) button

    private void LateUpdate()
    {
        if (!PauseManager.isPaused)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + posOffset, ref velocity, smooth);
        }
    }

    void Awake()
    {
        pauseButtonImage.sprite = pauseSprite;
    }

    public void PauseGame()
    {
        PauseManager.Pause();
    }

    public void TogglePause()
    {
        if (PauseManager.isPaused)
        {
            ResumeGame();
            pauseButtonImage.sprite = pauseSprite; // Change button image to pause icon
        }
        else
        {
            PauseGame();
            pauseButtonImage.sprite = playSprite; // Change button image to play (resume) icon
        }
    }

    void ResumeGame()
    {
        PauseManager.Resume();
    }
}