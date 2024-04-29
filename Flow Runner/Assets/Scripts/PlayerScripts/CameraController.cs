using UnityEngine;
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

    public float defaultWidth = 9.6f; // Width in units you design in.

    /// <summary>
    /// Controls the camera's movement to follow a target with a specified offset.
    /// </summary>
    private void LateUpdate()
    {
        if (!PauseManager.IsPaused && target != null) // Add null check for target transform
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + posOffset, ref velocity, smooth);
            // Add this line to adjust the camera's orthographic size based on the current screen aspect ratio
            Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;
        }
    }

    /// <summary>
    /// Updates the camera's position and adjusts its orthographic size based on the screen aspect ratio.
    /// </summary>
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Initializes the pause button's image sprite.
    /// </summary>
    void Awake()
    {
        pauseButtonImage.sprite = pauseSprite;
    }

    /// <summary>
    /// Toggles the game's pause state.
    /// </summary>
    public void TogglePause()
    {
        if (PauseManager.IsPaused)
        {
            ResumeGame();
            pauseButtonImage.sprite = pauseSprite; // Change button image to pause icon
        }
        else
        {
            PauseModalWindow modalWindow = PauseModalWindow.Create()
                .SetHeader("Game is Paused")
                .PauseMenu()
                .Show();
            PauseGame();
            pauseButtonImage.sprite = playSprite; // Change button image to play (resume) icon
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        PauseManager.Pause();
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    void ResumeGame()
    {
        PauseManager.Resume();
    }
}