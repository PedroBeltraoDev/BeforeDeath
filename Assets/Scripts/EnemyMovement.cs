using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rbEnemy;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private HealthSystem playerHealth;
    private bool isAttacking;

    [Header("Movimentação")]
    [SerializeField] private float velocidade = 3f;
    [SerializeField] private float alcanceDeteccao = 10f;
    [SerializeField] private float distanciaAtaque = 1.5f;

    [Header("Ataque")]
    [SerializeField] private float dano = 10f;
    [SerializeField] private float tempoEntreAtaques = 1f;
    private float tempoProximoAtaque;

    [Header("Pulo")]
    [SerializeField] private float forcaPulo = 12f;
    [SerializeField] private Transform detectorChao;
    [SerializeField] private LayerMask camadaChao;
    private bool estaNoChao;
    private const float raioDeteccaoChao = 0.3f;

    [Header("Pulo Inteligente")]
    [SerializeField] private Transform detectorParedeDireita;
    [SerializeField] private Transform detectorParedeEsquerda;
    [SerializeField] private float distanciaParede = 0.5f;

    private void Awake()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<HealthSystem>();
        }

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer, true);
    }

    private void Update()
    {
        if (player == null) return;

        tempoProximoAtaque -= Time.deltaTime;

        VerificarChao();
        VerificarDistanciaJogador();
        AtualizarOrientacao();
        TentarPular();
    }

    private void VerificarDistanciaJogador()
    {
        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia < distanciaAtaque)
        {
            // Parar para atacar
            rbEnemy.linearVelocity = new Vector2(0, rbEnemy.linearVelocity.y);
            isAttacking = true;

            if (tempoProximoAtaque <= 0f)
            {
                Atacar();
                tempoProximoAtaque = tempoEntreAtaques;
            }
        }
        else if (distancia < alcanceDeteccao)
        {
            // Perseguir o jogador
            Vector2 direcao = (player.position - transform.position).normalized;
            rbEnemy.linearVelocity = new Vector2(direcao.x * velocidade, rbEnemy.linearVelocity.y);
            isAttacking = false;
        }
        else
        {
            // Fica parado
            rbEnemy.linearVelocity = new Vector2(0, rbEnemy.linearVelocity.y);
            isAttacking = false;
        }
    }

    private void Atacar()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(dano);
            Debug.Log("Inimigo causou " + dano + " de dano ao player.");
        }
    }

    private void AtualizarOrientacao()
    {
        if (rbEnemy.linearVelocity.x > 0)
            spriteRenderer.flipX = false;
        else if (rbEnemy.linearVelocity.x < 0)
            spriteRenderer.flipX = true;
    }

    private void VerificarChao()
    {
        estaNoChao = Physics2D.OverlapCircle(detectorChao.position, raioDeteccaoChao, camadaChao);
    }

    private bool TemParedeNaFrente()
    {
        bool indoParaDireita = rbEnemy.linearVelocity.x > 0;
        Vector2 direcao = indoParaDireita ? Vector2.right : Vector2.left;
        Transform detectorUsado = indoParaDireita ? detectorParedeDireita : detectorParedeEsquerda;

        RaycastHit2D hit = Physics2D.Raycast(detectorUsado.position, direcao, distanciaParede, camadaChao);

        return hit.collider != null;
    }

    private void TentarPular()
    {
        if (estaNoChao &&
            Vector2.Distance(transform.position, player.position) < alcanceDeteccao &&
            TemParedeNaFrente())
        {
            rbEnemy.linearVelocity = new Vector2(rbEnemy.linearVelocity.x, forcaPulo);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcanceDeteccao);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);

        if (detectorParedeDireita != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(detectorParedeDireita.position, detectorParedeDireita.position + Vector3.right * distanciaParede);
        }

        if (detectorParedeEsquerda != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(detectorParedeEsquerda.position, detectorParedeEsquerda.position + Vector3.left * distanciaParede);
        }
    }
#endif
}