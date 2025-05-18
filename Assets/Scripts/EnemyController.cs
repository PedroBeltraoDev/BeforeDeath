using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int maxHealth = 3;
    public int damage = 1;

    private Transform targetPlayer;
    private Animator animator;
    private Rigidbody2D rb;
    private float lastAttackTime;
    private int currentHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

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

        // Movimento
        if (distance > attackRange)
        {
            Vector2 direction = (targetPlayer.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);

            // Ataque
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                animator.SetBool("isAttacking", true);
                lastAttackTime = Time.time;
                // Aqui você pode chamar uma função para causar dano ao jogador
            }
        }
    }

    // Chame isso quando o inimigo levar dano
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
        Destroy(gameObject, 1f); // Aguarda animação de morte antes de destruir
    }

    // Exemplo de resetar o ataque após a animação
    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}
