using System.Collections;
using UnityEngine;

public class groundEntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;
    private float jumpForce = 20f; // Si³a skoku
    private float minJumpInterval = 2f; // Minimalny czas miêdzy skokami
    private float maxJumpInterval = 4f; // Maksymalny czas miêdzy skokami

    private new Rigidbody2D rigidbody;
    public Vector2 velocity;
    public float deltaDistance = 0;
    public float deltaRadius = 0;
    private bool isGrounded = true;
    private bool jumping = false;
    private float jumpingTime = 0.25f;

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


    private void FixedUpdate()
    {
        isGrounded = rigidbody.Raycast(Vector2.down, 0f, 0.3f);
        if (jumping)
        {
            jumpingTime -= Time.fixedDeltaTime;
            if (jumpingTime <= 0f)
            {
                jumping = false;
                jumpingTime = 0.25f;
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
            if (jumping) yield return null;
            float jumpChance = Random.value;
            if (isGrounded && jumpChance >= 0.90)
            {
                jumping = true;
                velocity.y = jumpForce;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !jumping;
        float multiplier = falling ? 2f : 1f;
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }
}
