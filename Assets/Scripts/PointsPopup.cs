using UnityEngine;
using System.Collections;

public class PointsPopup : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 worldPosition;
    private float duration = 3f; // Czas trwania popupu w sekundach
    private bool monsterPopup = true;

    public void Initialize(Camera camera, Vector3 worldPos, Canvas canvas, bool isMonsterPopup = true)
    {
        mainCamera = camera;
        worldPosition = worldPos;
        monsterPopup = isMonsterPopup;
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
            RectTransform popupRectTransform = GetComponent<RectTransform>();
            popupRectTransform.position = screenPosition;
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
}