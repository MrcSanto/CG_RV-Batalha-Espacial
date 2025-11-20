using UnityEngine;

public class AsteroidControl : MonoBehaviour
{
    public GameObject scoreUITextGO;
    public GameObject ExplosionGO;
    float speed;
    private bool isDestroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 2f;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    // Update is called once per frame
    void Update()
    {
        // vamos pegar a posição atual do asteroide
        Vector2 position = transform.position;

        // calcular a nova posição do asteroide
        position = new Vector2 (position.x, position.y - speed * Time.deltaTime);

        // atualizar a posição do asteroide
        transform.position = position;

        // essa é o canto inferior esquerdo da tela 
        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

        // se o asteroide sair da tela, na parte inferior, vamos destruir o objeto
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

            scoreUITextGO.GetComponent<ScoreManager>().Score += 25;
            Destroy (gameObject);
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = Instantiate(ExplosionGO, transform.position, Quaternion.identity);

        explosion.transform.localScale = transform.localScale;

        // explosion.transform.position = transform.position;
    }
}
