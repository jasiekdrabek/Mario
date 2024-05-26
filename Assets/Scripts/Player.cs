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

    private void ShowPoints(int points)
    {
        if (pointsPopupPrefab == null)
        {
            Debug.LogError("PointsPopupPrefab is not assigned!");
            return;
        }

        if (mainCanvas == null)
        {
            Debug.LogError("MainCanvas is not assigned!");
            return;
        }

        GameObject popup = Instantiate(pointsPopupPrefab, mainCanvas.transform);

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        RectTransform popupRectTransform = popup.GetComponent<RectTransform>();
        popupRectTransform.position = screenPosition;

        TextMeshProUGUI textMesh = popup.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = points.ToString();
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in PointsPopupPrefab!");
        }

        popup.AddComponent<PointsPopup>().Initialize(Camera.main, transform.position, false);
    }

}