using UnityEngine;
using UnityEngine.SceneManagement;

public class Guide : MonoBehaviour
{
    public void toTitleScreen()
    {
        SceneManager.LoadSceneAsync("Title Screen");
    }

    public void toGuide1()
    {
        SceneManager.LoadSceneAsync("Guide1");
    }
    public void toGuide2()
    {
        SceneManager.LoadSceneAsync("Guide2");
    }
    public void toGuide3()
    {
        SceneManager.LoadSceneAsync("Guide3");
    }
    public void toGuide4()
    {
        SceneManager.LoadSceneAsync("Guide4");
    }
}