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

        slider.value = health;
        slider.maxValue = healthMax;
    }

    // Novo método para aplicar dano
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Math.Clamp(health, 0, healthMax);
    }
}
