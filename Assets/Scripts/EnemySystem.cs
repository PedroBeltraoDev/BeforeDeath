using UnityEngine;
using UnityEngine.UI;

public class EnemySystem : MonoBehaviour
{
    private Animator animator;
    [Header("Health Settings")]
    public float healthMax = 100f;
    private float health;

    [Header("UI")]
    public Slider slider;

    private void Start()
    {
        health = healthMax;

        if (slider != null)
        {
            slider.maxValue = healthMax;
            slider.value = health;
        }
        else
        {
            Debug.LogWarning("Slider da vida não atribuído em " + gameObject.name);
        }
    }

    private void Update()
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, healthMax);

        if (slider != null)
        {
            slider.value = health;
        }

        if (health <= 0)
        {
            Die();
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} morreu!");
        animator.SetTrigger("Die");
        Destroy(gameObject, 0.5f); // Dá meio segundo pra animação tocar antes de sumir
    }
}
