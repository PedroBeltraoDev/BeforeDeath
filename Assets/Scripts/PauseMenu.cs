using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    private bool jogoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
                Continuar();
            else
                Pausar();
        }
    }

    public void Pausar()
    {
        pauseUI.SetActive(true);
        // Time.timeScale = 0f;  // Não pausar o jogo
        jogoPausado = true;
    }

    public void Continuar()
    {
        pauseUI.SetActive(false);
        // Time.timeScale = 1f;  // Não retomar o tempo porque não foi pausado
        jogoPausado = false;
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    public void VoltarAoMenu(string nomeCena)
    {
        // Time.timeScale = 1f;  // Pode deixar isso aqui, por segurança
        SceneManager.LoadScene(nomeCena);
    }
}
