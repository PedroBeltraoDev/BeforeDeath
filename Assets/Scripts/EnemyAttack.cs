using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public float damage = 15f;
    public Transform player;
    public LayerMask playerLayer;

    private HealthSystem playerHealth;

    private float attackCooldown = 2.5f;  // cooldown em segundos
    private float lastAttackTime = -Mathf.Infinity;

    private void Start()
    {
        if (player != null)
            playerHealth = player.GetComponent<HealthSystem>();
    }

    private void Update()
    {
        if (playerHealth == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;  // atualiza o tempo do último ataque
        }
    }

    private void Attack()
    {
        playerHealth.TakeDamage(damage);
        Debug.Log("Inimigo atacou e causou " + damage + " de dano.");
    }
}
