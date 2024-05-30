using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;
    public float minJumpInterval = 1f;
    public float maxJumpInterval = 3f;
    public float moveRange = 4f;  // Zakres ruchu na osi y

    private new Rigidbody2D rigidbody;
    public Vector2 velocity;
    public float deltaDistance = 0;
    public float deltaRadius = 0;
    private Vector3 initialPosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
        StartCoroutine(RandomMove());
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if (rigidbody.Raycast(direction, deltaRadius, deltaDistance))
        {
            direction = -direction;
        }

        if (direction.x > 0f)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (direction.x < 0f)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        if (direction.y < 0f)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (direction.y > 0f)
        {
            transform.localEulerAngles = Vector3.zero;
        }
    }

    private IEnumerator RandomMove()
    {
        while (true)
        {
            float waitTime = Random.Range(minJumpInterval, maxJumpInterval);
            yield return new WaitForSeconds(waitTime);
            float deltaY =  transform.position.y - initialPosition.y;
            Vector3 newPosition = Vector3.zero;
            if (deltaY <= 4f && deltaY >= -2f) newPosition =   new Vector3(0, GetRandomGaussian() * moveRange, 0);
            if (deltaY > 4f) newPosition =  new Vector3(0, -1,0);
            if (deltaY < -2f) newPosition =  new Vector3(0, 1, 0);
            StartCoroutine(MoveTo(newPosition));
        }
    }

    private IEnumerator MoveTo(Vector3 position)
    {
        velocity.y = position.y;
        yield return new WaitForSeconds(1f);
        velocity.y = 0;
    }

    private float GetRandomGaussian()
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return Mathf.Clamp(randStdNormal / 3, -1f, 1f); // dzielimy przez 3 ¿eby randStdNormal w przybli¿eniu mia³ wartoœæ w zakresie [-1, 1]
    }
}
