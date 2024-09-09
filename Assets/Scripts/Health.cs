using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int playerHealth = 10;
    public int damage = 1; // The damage amount this player can receive from any weapon
    private EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = FindObjectOfType<EnemyAI>();
    }

    // Method to reduce player health
    public void TakeDamage(int damageAmount)
    {
        
        playerHealth -= damageAmount;

        // Insert player animations for taking damage here
        Debug.Log("Player took damage. Current health: " + playerHealth);

        if (playerHealth <= 0)
        {
            // Handle player death
            Debug.Log("Player has died.");
            // Optionally reset health
            playerHealth = 10;

            // Reload the current scene or handle death logic
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "EnemyWeapon" tag
        if (other.gameObject.CompareTag("Attack Sword") && enemyAI.isAttacking)
        {
            // Apply damage to the player
            TakeDamage(damage);
        }
    }
}