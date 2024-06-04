using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int score { get; set; }
    public bool isLevelLoading { get; set; }
    public static GameManager Instance { get; private set; }
    public int world { get;  set; }
    public int stage { get;  set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public int coinsConvertedToLives { get;set; }
    public bool isBig { get; set; }
    private int activePopups = 0; // Liczba aktywnych popupÑƒw
    private const int basePoints = 10; // Podstawowa liczba punktÑƒw za przeciwnika
    
    private const string filePath = "scores.txt";
    public float gameTime;
    public string difficultyLevel = "";
    public bool tutorial  { get; set; }


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
        gameTime = 0f;
        coinsConvertedToLives = 0;
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
        activePopups = 0;
        if (stage == 0)
        {
            SceneManager.LoadScene($"{world}-{stage}");
            return;
        } 
        SceneManager.LoadScene($"{world}-{stage}{difficultyLevel}");
        UpdateUI();
    }

    public void NextLevel()
    {
        StartCoroutine(ShowNextLevelPanel());
        Invoke(nameof(InvokeLoadLevel), 3.3f);
    }

    private void InvokeLoadLevel()
    {
        // WywoÑ–anie wÑ–aÑšciwej metody z parametrami
        LoadLevel(world, stage);
    }
    private IEnumerator ShowNextLevelPanel()
    {
        GameObject nls = Extensions.FindInactiveObjectByName("NextLevelScreen(Clone)");
        if (nls != null)
        {
            RectTransform rectTransform = nls.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
            nls.SetActive(true);
            yield return new WaitForSeconds(3f);
            nls.SetActive(false);            
        }
    }

    private IEnumerator ShowLifeLostPanel()
    {
        GameObject lls = Extensions.FindInactiveObjectByName("LifeLostScreen(Clone)");
        if (lls != null)
        {
            RectTransform rectTransform = lls.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
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
            coinsConvertedToLives++;
            AddLife();
        }
        UpdateUI();
    }

    public void AddLife()
    {
        if(lives <=9)lives++;
        score += 5;
        UpdateUI();
    }
    public void UpdateUI()
    {
        TextMeshProUGUI ct; 
        TextMeshProUGUI lt;
        TextMeshProUGUI st;

        if (!GameObject.Find("Coins")) return;
        GameObject.Find("Coins").TryGetComponent<TextMeshProUGUI>(out ct);
        if (ct != null)
        {
            ct.text = "x " + coins.ToString();
        }
        if (!GameObject.Find("Lives")) return;
        GameObject.Find("Lives").TryGetComponent<TextMeshProUGUI>(out lt);
        if (lt != null)
        {
            lt.text = lives.ToString();
        }
        if (!GameObject.Find("Score")) return;
        GameObject.Find("Score").TryGetComponent<TextMeshProUGUI>(out st);
        if (st != null)
        {
            st.text = "Score: " + score.ToString();
        }
    }

    public void UpdateUITime()
    {
        TextMeshProUGUI tt;
        if (!GameObject.Find("Time")) return;
        GameObject.Find("Time").TryGetComponent<TextMeshProUGUI>(out tt);
        if (tt != null)
        {
            tt.text = "Time: " + FormatTime(gameTime);
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
    private void Update()
    {
        if (isLevelLoading)
        {
            gameTime += Time.deltaTime;
            UpdateUITime();
        }

    }
    public void SaveScore(string name)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{score},{FormatTime(gameTime)},{coins},{name}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("BÅ‚Ä…d przy zapisywaniu wyniku: " + e.Message);
        }
    }
    public string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }



}