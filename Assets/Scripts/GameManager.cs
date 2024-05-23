
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
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

    private void Awake()
    {
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
            player.Grow();
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
        LoadLevel(1, 1);
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
        // Wywo³anie w³aœciwej metody z parametrami
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
        TextMeshProUGUI ct; 
        TextMeshProUGUI lt;
        TextMeshProUGUI st;
        GameObject.Find("Coins").TryGetComponent<TextMeshProUGUI>(out ct);
        if (ct != null)
        {
            ct.text = "Coins: " + coins.ToString();
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

}