using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 20;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Inimigo levou dano. Vida restante: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
