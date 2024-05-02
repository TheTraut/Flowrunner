using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IGuideTextsPage
{
    void Hide();
    void Show();
}

/// <summary>
/// Manages the highscores modal window.
/// </summary>
public class GuideModalWindow : ModalWindow<GuideModalWindow>
{
    [Serializable]
    private class GuideTextsPageObject
    {
        public IGuideTextsPage page;

        public GuideTextsPageObject(IGuideTextsPage page)
        {
            this.page = page;
        }
    }

    [Serializable]
    public class GuideTextsPage1 : IGuideTextsPage
    {
        // kase, create copies for each page
        // we can iterate over these things easy
        // so like if i click the next button, then iterate over page1 and hide and then do the same for page2 but show
        public GameObject pageParent;
        public TMP_Text bodyText;

        // kase, these are just examples for how to show the settings stuff
        public TMP_Text nameText;
        public TMP_Text volumeText;
        public TMP_Text[] keyTexts;

        // kase, the HidePageObjects(pageParent); is required, but if you needed to run any other functions then put them after the hide
        public void Hide()
        {
            HidePageObjects(pageParent);
        }

        // kase, the ShowPageObjects(pageParent); is required, but if you needed to run any other functions then put them before the show
        // like how i am setting the vars by getting the settings instance
        public void Show()
        {
            SetFromSettings();
            ShowPageObjects(pageParent);
        }

        public void SetFromSettings()
        {
            // Set name text
            if (nameText != null)
            {
                nameText.text = SettingsManager.Instance.PlayerName;
            }

            // Set volume text
            if (volumeText != null)
            {
                int volumePercentage = Mathf.RoundToInt(SettingsManager.Instance.Volume);
                volumeText.text = volumePercentage.ToString() + "%";
            }

            // Set key texts
            if (keyTexts != null && keyTexts.Length >= 3)
            {
                keyTexts[0].text = SettingsManager.Instance.GetFormattedKeyShortcut(SettingsManager.KeyType.Up);
                keyTexts[1].text = SettingsManager.Instance.GetFormattedKeyShortcut(SettingsManager.KeyType.Down);
                keyTexts[2].text = SettingsManager.Instance.GetFormattedKeyShortcut(SettingsManager.KeyType.Shield);
            }
        }
    }

    [Serializable]
    public class GuideTextsPage2 : IGuideTextsPage
    {
        // kase, this is a blank one to setup whatever
        public GameObject pageParent;
        public TMP_Text bodyText;

        // kase, setup other ui items here to hook in from the editor

        // kase, modify these if you need to add any other functions
        public void Hide()
        {
            HidePageObjects(pageParent);
        }

        public void Show()
        {
            ShowPageObjects(pageParent);
        }
    }

    #pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] private GuideTextsPage1 guideTextsPage1;
    [SerializeField] private GuideTextsPage2 guideTextsPage2;
    //[SerializeField] private GuideTextsPage3 guideTextsPage3;
    // kase, etc.
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    #pragma warning restore IDE0044 // Add readonly modifier

    private GuideTextsPageObject[] guideTextsPages;
    private int currentPageIndex = 0;
    private readonly int TOTAL_PAGES = 2; // Update when more pages

    /// <summary>
    /// Opens the highscores modal.
    /// </summary>
    /// <returns>The current instance of the highscores modal window.</returns>
    public GuideModalWindow Guide()
    {
        return this;
    }

    /// <summary>
    /// Loads when the object is awake.
    /// </summary>
    private void Awake()
    {
        SettingsManager.Instance.Load();
        InitializeGuideTextsPagesArray();
        HideAllGuideTexts();
        UpdateButtonVisibility();
        prevButton.onClick.AddListener(PrevButton_OnClick);
        nextButton.onClick.AddListener(NextButton_OnClick);

        ShowPage(currentPageIndex);
    }

    private void InitializeGuideTextsPagesArray()
    {
        guideTextsPages = new GuideTextsPageObject[]
        {
            new(guideTextsPage1),
            new(guideTextsPage2),
            //new(guideTextsPage3),
            // kase, etc.
        };
    }

    public override GuideModalWindow Close()
    {
        Instance = null;
        base.Close();
        return Instance;
    }

    /// <summary>
    /// Closes the modal.
    /// </summary>
    public void CloseGuide()
    {
        Close();
    }

    private void UpdateButtonVisibility()
    {
        bool canPrev = false;
        bool canNext = false;

        // Determine if previous button should be visible
        if (currentPageIndex > 0)
        {
            canPrev = true;
        }

        // Determine if next button should be visible
        if (currentPageIndex < TOTAL_PAGES - 1)
        {
            canNext = true;
        }

        SetButtonVisibility(prevButton, canPrev);
        SetButtonVisibility(nextButton, canNext);
    }

    private void SetButtonVisibility(Button button, bool isVisible)
    {
        if (button.TryGetComponent<CanvasGroup>(out var canvasGroup))
        {
            canvasGroup.alpha = isVisible ? 1f : 0f;
            canvasGroup.blocksRaycasts = isVisible;
        }
    }

    private void HideAllGuideTexts()
    {
        foreach (var pageObject in guideTextsPages)
        {
            pageObject.page.Hide(); // Check if the page implements IGuideTextsPage and call Hide() method
        }
    }

    private void ShowPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < guideTextsPages.Length)
        {
            HideAllGuideTexts();
            guideTextsPages[pageIndex].page.Show(); // Check if the page implements IGuideTextsPage and call Show() method
        }
        else
        {
            Debug.LogError("Invalid page index.");
        }
    }

    public static void HidePageObjects(GameObject pageParent)
    {
        if (pageParent != null)
        {
            foreach (Transform child in pageParent.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public static void ShowPageObjects(GameObject pageParent)
    {
        if (pageParent != null)
        {
            foreach (Transform child in pageParent.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void PrevButton_OnClick()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateButtonVisibility();
            ShowPage(currentPageIndex);
        }
    }

    private void NextButton_OnClick()
    {
        if (currentPageIndex < guideTextsPages.Length - 1)
        {
            currentPageIndex++;
            UpdateButtonVisibility();
            ShowPage(currentPageIndex);
        }
    }
}