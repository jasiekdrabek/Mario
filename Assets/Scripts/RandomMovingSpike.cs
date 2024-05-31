using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovingSpike : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    private float minSpeed = 0.7f;
    private float maxSpeed = 1.7f;
    Vector3 targetPos;
    new Rigidbody2D rigidbody;
    Vector3 moveDirection;
    public float waitDuration;
    private float minWaitTime = 1f;
    private float maxWaitTime = 2f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
        if (Vector2.Distance(transform.position, posA.position) < 0.03f)
        {
            moveDirection = Vector3.zero;
            targetPos = posB.position;
            StartCoroutine(WaitNext());
        }

        if (Vector2.Distance(transform.position, posB.position) < 0.03f)
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

    private IEnumerator WaitNext()
    {
        yield return new WaitForSeconds(waitDuration);
        DirectionCalculate();
    }
}
