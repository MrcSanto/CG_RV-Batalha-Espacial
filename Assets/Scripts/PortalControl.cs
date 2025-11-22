using UnityEngine;

public class PortalControl : MonoBehaviour
{
    public GameObject gameManagerGO;
    private const int MIN_ENEMIES_FOR_VICTORY = 5;

    private bool playerEntered = false;
    private bool isMoving = false;
    private float moveSpeed = 2f;

    void Start()
    {
        // Encontra o GameManager se não foi atribuído
        if (gameManagerGO == null)
        {
            gameManagerGO = GameObject.FindGameObjectWithTag("GameManagerTag");
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveDown();
            CheckIfOffScreen();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Verifica se colidiu com o player
        if (col.tag == "PlayerShipTag" && !playerEntered)
        {
            playerEntered = true;
            CheckVictoryCondition();
        }
    }

    void CheckVictoryCondition()
    {
        // Pega a quantidade de inimigos eliminados do EnemyControl
        int enemiesDestroyed = EnemyControl.enemiesDestroyed;

        GameManager gameManager = gameManagerGO.GetComponent<GameManager>();

        if (enemiesDestroyed >= MIN_ENEMIES_FOR_VICTORY)
        {
            gameManager.SetGameManagerState(GameManager.GameManagerState.Victory);
            StartMoving();
        }
        else
        {
            gameManager.SetGameManagerState(GameManager.GameManagerState.GameOver);
            StartMoving();
        }
        
    }

    void StartMoving()
    {
        isMoving = true;
    }

    void MoveDown()
    {
        // Move o portal para baixo
        Vector2 position = transform.position;
        position.y -= moveSpeed * Time.deltaTime;
        transform.position = position;
    }

    void CheckIfOffScreen()
    {
        // Pega o limite inferior da tela
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // Se o portal saiu da tela, destrói
        if (transform.position.y < min.y - 1f)
        {
            Destroy(gameObject);
        }
    }
}