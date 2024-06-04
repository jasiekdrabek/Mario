using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    Vector3 targetPos;
    new Rigidbody2D rigidbody;
    Vector3 moveDirection;
    public float waitDuration;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
