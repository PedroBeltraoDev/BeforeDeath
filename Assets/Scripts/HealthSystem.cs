using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour 
{
    public Slider slider;
    public float health = 100;
    public float healthMax = 150;

    private GameOverScreen gameOverScreen;

private void Start()
{
    gameOverScreen = FindObjectOfType<GameOverScreen>();
}

    void Update()
{
    if (slider != null)
    {
        slider.value = health;
        slider.maxValue = healthMax;
    }

    // Teste manual: aperte 'K' para tomar 10 de dano
    if (Input.GetKeyDown(KeyCode.K))
    {
        TakeDamage(10f);
    }
}


    // Método para aplicar dano
    public void TakeDamage(float damage)
    {
        Debug.Log($"TakeDamage chamado, dano: {damage}, vida antes: {health}");
        health -= damage;
        health = Mathf.Clamp(health, 0, healthMax);
        Debug.Log($"Vida após dano: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    // Método para curar
    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, healthMax);
        Debug.Log($"Curado: +{amount}, Vida atual: {health}");
    }

    void Die()
{
    health = 0;  // Força a vida a zero
    Debug.Log("Player morreu!");

    if (gameOverScreen != null)
{
    gameOverScreen.ShowGameOver();
}
else
{
    Debug.LogWarning("GameOverScreen não encontrado na cena!");
}

    enabled = false; // desativa este script (HealthSystem)
    // Se quiser, desative o movimento ou controles aqui também
    // Exemplo: GetComponent<PlayerMovement>().enabled = false;

    Destroy(gameObject, 1.0f);  // Destrói o player após 1 segundo
}

    }

