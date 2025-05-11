using UnityEngine;

public class Movimento : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

    [Header("Movimentação")]
    [SerializeField] private int velocidade = 5;

    [Header("Pulo e Pulo Duplo")]
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;
    [SerializeField] private int maximoPulos = 2;
    private int quantidadePulos;
    private bool estaNoChao;

    [Header("Dash")]
    [SerializeField] private float velocidadeDash = 15f;
    [SerializeField] private float duracaoDash = 0.2f;
    [SerializeField] private float tempoRecargaDash = 1f;
    private bool podeDarDash = true;
    private bool estaDandoDash = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int movendoHash = Animator.StringToHash("movendo");
    private int saltandoHash = Animator.StringToHash("saltando");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        // Reseta pulos ao tocar o chão
        if (estaNoChao)
        {
            quantidadePulos = maximoPulos;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        // Pulo e pulo duplo
        if (Input.GetKeyDown(KeyCode.Space) && quantidadePulos > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Cancela impulso vertical anterior
            rb.AddForce(Vector2.up * 600);
            quantidadePulos--;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && podeDarDash)
        {
            StartCoroutine(FazerDash());
        }

        // Animações
        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(saltandoHash, !estaNoChao);

        // Virar sprite
        if (horizontalInput > 0) spriteRenderer.flipX = false;
        else if (horizontalInput < 0) spriteRenderer.flipX = true;
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

        float direcao = spriteRenderer.flipX ? -1f : 1f;
        rb.linearVelocity = new Vector2(direcao * velocidadeDash, rb.linearVelocity.y); // Mantém a força do pulo

        yield return new WaitForSeconds(duracaoDash);
        estaDandoDash = false;

        yield return new WaitForSeconds(tempoRecargaDash);
        podeDarDash = true;
    }
}
