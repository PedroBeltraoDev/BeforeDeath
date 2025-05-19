using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemySystem : MonoBehaviour
{
    public Slider slider;
    public float health = 100;
    public float healthMax = 100;

    private void Update()
    {
        health = Math.Clamp(health, 0, healthMax);

        slider.value = health;
        slider.maxValue = healthMax;
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

    private void Die()
    {
        // Aqui você pode colocar a lógica de morte do player,
        // por exemplo desabilitar o personagem, ativar animação de morte, etc.
        Debug.Log("Player morreu!");
        // Exemplo: gameObject.SetActive(false);
    }
}
