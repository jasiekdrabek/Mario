using System.Collections;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        Player p = player.gameObject.GetComponent<Player>();
        Vector3 delta;
        if (p.big)
        {
            delta = new Vector3(0f, 0f, 0f);
        }
        else
        {
            delta = new Vector3(0f, 0.5f, 0f);
        }
        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position + Vector3.right  + Vector3.up);
        yield return MoveTo(player, castle.position + delta);

        player.gameObject.SetActive(false);

        GameManager.Instance.world = nextWorld;
        GameManager.Instance.stage = nextStage;
        GameManager.Instance.NextLevel();
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }

}