using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void SeeGuide()
    {
        SceneManager.LoadSceneAsync("Guide1");
    }

    public void SeeHighScores()
    {
        SceneManager.LoadSceneAsync("High Scores");
    }

    public void SeeSettings()
    {
        SceneManager.LoadSceneAsync("Settings");
    }
}
