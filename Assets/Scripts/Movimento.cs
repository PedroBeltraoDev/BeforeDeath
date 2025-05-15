using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class Movimento : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Animator animador;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Movimentação")]
    [SerializeField] private float velocidade = 5f;
    private float entradaHorizontal;

    [Header("Pulo")]
    [SerializeField] private Transform detectorChao;
    [SerializeField] private LayerMask camadaPulo;           // Chão + Plataformas
    [SerializeField] private LayerMask camadaChaoSolido;     // Apenas Chão
    [SerializeField] private int maxPulos = 2;
    private int pulosRestantes;
    private bool estaNoChao;

    [Header("Dash")]
    [SerializeField] private float velocidadeDash = 15f;
    [SerializeField] private float duracaoDash = 0.2f;
    [SerializeField] private float tempoRecargaDash = 1f;
    private bool podeDarDash = true;
    private bool estaDandoDash = false;

    [Header("Plataformas")]
    [SerializeField] private string nomeLayerIgnoraPlataforma = "IgnoraPlataforma";
    private int layerOriginal;

    private const float RAIO_DETECCAO_CHAO = 0.2f;
    private const float FORCA_PULO = 12f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        layerOriginal = gameObject.layer;
    }

    private void Update()
    {
        AtualizarEntrada();
        VerificarChao();
        AtualizarPulos();
        LidarComPulo();
        LidarComDash();
        LidarComDescidaPlataforma();
        AtualizarOrientacaoSprite();
        AtualizarAnimacoes();
    }

    private void FixedUpdate()
    {
        if (!estaDandoDash)
            rb.linearVelocity = new Vector2(entradaHorizontal * velocidade, rb.linearVelocity.y);
    }

    private void AtualizarEntrada()
    {
        entradaHorizontal = Input.GetAxisRaw("Horizontal");
    }

    private void VerificarChao()
    {
        estaNoChao = Physics2D.OverlapCircle(detectorChao.position, RAIO_DETECCAO_CHAO, camadaPulo);
    }

    private void AtualizarPulos()
    {
        if (estaNoChao && rb.linearVelocity.y <= 0.1f)
            pulosRestantes = maxPulos;
    }

    private void LidarComPulo()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pulosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, FORCA_PULO);
            pulosRestantes--;
        }
    }

    private void LidarComDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && podeDarDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void LidarComDescidaPlataforma()
    {
        bool sobreChaoSolido = Physics2D.OverlapCircle(detectorChao.position, RAIO_DETECCAO_CHAO, camadaChaoSolido);

        if (Input.GetKeyDown(KeyCode.S) && !sobreChaoSolido)
        {
            StartCoroutine(DescerPlataforma());
        }
    }

    private void AtualizarOrientacaoSprite()
    {
        if (entradaHorizontal > 0)
            spriteRenderer.flipX = true;
        else if (entradaHorizontal < 0)
            spriteRenderer.flipX = false;
    }

    private System.Collections.IEnumerator Dash()
    {
        podeDarDash = false;
        estaDandoDash = true;

        float direcao = entradaHorizontal != 0 ? Mathf.Sign(entradaHorizontal) : (spriteRenderer.flipX ? 1f : -1f);
        rb.linearVelocity = new Vector2(direcao * velocidadeDash, rb.linearVelocity.y);

        yield return new WaitForSeconds(duracaoDash);
        estaDandoDash = false;

        yield return new WaitForSeconds(tempoRecargaDash);
        podeDarDash = true;
    }

    private System.Collections.IEnumerator DescerPlataforma()
    {
        gameObject.layer = LayerMask.NameToLayer(nomeLayerIgnoraPlataforma);
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = layerOriginal;
    }

    private void AtualizarAnimacoes()
    {
        animador.SetFloat("HorizontalAnim", Mathf.Abs(rb.linearVelocity.x));
        animador.SetFloat("VerticalAnim", rb.linearVelocity.y);
        animador.SetBool("GroundCheck", estaNoChao);
        animador.SetBool("DashCheck", estaDandoDash);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (detectorChao != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectorChao.position, RAIO_DETECCAO_CHAO);
        }
    }
#endif
}
