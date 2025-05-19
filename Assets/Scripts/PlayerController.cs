using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    public int currentHealth;
    public Image healthBarFill;

    [Header("Dano")]
    public int baseDamage = 10;
    public int criticalDamage = 18;
    public float criticalChance = 0.1f; // 10%
    public float criticalChanceIncrease = 0.01f; // +1% a cada 15 kills
    public float damageIncreasePer5Kills = 0.2f;

    [Header("Vampirismo")]
    public float vampirismChance = 0.75f; // 75%
    public float vampirismAmount = 0.7f;

    [Header("Contador")]
    public int enemiesKilled = 0;

    private Camera mainCamera;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique esquerdo
        {
            AttackEnemy();
        }
    }

    void AttackEnemy()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            bool isCritical = Random.value < criticalChance;
            int damage = isCritical ? criticalDamage : baseDamage;

            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
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
        Destroy(gameObject);
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;

        // Aumenta chance de crítico
        if (enemiesKilled % 15 == 0)
        {
            criticalChance += criticalChanceIncrease;
        }

        // Aumenta dano base
        if (enemiesKilled % 5 == 0)
        {
            baseDamage += Mathf.RoundToInt(damageIncreasePer5Kills);
        }

        // Vampirismo
        if (Random.value < vampirismChance)
        {
            Heal(Mathf.RoundToInt(vampirismAmount));
        }

        // Aumenta vida máxima a cada 20 kills
        if (enemiesKilled % 20 == 0)
        {
            maxHealth += 5;
            currentHealth += 5;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            UpdateHealthUI();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
