using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 input;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // no gravity for 2D top-down
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        input = new Vector2(x, y).normalized;

        // animator control
        GetComponent<Animator>().SetBool("isMoving", input.sqrMagnitude > 0);

        Flip(x);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }

    // --- FLIP FUNCTION ---
    void Flip(float inputX)
    {
        if (inputX > 0 && !facingRight)
            DoFlip();
        else if (inputX < 0 && facingRight)
            DoFlip();
    }

    void DoFlip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}