using System.Collections;
using TMPro;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    private new Rigidbody2D rigidbody;
    public float shellSpeed = 12f;

    private bool shelled;
    private bool pushed;
    private bool hited;

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
                if (player.isHit) return;
                player.Hit();
            }
        }
        if (!shelled && collision.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
        if (shelled && collision.gameObject.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - collision.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                Player player = collision.gameObject.GetComponent<Player>();

                if (player.starpower)
                {
                    Destroy(gameObject);
                }
                else
                {
                    player.Hit();
                }
            }
        }
    }

    private void EnterShell()
    {
        shelled = true;
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        Player player = GameObject.Find("Mario").GetComponent<Player>();
        player.ShowPoints(points,true); // Wyœwietlanie punktów
        GetComponent<SpriteRenderer>().sprite = shellSprite;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        StartCoroutine(DestroyAfterTime(5f));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
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
        if (hited) return;
        hited = true;
        int points = GameManager.Instance.GetPoints();
        GameManager.Instance.score += points;
        GameManager.Instance.UpdateUI();
        Player player = GameObject.Find("Mario").GetComponent<Player>();
        player.ShowPoints(points,true); // Wyœwietlanie punktów
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject,1f);
    }
}