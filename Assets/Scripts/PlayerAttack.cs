using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float damage = 15f;
    public float attackCooldown = 1f;
    private float lastAttackTime = -Mathf.Infinity;

    public LayerMask enemyLayer;

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
        // Clique esquerdo para atacar
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }

        // Tecla E para usar Vampirismo
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryVampirism();
        }
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
        else
        {
            float wait = (lastAttackTime + attackCooldown) - Time.time;
            Debug.Log($"Ataque em cooldown. Aguarde {wait:F1} segundos.");
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemySystem enemySystem = enemy.GetComponent<EnemySystem>();
            if (enemySystem != null)
            {
                enemySystem.TakeDamage(damage);
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
            EnemySystem enemySystem = enemy.GetComponent<EnemySystem>();
            if (enemySystem != null)
            {
                enemySystem.TakeDamage(vampirismDamage);
                Debug.Log($"Vampirismo atingiu {enemy.name} causando {vampirismDamage} de dano.");
            }
        }

        if (playerHealth != null)
        {
            playerHealth.health += vampirismHeal;
            playerHealth.health = Mathf.Clamp(playerHealth.health, 0, playerHealth.healthMax);
            Debug.Log($"Player recuperou {vampirismHeal} de vida.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, vampirismRange);
    }
}
