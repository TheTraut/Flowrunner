using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadSceneAsync("Title Screen");
    }
}
