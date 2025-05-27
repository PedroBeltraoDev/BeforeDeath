using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider slider;
    public float health = 100;
    public float healthMax = 150;

    private void Update()
    {
        health = Math.Clamp(health, 0, healthMax);

        if (slider != null)
        {
            slider.value = health;
            slider.maxValue = healthMax;
        }
    }

    // Método para aplicar dano
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Math.Clamp(health, 0, healthMax);

        if (health <= 0)
        {
            Die();
        }
    }

    // NOVO: Método para curar
    public void Heal(float amount)
    {
        health += amount;
        health = Math.Clamp(health, 0, healthMax);
        Debug.Log($"Curado: +{amount}, Vida atual: {health}");
    }

    private void Die()
    {
        Debug.Log("Player morreu!");
        // Exemplo: gameObject.SetActive(false);
    }
}
