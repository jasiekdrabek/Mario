using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] highScoreTexts;
    private HighScoreManager highScoreManager;

    private void Start()
    {
        highScoreManager = FindObjectOfType<HighScoreManager>();
        if (highScoreManager == null)
        {
            Debug.LogError("HighScoreDisplay: HighScoreManager not found!");
        }

        StartCoroutine(DisplayScoresCoroutine());
    }

    IEnumerator DisplayScoresCoroutine()
    {
        yield return null;
        DisplayHighScores();
    }

    public void DisplayHighScores()
    {
        List<HighScoreEntry> highScores = highScoreManager.LoadHighScores();
        Debug.Log("HighScoreDisplay: Displaying " + highScores.Count + " high scores.");

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

