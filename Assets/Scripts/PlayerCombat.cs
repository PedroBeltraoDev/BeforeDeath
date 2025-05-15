using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Ataque")]
    public Transform attackOrigin;
    public float attackRadius = 0.2f;
    public LayerMask enemyMask;
    public int damage = 10;

    [Header("Defesa")]
    public KeyCode defendKey = KeyCode.K;
    public bool isDefending = false;

    void Update()
    {
        HandleAttackInput();
        HandleDefenseInput();
    }

    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void HandleDefenseInput()
    {
        isDefending = Input.GetKey(defendKey); // enquanto segura, está defendendo
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                enemyScript.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }
}
