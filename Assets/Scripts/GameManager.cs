
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int score { get; set; }
    public bool isLevelLoading { get; set; }
    public static GameManager Instance { get; private set; }
    public int world { get;  set; }
    public int stage { get;  set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public bool isBig { get; set; }
    public bool tutorial  { get; set; }

    private int activePopups = 0; // Liczba aktywnych popup�w
    private const int basePoints = 10; // Podstawowa liczba punkt�w za przeciwnika

    private void Awake()
    {
        tutorial = true;
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
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
    }

    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isLevelLoading)
        {
            UpdateUI();
        }
        if (isBig)
        {
            Player player = GameObject.Find("Mario").GetComponent<Player>();
            player.Grow(false);
            score -= 10;
            UpdateUI();
        }
    }

    public void StartMenu()
    {

        SceneManager.LoadScene("main menu");

    }

    public void NewGame()
    {        
        lives = 3;
        coins = 0;
        score = 0;
        activePopups = 0;
        if (tutorial)
        {
            LoadLevel(1, 0);
        }
        else
        {
            LoadLevel(1, 1);
        }
    }

    public void GameOver()
    {
        isLevelLoading = false;
        SceneManager.LoadScene("Game_over");
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;
        isLevelLoading=true;
        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        StartCoroutine(ShowNextLevelPanel());
        Invoke(nameof(InvokeLoadLevel), 3.3f);
    }

    private void InvokeLoadLevel()
    {
        // Wywo�anie w�a�ciwej metody z parametrami
        LoadLevel(world, stage);
    }
    private IEnumerator ShowNextLevelPanel()
    {
        GameObject nls = Extensions.FindInactiveObjectByName("NextLevelScreen");
        if (nls != null)
        {
            nls.SetActive(true);
            yield return new WaitForSeconds(3f);
            nls.SetActive(false);            
        }
    }

    private IEnumerator ShowLifeLostPanel()
    {
        GameObject lls = Extensions.FindInactiveObjectByName("LifeLostScreen");
        if (lls != null)
        {
            lls.SetActive(true);
            yield return new WaitForSeconds(3f);
            lls.SetActive(false);
        }
    }


    public void ResetLevel()
    {
        UpdateUI();
        lives--;

        if (lives > 0)
        {
            StartCoroutine(ShowLifeLostPanel());
            Invoke(nameof(LoadCurrentLevel),3.3f);
        }
        else
        {
            Invoke(nameof(GameOver), 3.3f); 
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
            score += 45;
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
    public void UpdateUI()
    {
        TextMeshProUGUI ct; 
        TextMeshProUGUI lt;
        TextMeshProUGUI st;
        GameObject.Find("Coins").TryGetComponent<TextMeshProUGUI>(out ct);
        if (ct != null)
        {
            ct.text = "x " + coins.ToString();
        }
        GameObject.Find("Lives").TryGetComponent<TextMeshProUGUI>(out lt);
        if (lt != null)
        {
            lt.text = "Lives: " + lives.ToString();
        }
        GameObject.Find("Score").TryGetComponent<TextMeshProUGUI>(out st);
        if (st != null)
        {
            st.text = "Score: " + score.ToString();
        }
    }
    public void IncreaseActivePopups()
    {
        activePopups++;
    }

    public void DecreaseActivePopups()
    {
        activePopups--;
    }

    public int GetPoints()
    {
        return basePoints * (activePopups + 1);
    }
}