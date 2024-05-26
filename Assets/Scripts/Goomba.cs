using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;
    public GameObject pointsPopupPrefab;
    public Canvas mainCanvas;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower)
            {
                Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Vector2 speed = other.attachedRigidbody.velocity;
            if (speed.x > 0.1f)
            {
                Hit();
            }
        }
    }

    private void Flatten()
    {
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        ShowPoints(points);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        ShowPoints(points);
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

        popup.AddComponent<PointsPopup>().Initialize(Camera.main, transform.position);
        GameManager.Instance.IncreaseActivePopups();
    }

}