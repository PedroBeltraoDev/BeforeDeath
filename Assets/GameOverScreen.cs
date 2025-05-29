using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverPanel; // O painel que mostra o texto e botões

    private void Start()
    {
        gameOverPanel.SetActive(false);  // Começa desativado
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);  // Ativa o painel
    }
}
