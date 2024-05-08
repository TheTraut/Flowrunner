using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
