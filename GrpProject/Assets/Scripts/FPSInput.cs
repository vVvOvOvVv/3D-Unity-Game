using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

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
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private TextMeshProUGUI currentHPTxt, maxHPTxt;

    // shield variables
    [SerializeField] public int shield = 0, maxShield = 100;
    [SerializeField] public Slider playerShieldBar;
    [SerializeField] private TextMeshProUGUI shieldTxt;

    private Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        // get the character controller component
        charController = GetComponent<CharacterController>();

        // Initialize health
        currentHealth = maxHealth; 
        UpdateHPNumbers();
        // shield
        playerShieldBar.maxValue = maxShield;
        playerShieldBar.enabled = false;
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
        // if player has shield/armor, consume that first
        if (shield < 0)
        {
            if (shield < dmg)
            {
                currentHealth -= dmg - shield;
                shield = 0;
            } else
            {
                shield -= dmg;
            }
        }
        currentHealth -= dmg;
        UpdatePlayerHPBar();
        UpdateHPNumbers();

        if (currentHealth <= 0)
        {
            // death animation or game over screen
            Debug.Log("HP depleted, resetting health");
            currentHealth = maxHealth; // DELETE LATER - for debugging only
        }
    }

    private void UpdatePlayerHPBar()
    {
        playerHPBar.value = currentHealth / maxHealth;
    }

    private void UpdateHPNumbers()
    {
        currentHPTxt.SetText(currentHealth.ToString());
        maxHPTxt.SetText(maxHealth.ToString());
    }

    private void UpdateArmorBar()
    {
        shieldTxt.SetText(shield.ToString());
        playerShieldBar.value = shield;

        if (shield <= 0)
            playerShieldBar.enabled = false;
        else
            playerShieldBar.enabled = true;
    }
}
