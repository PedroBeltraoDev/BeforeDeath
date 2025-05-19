using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float damage = 15f;
    public LayerMask enemyLayer;  // Defina no Inspector a camada dos inimigos

    // Vampirismo
    public float vampirismRange = 5f;
    public float vampirismDamage = 4f;
    public float vampirismHeal = 15f;
    public float vampirismCooldown = 3.5f;
    private float lastVampirismTime = -Mathf.Infinity;

    private HealthSystem playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<HealthSystem>();
        if (playerHealth == null)
        {
            Debug.LogWarning("HealthSystem não encontrado no jogador.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Ataque normal com botão esquerdo
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))  // Vampirismo ao apertar E
        {
            TryVampirism();
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Inimigo {enemy.name} atingido com {damage} de dano.");
            }
        }
    }

    void TryVampirism()
    {
        if (Time.time >= lastVampirismTime + vampirismCooldown)
        {
            Vampirism();
            lastVampirismTime = Time.time;
        }
        else
        {
            float remaining = (lastVampirismTime + vampirismCooldown) - Time.time;
            Debug.Log($"Vampirismo em cooldown. Aguarde {remaining:F1} segundos.");
        }
    }

    void Vampirism()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, vampirismRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(vampirismDamage);
                Debug.Log($"Vampirismo atingiu inimigo {enemy.name} causando {vampirismDamage} de dano.");
            }
        }

        if (playerHealth != null)
        {
            playerHealth.health += vampirismHeal;
            if (playerHealth.health > playerHealth.healthMax)
                playerHealth.health = playerHealth.healthMax;

            Debug.Log($"Player recuperou {vampirismHeal} de vida pelo vampirismo.");
        }
    }

    // Visualização no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, vampirismRange);
    }
}
