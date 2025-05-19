using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int maxHealth = 100;
    public int baseDamage = 8;
    public float difficultyRampTime = 60f; // A cada 60 segundos o dano aumenta
    public float gravityScale = 4f;

    private Transform targetPlayer;
    private Animator animator;
    private Rigidbody2D rb;
    private float lastAttackTime;
    private int currentHealth;
    private float spawnTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        currentHealth = maxHealth;
        spawnTime = Time.time;

        // Procura automaticamente um dos jogadores
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
            targetPlayer = players[Random.Range(0, players.Length)].transform;
    }

    void Update()
    {
        if (targetPlayer == null || currentHealth <= 0)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        float distance = Vector2.Distance(transform.position, targetPlayer.position);

        if (distance > attackRange)
        {
            Vector2 direction = (targetPlayer.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y); // Mantém gravidade
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            animator.SetBool("isWalking", false);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                animator.SetBool("isAttacking", true);
                lastAttackTime = Time.time;

                int difficultyBonus = Mathf.FloorToInt((Time.time - spawnTime) / difficultyRampTime);
                int totalDamage = baseDamage + difficultyBonus;

                if (targetPlayer != null)
                {
                    PlayerHealth player = targetPlayer.GetComponent<PlayerHealth>();
                    if (player != null)
                        player.TakeDamage(totalDamage);
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;

        // Avisa o jogador que matou o inimigo
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.OnEnemyKilled();
        }

        Destroy(gameObject, 1f); // Aguarda animação de morte
    }

    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}
