using UnityEngine;

public class VisibilityTrigger2D : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Pobierz komponent SpriteRenderer (dla obiektów 2D)
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ustaw obiekt jako niewidoczny na pocz¹tku
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ustaw obiekt jako widoczny
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ustaw obiekt jako widoczny
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
        }
    }
}

