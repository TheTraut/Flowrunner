using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Guide : MonoBehaviour
{
    [Serializable]
    public class GuideTexts
    {
        public TMP_Text nameText;
        public TMP_Text volumeText;
        public TMP_Text[] keyTexts;
    }

    /// <summary>
    /// Loads settings when the object is awake.
    /// </summary>
    private void Awake()
    {
        SettingsManager.Instance.Load();
    }

    public void ToTitleScreen()
    {
        SceneManager.LoadSceneAsync("Title Screen");
    }
}