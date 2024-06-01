using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                Player player = other.GetComponent<Player>();

                if (!player.starpower)              
                {
                new WaitForSeconds(0.3f);
                player.Hit();
                new WaitForSeconds(3f);
                }
            }

    }
}