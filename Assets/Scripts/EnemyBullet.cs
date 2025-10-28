using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed; // velocidade da bala
    Vector2 _direction; // direção da bala (direção do player)
    bool isReady; // precisamos saber quando a bala estar pronta para ser disparada]

    // vamos setar valores padrão na função de awake
    void Awake()
    {
        speed = 5f;
        isReady = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetDirection (Vector2 direction)
    {
        _direction = direction.normalized;

        isReady = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            // pegar a pos atual da bala
            Vector2 position = transform.position;

            // calcular nova pos da bala
            position += _direction * speed * Time.deltaTime;

            // atualiza a nova pos da bala
            transform.position = position;

            // vamos destruir o obj da bala quando ela sair da tela
            Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

            if ((transform.position.x < min.x) || (transform.position.x > max.x) ||
                (transform.position.y < min.y) || (transform.position.y > max.y))
            {
                Destroy(gameObject);
            }
        }
    }
}
