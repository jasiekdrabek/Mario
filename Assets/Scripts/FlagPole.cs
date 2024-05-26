using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;
    public GameObject pointsPopupPrefab; // Prefab dla popupu punktów
    public Canvas mainCanvas; // Canvas dla popupu punktów
    private float startTime;
    private GameObject popup;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            startTime = Time.time;
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        Player p = player.gameObject.GetComponent<Player>();
        Vector3 delta = p.big ? Vector3.zero : new Vector3(0f, 0.5f, 0f);
        popup = Instantiate(pointsPopupPrefab, mainCanvas.transform);
        popup.AddComponent<PointsPopup>().Initialize(Camera.main, transform.position + Vector3.up *9, false);

        //float startTime = Time.time;
        yield return MoveTo(player, poleBottom.position + delta);
        yield return MoveTo(player, player.position + Vector3.right + delta);
        yield return MoveTo(player, castle.position + delta);
        float endTime = Time.time;
        player.gameObject.SetActive(false);
        float duration = endTime - startTime;
        int points = Mathf.CeilToInt(duration * 100); // Mo¿esz dostosowaæ tê formu³ê

        if (popup != null)
        {
            popup.GetComponent<PointsPopup>().SetPoints(points);
        }
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        yield return new WaitForSeconds(2f);
        GameManager.Instance.world = nextWorld;
        GameManager.Instance.stage = nextStage;
        GameManager.Instance.NextLevel();
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            float duration = Time.time - startTime;
            int points = Mathf.CeilToInt(duration * 100);
            if (popup != null)
            {
                popup.GetComponent<PointsPopup>().SetPoints(points);
            }
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }
}