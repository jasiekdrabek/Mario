using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.isBig = false;
            GameManager.Instance.ResetLevel();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }

}