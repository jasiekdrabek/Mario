using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    public PlayerSpriteRenderer activeRenderer;
    public GameObject Big;
    public GameObject Small;
    public GameObject pointsPopupPrefab;
    public Canvas mainCanvas;

    public CapsuleCollider2D capsuleCollider { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }

    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }
    public bool isHit { get; set; }

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathAnimation = GetComponent<DeathAnimation>();
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        Small.SetActive(true);
        Big.SetActive(false);
        activeRenderer = smallRenderer;
    }

    public void Hit()
    {
        if (!dead && !starpower)
        {
            if (big)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;
        GameManager.Instance.isBig = false;
        GameManager.Instance.ResetLevel();
    }

    public void Grow(bool showPoints = true)
    {
        GameManager.Instance.score += 10;
        GameManager.Instance.UpdateUI();
        if (showPoints)
        {
            ShowPoints(10);
        }
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;
        Small.SetActive(false);
        Big.SetActive(true);
        GameManager.Instance.isBig = true;
        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);
        StartCoroutine(ScaleAnimation());
    }

    public void Shrink()
    {
        isHit = true;
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;
        Small.SetActive(true);
        Big.SetActive(false);
        GameManager.Instance.isBig = false;
        capsuleCollider.size = new Vector2(1f, 1.5f);
        capsuleCollider.offset = new Vector2(0f, -0.2f);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
        isHit = false;
    }

    public void Starpower()
    {
        GameManager.Instance.score += 10;
        GameManager.Instance.UpdateUI();
        ShowPoints(10);
        StartCoroutine(StarpowerAnimation());
    }

    private IEnumerator StarpowerAnimation()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

    public void AddLife()
    {
        ShowPoints(5);
        GameManager.Instance.AddLife();
    }

    public bool isBig()
    {
        if (activeRenderer == bigRenderer)
        {
            return true;
        }
        return false;
    }

    public void ShowPoints(int points, bool isMonsterPopUp = false)
    {
        GameObject popup = Instantiate(pointsPopupPrefab, mainCanvas.transform);
        PointsPopup pointsPopup = popup.AddComponent<PointsPopup>();
        pointsPopup.Initialize(Camera.main, transform.position, isMonsterPopUp);
        pointsPopup.SetPoints(points);

        if (isMonsterPopUp) GameManager.Instance.IncreaseActivePopups();
    }

}