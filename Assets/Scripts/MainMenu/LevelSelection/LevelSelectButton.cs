using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controls level select button appearance and on click action
/// </summary>
public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private Sprite completedLevelIcon;
    [SerializeField] private Sprite normalLevelIcon;
    [SerializeField] private Sprite lockedLevelIcon;
    [SerializeField] private TMP_Text buttonText;

    [Tooltip("Index of corresponding Level from LevelsData")]
    [SerializeField] private int levelIndex;

    void Awake()
    {
        bool isUnlocked = false;
        bool isCompleted = false;

        if (levelIndex == 0 || LevelCompletionApi.IsLevelCompleted(levelIndex - 1))
        {
            isUnlocked = true;
        }

        if (LevelCompletionApi.IsLevelCompleted(levelIndex))
        {
            isCompleted = true;
        }

        SetupButtonAppearance(isCompleted, isUnlocked);

        if (isUnlocked) SetupOnClickAction();
    }

    private void SetupOnClickAction()
    {
        SceneLoader sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
        GetComponent<Button>().onClick.AddListener(() => sceneLoader.LoadLevelScene(levelIndex));
    }

    private void SetupButtonAppearance(bool isCompleted, bool isUnlocked)
    {
        if (isCompleted && isUnlocked)
        {
            SetupCompletedLevelAppearance();

        }
        else if (!isCompleted && isUnlocked)
        {
            SetupNormalLevelAppearance();
        }
        else
        {
            SetupLockedLevelAppearance();
        }
    }

    private void SetupCompletedLevelAppearance()
    {
        GetComponent<Image>().sprite = completedLevelIcon;
        AddLevelNumberText();
    }

    private void SetupNormalLevelAppearance()
    {
        GetComponent<Image>().sprite = normalLevelIcon;
        AddLevelNumberText();
    }

    private void SetupLockedLevelAppearance()
    {
        GetComponent<Image>().sprite = lockedLevelIcon;

        Destroy(GetComponent<Button>());
    }

    private void AddLevelNumberText()
    {
        // level number is levelIndex + 1 (because level counting starts from 1)
        buttonText.text = (levelIndex + 1) + "";
    }
}
