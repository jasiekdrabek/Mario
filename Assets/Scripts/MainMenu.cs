using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public void PlayGame() 
    {
          GameManager.Instance.NewGame();
//        SceneManager.LoadSceneAsync("1-1");
    }
    public void QuitGame()
    {
        // Zakończ grę
        Application.Quit();
        Debug.Log("Gra zakończona");
    }
}
