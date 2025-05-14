using UnityEngine;

public class Movimento : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

    [Header("Movimentação")]
    [SerializeField] private int velocidade = 5;

    [Header("Pulo e Pulo Duplo")]
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask layerParaPulo;       // Chao + Plataforma
    [SerializeField] private LayerMask layerChaoSolido;      // Só Chao
    [SerializeField] private int maximoPulos = 2;
    private int quantidadePulos;
    private bool estaNoChao;

    [Header("Dash")]
    [SerializeField] private float velocidadeDash = 15f;
    [SerializeField] private float duracaoDash = 0.2f;
    [SerializeField] private float tempoRecargaDash = 1f;
    private bool podeDarDash = true;
    private bool estaDandoDash = false;

    private SpriteRenderer spriteRenderer;
    private int layerOriginal;
    [SerializeField] private string layerIgnoraPlataforma = "IgnoraPlataforma";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        layerOriginal = gameObject.layer;
    }

    void Update()
    {
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, layerParaPulo);

        if (estaNoChao && rb.linearVelocity.y <= 0.1f)
        {
            quantidadePulos = maximoPulos;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && quantidadePulos > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 12f);
            quantidadePulos--;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && podeDarDash)
        {
            StartCoroutine(FazerDash());
        }

        if (Input.GetKeyDown(KeyCode.S) && !Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, layerChaoSolido))
        {
            StartCoroutine(CairDaPlataforma());
        }

        if (horizontalInput > 0) spriteRenderer.flipX = true;
        else if (horizontalInput < 0) spriteRenderer.flipX = false;
    }

    private void FixedUpdate()
    {
        if (!estaDandoDash)
        {
            rb.linearVelocity = new Vector2(horizontalInput * velocidade, rb.linearVelocity.y);
        }
    }

    private System.Collections.IEnumerator FazerDash()
    {
        podeDarDash = false;
        estaDandoDash = true;

        float direcao = horizontalInput != 0 ? Mathf.Sign(horizontalInput) : (spriteRenderer.flipX ? -1f : 1f);
        rb.linearVelocity = new Vector2(direcao * velocidadeDash, rb.linearVelocity.y);

        yield return new WaitForSeconds(duracaoDash);
        estaDandoDash = false;

        yield return new WaitForSeconds(tempoRecargaDash);
        podeDarDash = true;
    }

    private System.Collections.IEnumerator CairDaPlataforma()
    {
        gameObject.layer = LayerMask.NameToLayer(layerIgnoraPlataforma);
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = layerOriginal;
    }
}
