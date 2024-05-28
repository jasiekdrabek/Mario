using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject highScorePanel;
    public void PlayGame()
    {
        GameManager.Instance.NewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gra zakończona");
    }

    public void ShowHighScores()
    {
        if (highScorePanel != null)
        {
            highScorePanel.SetActive(true);
            HighScoreDisplay highScoreDisplay = highScorePanel.GetComponent<HighScoreDisplay>();
            if (highScoreDisplay != null)
            {
                highScoreDisplay.DisplayHighScores();
            }
            else
            {
                Debug.LogError("MainMenu: HighScoreDisplay component not found on highScorePanel.");
            }
        }
        else
        {
            Debug.LogError("MainMenu: highScorePanel not assigned!");
        }
    }

    public void HideHighScores()
    {
        highScorePanel.SetActive(false);
    }


    public void SetLevelOptions(bool isBasicLevel, bool isAdvancedLevel)
    {
        GameManager.Instance.isBasicLevelSelected = isBasicLevel;
        GameManager.Instance.isAdvancedLevelSelected = isAdvancedLevel;
    }
    public void OnBasicLevelToggle(bool isOn)
    {

        if (isOn)
        {
            SetLevelOptions(true, false);

        }
    }

    public void OnAdvancedLevelToggle(bool isOn)
    {

        if (isOn)
        {
            SetLevelOptions(false, true);
        }
    }
}
