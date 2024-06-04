using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameOverController : MonoBehaviour
{
    public TMP_InputField saveScoreInputField;
    public void Start()
    {
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Score:" + GameManager.Instance.score.ToString();
        GameObject.Find("Coins").GetComponent<TextMeshProUGUI>().text = "Coins:" + (GameManager.Instance.coins + GameManager.Instance.coinsConvertedToLives).ToString();
        GameObject.Find("Time").GetComponent<TextMeshProUGUI>().text = "Time:" + GameManager.Instance.FormatTime(GameManager.Instance.gameTime);

    }
    public void RestartGame()
    {
        GameManager.Instance.NewGame();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.StartMenu();
    }
    public void SaveScore()
    {
        string name = saveScoreInputField.text;
        GameManager.Instance.SaveScore(name);
        GameObject.Find("SaveScorePanel").SetActive(false);
    }
}
