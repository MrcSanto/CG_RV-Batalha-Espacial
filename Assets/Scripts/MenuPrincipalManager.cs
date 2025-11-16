using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [Header("Menu Principal")]
    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;

    private bool menuAberto = false;
    private string cenaAtual;

    private void Start()
    {
        // Guarda o nome da cena atual para restart no pause
        cenaAtual = SceneManager.GetActiveScene().name;

        // Se for a cena do jogo, garantimos que o menu de pause comece fechado
        if (painelOpcoes != null)
            painelOpcoes.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        // Só permite abrir com ESC se NÃO for o menu principal
        if (SceneManager.GetActiveScene().name != nomeDoLevelDeJogo)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuAberto)
                    FecharOpções();
                else
                    AbrirOpções();
            }
        }
    }

    // --------------------------
    // FUNÇÕES ORIGINAIS
    // --------------------------

    public void Jogar()
    {
        SceneManager.LoadScene(nomeDoLevelDeJogo);
    }

    public void AbrirOpções()
    {
        if (painelMenuInicial != null)
            painelMenuInicial.SetActive(false);

        painelOpcoes.SetActive(true);
        Time.timeScale = 0f;
        menuAberto = true;
    }

    public void FecharOpções()
    {
        painelOpcoes.SetActive(false);

        if (painelMenuInicial != null)
            painelMenuInicial.SetActive(true);

        Time.timeScale = 1f;
        menuAberto = false;
    }

    public void SairJogo()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }

    // --------------------------
    // NOVO: EXIT DO PAUSE → RESTART
    // --------------------------
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaAtual);
    }
}
