using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Componentes")]
    private Animator animador;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Ataque")]
    [SerializeField] private float tempoRecarga = 0.5f;
    [SerializeField] private float raioAtaque = 0.7f;
    [SerializeField] private Transform pontoDeAtaque;
    [SerializeField] private LayerMask camadaInimigo;
    [SerializeField] private float dano = 25f;

    private bool podeAtacar = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animador = GetComponent<Animator>();
    }

    private void Update()
    {
        Atacar();
    }

    private void Atacar()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && podeAtacar)
        {
            animador.SetTrigger("AttackCheck");
            DetectarInimigos();
            StartCoroutine(RecargaAtaque());
        }
    }

    private void DetectarInimigos()
    {
        Collider2D[] inimigos = Physics2D.OverlapCircleAll(pontoDeAtaque.position, raioAtaque, camadaInimigo);

        foreach (Collider2D inimigo in inimigos)
        {
            EnemySystem enemySystem = inimigo.GetComponent<EnemySystem>();
            if (enemySystem != null)
            {
                enemySystem.TakeDamage(dano);
            }
        }
    }

    private System.Collections.IEnumerator RecargaAtaque()
    {
        podeAtacar = false;
        yield return new WaitForSeconds(tempoRecarga);
        podeAtacar = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (pontoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pontoDeAtaque.position, raioAtaque);
        }
    }
}
