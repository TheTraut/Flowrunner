
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(-1f, 5f)]
    public float scrollSpeed = 0.5f;
    private float offset;
    private Material mat;

    /// <summary>
    /// Scrolls the background texture horizontally.
    /// </summary>
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    /// <summary>
    /// Updates the offset of the background texture to create scrolling effect.
    /// </summary>
    void Update()
    {
        if (!PauseManager.isPaused)
        {
            offset += (Time.deltaTime * scrollSpeed) / 10f;
            mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}
