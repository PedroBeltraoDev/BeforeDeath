using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator animador;
    private Rigidbody2D rbEnemy;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    [Header("Movimentação")]
    [SerializeField] private float velocidade = 3f;
    [SerializeField] private float alcanceDeteccao = 10f;

    [Header("Pulo")]
    [SerializeField] private float forcaPulo = 12f;
    [SerializeField] private Transform detectorChao;
    [SerializeField] private LayerMask camadaChao;
    private bool estaNoChao;
    private const float RAIO_DETECCAO_CHAO = 0.3f; // Aumentado para melhor detecção

    [Header("Pulo Inteligente")]
    [SerializeField] private Transform detectorParede;
    [SerializeField] private Transform detectorParede2;

    [SerializeField] private float distanciaParede = 0.5f;

    private void Awake()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer, true);
    }

    private void Update()
    {
        VerificarChao();
        VerificarDistanciaJogador();
        AtualizarAnimacoes();
        AtualizarOrientacao();
        TentarPular();
    }

    private void VerificarDistanciaJogador()
    {
        if (Vector2.Distance(transform.position, player.position) < alcanceDeteccao)
        {
            Vector2 direcao = (player.position - transform.position).normalized;
            Vector2 velocidadeAtual = rbEnemy.linearVelocity;
            rbEnemy.linearVelocity = new Vector2(direcao.x * velocidade, velocidadeAtual.y);
        }
        else
        {
            rbEnemy.linearVelocity = new Vector2(0, rbEnemy.linearVelocity.y);
        }
    }

    private void AtualizarAnimacoes()
    {
        animador.SetFloat("EhorizontalAnim", Mathf.Abs(rbEnemy.linearVelocity.x));
        animador.SetFloat("EverticalAnim", rbEnemy.linearVelocity.y);
        animador.SetBool("GroundCheck", estaNoChao);
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
        estaNoChao = Physics2D.OverlapCircle(detectorChao.position, RAIO_DETECCAO_CHAO, camadaChao);
    }

    private bool TemParedeNaFrente()
    {
        bool indoParaDireita = rbEnemy.linearVelocity.x > 0;
        Vector2 direcao = indoParaDireita ? Vector2.right : Vector2.left;
        Transform detectorUsado = indoParaDireita ? detectorParede : detectorParede2;

        RaycastHit2D hit = Physics2D.Raycast(detectorUsado.position, direcao, distanciaParede, camadaChao);

        if (hit.collider != null)
        {
            Debug.Log("Raycast acertou: " + hit.collider.name);
        }

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

        if (detectorParede != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(detectorParede.position, detectorParede.position + Vector3.right * distanciaParede);
        }

        if (detectorParede2 != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(detectorParede2.position, detectorParede2.position + Vector3.left * distanciaParede);
        }
    }

#endif
}
