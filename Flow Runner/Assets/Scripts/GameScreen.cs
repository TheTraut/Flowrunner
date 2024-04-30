using UnityEngine;

/// <summary>
/// Manages interactions and UI elements on the game screen.
/// </summary>
public class GameScreen : MonoBehaviour
{
    /// <summary>
    /// Initializes the game screen by loading the highscore instance.
    /// </summary>
    private void Awake()
    {
        HighscoresManager.Instance.LoadHighscores();
    }

    private void update()
    {
        DestroyInactiveModals<PauseModalWindow>();
        DestroyInactiveModals<SettingsModalWindow>();
    }

    /// <summary>
    /// Destroys any modal windows that are not visible.
    /// </summary>
    private void DestroyInactiveModals<T>() where T : ModalWindow<T>
    {
        // Find all active modal windows
        T[] activeModals = FindObjectsOfType<T>();

        foreach (T modal in activeModals)
        {
            // Check if the modal window is not visible and its close animation is not playing
            if (!modal.Visible && IsClosingAnimationPlaying(modal))
            {
                Destroy(modal.gameObject);
            }
        }
    }
    
    private bool IsClosingAnimationPlaying<T>(T modal) where T : ModalWindow<T>
    {
        // Check if the modal's animation state is the closing animation
        return modal.animator.GetCurrentAnimatorStateInfo(0).IsName("Close");
    }
}