using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HighScoreDisplay : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private HighScoreManager highScoreManager;

    private void Awake()
    {
        highScoreManager = FindObjectOfType<HighScoreManager>();
        if (highScoreManager == null)
        {
            Debug.LogError("HighScoreDisplay: HighScoreManager not found!");
        }
        entryContainer = transform.Find("HighScoreContainer");
        entryTemplate = entryContainer.Find("HighScoreTemplate");
        entryTemplate.gameObject.SetActive(false);
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
        float height = 40f;
        for (int i = 0; i < highScores.Count; i++)
        {
            Transform highScore = Instantiate(entryTemplate,entryContainer);
            RectTransform highScoreRect = highScore.GetComponent<RectTransform>();
            highScoreRect.anchoredPosition = new Vector2(0, -height * (i+1));
            highScore.gameObject.SetActive(true);
            highScore.Find("Score").GetComponent<TextMeshProUGUI>().text = highScores[i].score.ToString();
            highScore.Find("Time").GetComponent<TextMeshProUGUI>().text = highScores[i].time.ToString();
            highScore.Find("Coins").GetComponent<TextMeshProUGUI>().text = highScores[i].coins.ToString();
            highScore.Find("Background").gameObject.SetActive(i %2 == 1);
        }
    }
}

