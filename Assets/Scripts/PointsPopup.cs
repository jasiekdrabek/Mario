using UnityEngine;
using System.Collections;
using TMPro;

public class PointsPopup : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 worldPosition;
    private float duration = 3f; // Czas trwania popupu w sekundach
    private bool monsterPopup = true;
    private TextMeshProUGUI pointsText;
    private RectTransform popupRectTransform;

    public void Initialize(Camera camera, Vector3 worldPos, bool isMonsterPopup = true)
    {
        mainCamera = camera;
        worldPosition = worldPos + new Vector3(1,1,0);
        monsterPopup = isMonsterPopup;
        popupRectTransform = GetComponent<RectTransform>();
    }
    public void SetPoints(int points)
    {
        pointsText = GetComponentInChildren<TextMeshProUGUI>();
        if (pointsText != null)
        {
            pointsText.text = points.ToString();
        }
    }

    void Start()
    {
            StartCoroutine(DestroyAfterTime(duration));
    }

    void Update()
    {
        if (mainCamera != null)
        {
            Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                popupRectTransform.parent as RectTransform, screenPosition, mainCamera, out canvasPosition);
            popupRectTransform.localPosition = canvasPosition;
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (monsterPopup)
        {
            GameManager.Instance.DecreaseActivePopups();
        }
        Destroy(gameObject);
    }

    public void UpdateWorldPosition(Vector3 newWorldPosition)
    {
        worldPosition = newWorldPosition;
    }
}