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

    public void OnBasicLevelToggle(bool isOn)
    {
        GameManager.Instance.difficultyLevel = "";
    }

    public void OnAdvancedLevelToggle(bool isOn)
    {
        GameManager.Instance.difficultyLevel = "-hidden";
    }

    public void OnTutorialToggle(bool isOn)
    {        
            GameManager.Instance.tutorial = !GameManager.Instance.tutorial;
    }
}
