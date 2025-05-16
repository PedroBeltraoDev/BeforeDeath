using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthMultiplayer : MonoBehaviour
{
    public int playerId = 1; // 1 ou 2
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player " + playerId + " morreu!");
        Destroy(gameObject);
    }
}
