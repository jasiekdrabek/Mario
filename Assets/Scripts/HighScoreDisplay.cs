using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] highScoreTexts;
    private HighScoreManager highScoreManager;

    private void Start()
    {
        highScoreManager = FindObjectOfType<HighScoreManager>();
    }

    public void DisplayHighScores()
    {
        List<HighScoreEntry> highScores = highScoreManager.LoadHighScores();
        for (int i = 0; i < highScoreTexts.Length; i++)
        {
            if (i < highScores.Count)
            {
                highScoreTexts[i].text = $"Score: {highScores[i].score} Time: {highScores[i].time} Coins: {highScores[i].coins}";
            }
            else
            {
                highScoreTexts[i].text = "";
            }
        }
    }
}
