using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;
    private float minJumpInterval = 2f;
    private float maxJumpInterval = 4f;
    private float moveRange = 4f;  // Zakres ruchu na osi y

    private new Rigidbody2D rigidbody;
    public Vector2 velocity;
    public float deltaDistance = 0;
    public float deltaRadius = 0;
    private Vector3 initialPosition;
    private bool isChangeHeight = false;
    private float changeTime = 0.5f;
    private float changeHeight;

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
        if(isChangeHeight)
        {
            velocity.y = changeHeight;
            changeTime -= Time.fixedDeltaTime;
            if(changeTime <= 0)
            {
                changeTime = 0.8f;
                isChangeHeight = false;
                velocity.y = 0;
            }
        }
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
            if (deltaY <= 2f && deltaY >= -2f) changeHeight =   1+ GetRandomGaussian() * moveRange;
            if (deltaY > 2f) changeHeight =  -2;
            if (deltaY < -2f) changeHeight =  2;
            isChangeHeight = true;
        }
    }

    private float GetRandomGaussian()
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return Mathf.Clamp(randStdNormal / 3, -1f, 1f); // dzielimy przez 3 ¿eby randStdNormal w przybli¿eniu mia³ wartoœæ w zakresie [-1, 1]
    }
}   
