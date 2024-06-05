using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

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
                if (player.isHit) return;
                player.Hit();
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void Flatten()
    {
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        Player player = GameObject.Find("Mario").GetComponent<Player>();
        player.ShowPoints(points, true); // Wyœwietlanie punktów
        GetComponent<Collider2D>().enabled = false;
        if(GetComponent<groundEntityMovement>() != null) GetComponent<groundEntityMovement>().enabled = false;
        if (GetComponent<flyingEntityMovement>() != null) GetComponent<flyingEntityMovement>().enabled = false;
        if (GetComponent<EntityMovement>() != null) GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        Player player = GameObject.Find("Mario").GetComponent<Player>();
        player.ShowPoints(points, true); // Wyœwietlanie punktów
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }
}