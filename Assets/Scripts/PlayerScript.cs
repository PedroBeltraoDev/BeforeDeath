using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody2D corpoPersonagem;
    private SpriteRenderer spriteRenderer;

    [Header("Movimentação")]
    public float velocidade = 5f;
    public float forcaPulo = 10f;
    private float direcaoHorizontal;
    private bool noChao;
    private int contadorPulos; // Novo

    [Header("Dash")]
    public float dashVelocidade = 15f;
    public float dashTempo = 0.2f;
    public float dashCooldown = 1f;
    private bool estaDandoDash = false;
    private float tempoUltimoDash;

    [Header("Checagem de Chão")]
    public LayerMask camadaChao;
    public Transform checagemChao;
    public float raioChao = 0.1f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        direcaoHorizontal = Input.GetAxisRaw("Horizontal");
        noChao = Physics2D.OverlapCircle(checagemChao.position, raioChao, camadaChao);

        // Resetar contador de pulos ao tocar o chão
        if (noChao)
        {
            contadorPulos = 0;
        }

        if (!estaDandoDash)
        {
            MoverHorizontalmente();
            VirarSprite();
        }

        // Pulo (agora com pulo duplo)
        if (Input.GetKeyDown(KeyCode.Space) && contadorPulos < 2)
        {
            Pular();
            contadorPulos++;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !estaDandoDash && Time.time >= tempoUltimoDash + dashCooldown)
        {
            Vector2 direcaoDash = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
            if (direcaoDash != Vector2.zero)
            {
                StartCoroutine(FazerDash(direcaoDash));
            }
        }
    }

    private void MoverHorizontalmente()
    {
        corpoPersonagem.linearVelocity = new Vector2(direcaoHorizontal * velocidade, corpoPersonagem.linearVelocity.y);
    }

    private void VirarSprite()
    {
        if (direcaoHorizontal != 0)
        {
            spriteRenderer.flipX = direcaoHorizontal > 0;
        }
    }

    private void Pular()
    {
        corpoPersonagem.linearVelocity = new Vector2(corpoPersonagem.linearVelocity.x, 0);
        corpoPersonagem.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
    }

    private IEnumerator FazerDash(Vector2 direcao)
    {
        estaDandoDash = true;
        corpoPersonagem.linearVelocity = direcao * dashVelocidade;
        yield return new WaitForSeconds(dashTempo);
        estaDandoDash = false;
        tempoUltimoDash = Time.time;
    }

    private void OnDrawGizmosSelected()
    {
        if (checagemChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checagemChao.position, raioChao);
        }
    }
}
