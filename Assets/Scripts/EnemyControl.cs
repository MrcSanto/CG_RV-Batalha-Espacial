using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    public GameObject ExplosionGO;
    public GameObject scoreUITextGO;

    public Text enemyDestroyedText;

    float speed;
    private bool isDestroyed = false;

    // Variável estática para contar inimigos destruídos
    public static int enemiesDestroyed = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 2f;

        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

        if (enemyDestroyedText == null)
        {
            GameObject textGO = GameObject.FindGameObjectWithTag("EnemyDestroyedTextTag");
            if (textGO != null)
            {
                enemyDestroyedText = textGO.GetComponent<Text>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // vamos pegar a posição atual do inimigo
        Vector2 position = transform.position;

        // calcular a nova posição do inimigo
        position = new Vector2 (position.x, position.y - speed * Time.deltaTime);

        // atualizar a posição do inimigo
        transform.position = position;

        // essa é o canto inferior esquerdo da tela 
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

        // se o inimigo sair da tela, na parte inferior, vamos destruir o objeto
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isDestroyed) return;

        if ((col.tag == "PlayerShipTag") || (col.tag == "PlayerBulletTag"))
        {
            PlayExplosion();
            isDestroyed = true;

            // incrementando a pontuacao
            scoreUITextGO.GetComponent<ScoreManager>().Score += 50;

            // incrementando o contador de inimigos destruidos
            enemiesDestroyed++;
            UpdateEnemyDestroyedText();

            Destroy (gameObject);
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate (ExplosionGO);

        explosion.transform.position = transform.position;
    }

    void UpdateEnemyDestroyedText()
    {
        if (enemyDestroyedText != null)
        {
            Text textComponent = enemyDestroyedText.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = enemiesDestroyed.ToString();

                // muda a cor conforme o número de inimigos destruídos
                if (enemiesDestroyed >= 5)
                {
                    textComponent.color = Color.green;
                }
            }
        }
    }


    // Método para resetar o contador
    public static void ResetEnemiesDestroyed()
    {
        enemiesDestroyed = 0;

        GameObject textGO = GameObject.FindGameObjectWithTag("EnemyDestroyedTextTag");
        if (textGO != null)
        {
            Text textComponent = textGO.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = "0";
                textComponent.color = Color.red;   // 0 → Vermelho
            }
        }
    }

}
