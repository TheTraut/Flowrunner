using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{
    public float speed = 5f;

    /// <summary>
    /// Controls the side-to-side movement of an object.
    /// </summary>
    void Update()
    {
        if (!PauseManager.isPaused)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(horizontalInput, 0, 0);
            transform.Translate(movement * speed * Time.deltaTime);
        }
    }
}