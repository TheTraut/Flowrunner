using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{
    public float speed = 5f;

    /// <summary>
    /// Controls the side-to-side movement of an object.
    /// </summary>
    void Update()
    {
        if (!PauseManager.IsPaused)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 movement = new(horizontalInput, 0, 0);
            transform.Translate(movement * speed * Time.deltaTime);
        }
    }
}