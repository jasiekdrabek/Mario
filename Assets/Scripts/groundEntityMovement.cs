using System.Collections;
using UnityEngine;

public class groundEntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;
    public float jumpForce = 50f; // Si³a skoku
    public float minJumpInterval = 1f; // Minimalny czas miêdzy skokami
    public float maxJumpInterval = 3f; // Maksymalny czas miêdzy skokami

    private new Rigidbody2D rigidbody;
    public Vector2 velocity;
    public float deltaDistance = 0;
    public float deltaRadius = 0;
    private bool isGrounded = true;
    private bool jumping = false;
    private float jumpingTime = 1f;

    public float gravity => (-2f * 5f) / Mathf.Pow((1f / 2f), 2);
    public bool falling => velocity.y < 0f && !isGrounded;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
        StartCoroutine(RandomJump());
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

    private void Update()
    {
        isGrounded = rigidbody.Raycast(Vector2.down, 0f, 0.3f);
        if (jumping)
        {
            Jump();
            jumpingTime -= 0.03f;
            if (jumpingTime <= 0f)
            {
                jumping = false;
                jumpingTime = 1f;
            }
        }
        if (isGrounded)
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
        else
        {
            ApplyGravity();
        }
    }

    private void FixedUpdate()
    {
        if (rigidbody.Raycast(direction, deltaRadius, deltaDistance))
        {
            direction = -direction;
        }
        velocity.x = direction.x * speed;
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;
        rigidbody.MovePosition(position);

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

    private IEnumerator RandomJump()
    {
        while (true)
        {
            float waitTime = Random.Range(minJumpInterval, maxJumpInterval);
            yield return new WaitForSeconds(waitTime);
            if (isGrounded)
            {
                jumping = true;
            }
        }
    }

    private void Jump()
    {
        velocity.y = jumpForce;
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f;
        float multiplier = falling ? 2f : 1f;
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }
}
