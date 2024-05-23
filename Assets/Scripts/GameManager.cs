
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI livesText;
    public GameObject LifeLostPanel;
    public GameObject NextLevelPanel;
    private int score = 0;


    public static GameManager Instance { get; private set; }
    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
//            LifeLostPanel = GameObject.Find("LifeLostPanel");
//            LifeLostPanel.SetActive(false);
            DontDestroyOnLoad(gameObject);

        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        score = 0;
        StartMenu();
        //UpdateUI();
    }
    public void StartMenu()
    {

        SceneManager.LoadScene("main menu");

    }

    public void NewGame()
    {        
        lives = 3;
        coins = 0;
//        if (coinsText == null)
//        {
//            coinsText = GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>();
//        }
        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game_over");
        //NewGame();
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        StartCoroutine(ShowNextLevelPanel());
        LoadLevel(world, stage + 1);
    }
    private IEnumerator ShowNextLevelPanel()
    {
        if (NextLevelPanel != null)
        {
            NextLevelPanel.SetActive(true);
            yield return new WaitForSeconds(5f);
            if (NextLevelPanel != null)
            {
                NextLevelPanel.SetActive(false);
            }
        }
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }


    private IEnumerator ShowLifeLostPanel()
    {
        if (LifeLostPanel != null)
        {
            LifeLostPanel.SetActive(true);
            yield return new WaitForSeconds(5f);
            if (LifeLostPanel != null)
            {
                LifeLostPanel.SetActive(false);
            }
        }
    }


    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            StartCoroutine(ShowLifeLostPanel());
            Invoke(nameof(LoadCurrentLevel), 5f);
        }
        else
        {
            GameOver();
        }
    }

    private void LoadCurrentLevel()
    {
        LoadLevel(world, stage);
    }
    public void AddCoin()
    {
        coins++;
        score += 10;
        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
        UpdateUI();
    }

    public void AddLife()
    {
        lives++;
        score += 5;
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (coinsText == null)
        {
            coinsText = GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>();
        }
        if (livesText == null)
        {
            livesText = GameObject.Find("LivesText").GetComponent<TextMeshProUGUI >();
        }

        coinsText.text = "Coins: " + coins.ToString();
        livesText.text = "Lives: " + lives.ToString();
    }

}