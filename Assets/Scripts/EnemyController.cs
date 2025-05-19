using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int maxHealth = 100;
    public int baseDamage = 8;
    public float gravityScale = 1f;

    private int currentHealth;
    private float lastAttackTime;
    private Transform targetPlayer;
    private Animator animator;
    private Rigidbody2D rb;
    private float spawnTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        currentHealth = maxHealth;
        spawnTime = Time.time;

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
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y); // ✅ CORREÇÃO AQUI
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // ✅ Mantém velocidade vertical
            animator.SetBool("isWalking", false);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;

                PlayerHealth player = targetPlayer.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    int scaledDamage = baseDamage + Mathf.FloorToInt((Time.time - spawnTime) / 30f);
                    player.TakeDamage(scaledDamage);
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

        WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();
        if (waveSpawner != null)
            waveSpawner.EnemyDied();

        Destroy(gameObject, 1f);
    }

    public void ResetAttack()
    {
        animator.ResetTrigger("Attack");
    }
}

