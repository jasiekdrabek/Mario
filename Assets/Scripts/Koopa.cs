using TMPro;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    private new Rigidbody2D rigidbody;
    public float shellSpeed = 12f;
    public GameObject pointsPopupPrefab; // Prefab do wyœwietlania punktów
    public Canvas mainCanvas;

    private bool shelled;
    private bool pushed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower)
            {
                Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shelled && other.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                Player player = other.GetComponent<Player>();

                if (player.starpower)
                {
                    Hit();
                }
                else
                {
                    player.Hit();
                }
            }
        }
        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Vector2 speed = other.attachedRigidbody.velocity;
            if (speed.x > 0.1f)
            {
                Hit();
            }
        }
    }

    private void EnterShell()
    {
        shelled = true;
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        ShowPoints(points); // Wyœwietlanie punktów
        GetComponent<SpriteRenderer>().sprite = shellSprite;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;

        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        ShowPoints(points); // Wyœwietlanie punktów
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
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

        popup.AddComponent<PointsPopup>().Initialize(Camera.main, transform.position, mainCanvas);
        GameManager.Instance.IncreaseActivePopups();
    }
}