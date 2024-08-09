using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller

public class FPSInput : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -20.0f;
    private float vertSpeed;
    private CharacterController charController;

    // Dash variables
    public float dashSpeed = 18.0f;
    public float dashDuration = 0.3f;
    private bool isDashing = false;
    //private float dashTime = 0f;

    // Health variables
    public int maxHealth = 100;
    public int currentHealth;
    public float healthRecoveryAmount = 1.0f;

    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        // get the character controller component
        charController = GetComponent<CharacterController>();

        // Initialize health
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // changes based on WASD keys
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        movementDirection = transform.right * deltaX + transform.forward * deltaZ;

        // make diagonal movement consistent
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f) * speed;


        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            vertSpeed = jumpSpeed;
        }
        else if (!charController.isGrounded)
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }

        movementDirection.y = vertSpeed;
        movementDirection *= Time.deltaTime;
        //charController.Move(movement);

        // Check for dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        // Apply movement
        if (!isDashing)
        {
            charController.Move(movementDirection);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        // Calculate dash direction
        Vector3 dashDirection = movementDirection.normalized * dashSpeed;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            charController.Move(dashDirection * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            // death animation or game over screen
            Debug.Log("HP depleted, resetting health");
            currentHealth = maxHealth; // DELETE LATER - for debugging only
        }
    }
}
