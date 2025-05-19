using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthBarFill;
    public GameObject deathEffect;

    public int enemiesKilled = 0;

    private float criticalChance = 0.10f;
    private float baseDamage = 10f;
    private float criticalDamage = 18f;
    private float lifestealAmount = 0.7f;
    private float lifestealChance = 0.75f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 2f, LayerMask.GetMask("Enemy"));
        if (hit.collider != null)
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                float chance = Random.value;
                int damageDealt;

                if (chance < criticalChance)
                {
                    damageDealt = Mathf.RoundToInt(criticalDamage);
                }
                else
                {
                    damageDealt = Mathf.RoundToInt(baseDamage);
                }

                enemy.TakeDamage(damageDealt);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;

        // Aumenta chance de crítico
        if (enemiesKilled % 15 == 0)
            criticalChance += 0.01f;

        // Aumenta dano base
        if (enemiesKilled % 5 == 0)
            baseDamage += 0.2f;

        // Vampirismo
        if (Random.value < lifestealChance)
        {
            currentHealth += Mathf.RoundToInt(lifestealAmount);
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }

        // Aumenta vida máxima a cada 20 inimigos
        if (enemiesKilled % 20 == 0)
        {
            maxHealth += 5;
            currentHealth += 5;
        }

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
