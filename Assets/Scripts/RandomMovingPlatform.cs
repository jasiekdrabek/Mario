using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    private float minSpeed = 2.5f;
    private float maxSpeed = 3.7f;
    Vector3 targetPos;
    PlayerMovement playerMovement;
    new Rigidbody2D rigidbody;
    Vector3 moveDirection;
    Rigidbody2D playerRigidbody2D;
    public float waitDuration;
    private float minWaitTime = 0.3f;
    private float maxWaitTime = 1.2f;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        speed = Random.Range(minSpeed, maxSpeed);
        waitDuration = Random.Range(minWaitTime, maxWaitTime);
    }
    private void Start()
    {
        targetPos = posB.position;
        DirectionCalculate();
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            moveDirection = Vector3.zero;
            targetPos = posB.position;
            StartCoroutine(WaitNext());
        }

        if (Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            moveDirection = Vector3.zero;
            targetPos = posA.position;
            StartCoroutine(WaitNext());
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = moveDirection * speed;
    }

    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.isOnPlatform = true;
            playerMovement.rigidbodyPlatform = rigidbody;
            playerRigidbody2D.gravityScale = playerRigidbody2D.gravityScale * 50;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMovement.isOnPlatform = false;
            playerRigidbody2D.gravityScale = playerRigidbody2D.gravityScale / 50;
        }
    }

    private IEnumerator WaitNext()
    {
        yield return new WaitForSeconds(waitDuration);
        DirectionCalculate();
    }
}
