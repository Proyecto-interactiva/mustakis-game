using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Configuration
    public float speed = 0.5f;

    // Movement vectors
    Vector3 horizontal;
    Vector3 vertical;
    Vector3 currentDirection;
    Vector3 prevDirection;

    // Physics movement
    public Rigidbody2D rb;

    public Joystick joystick;
    

    // Animation components
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        prevDirection = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        currentDirection = new Vector3();
        horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
        vertical = new Vector3(0.0f, Input.GetAxisRaw("Vertical"), 0.0f);

     if (horizontal == Vector3.zero) horizontal = new Vector3(joystick.Horizontal, 0.0f, 0.0f);
     if (vertical == Vector3.zero) vertical = new Vector3(0.0f, joystick.Vertical, 0.0f);

        // Direction logic
        if (horizontal != Vector3.zero && vertical == Vector3.zero)
        {
            currentDirection = horizontal;
            prevDirection = horizontal;
        }
        else if (horizontal == Vector3.zero && vertical != Vector3.zero)
        {
            currentDirection = vertical;
            prevDirection = vertical;
        }
        else
        {
            if (prevDirection == vertical)
            {
                currentDirection = horizontal;
            }
            else if (prevDirection == horizontal)
            {
                currentDirection = vertical;
            }
        }

        // Movement
        rb.velocity = new Vector2(currentDirection.x, currentDirection.y) * speed;

        // Animation logic
        animator.SetInteger("Horizontal", (int) currentDirection.x);
        animator.SetInteger("Vertical", (int) currentDirection.y);
        // Gabo - Direccion "Idle"
        if (currentDirection.x < 0)
        {
            animator.SetInteger("Facing", 1);
        }
        else if (currentDirection.x > 0)
        {
            animator.SetInteger("Facing", 2);
        }
        else if (currentDirection.y > 0)
        {
            animator.SetInteger("Facing", 3);
        }
        else if (currentDirection.y < 0)
        {
            animator.SetInteger("Facing", 4);
        }

    }
}
