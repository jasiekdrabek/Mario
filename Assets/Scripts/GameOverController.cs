using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverController : MonoBehaviour
{
    public void RestartGame()
    {
        GameManager.Instance.NewGame();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.StartMenu();
    }
}
