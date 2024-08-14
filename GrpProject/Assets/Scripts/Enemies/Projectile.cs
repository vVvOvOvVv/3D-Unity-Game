using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 2;
    public float knockbackForce = 0.01f;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, 3f); // Destroy the projectile after 5 seconds to prevent infinite travel
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player
            FPSInput player = other.GetComponent<FPSInput>();
            if (player != null)
            {
                player.TakeDamage(damage);

                // Apply a controlled knockback
                Rigidbody playerRb = other.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    // Apply knockback along the horizontal plane
                    Vector3 knockbackDirection = new Vector3(direction.x, 0, direction.z).normalized;
                    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.VelocityChange);

                }
            }

            // Destroy the projectile on impact with the player
            Destroy(gameObject);
        }
        else
        {
            // Optionally, destroy the projectile if it hits something else
            Destroy(gameObject);
        }
    }
}
