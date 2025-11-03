using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [Header("Velocidade de descida")]
    public float speed = 2f;

    private float minY;

    void Awake()
    {
        // Limite inferior da tela
        minY = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
    }

    void Update()
    {
        // Move para baixo
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Destrói o objeto quando sair da tela
        if (transform.position.y < minY - GetComponent<SpriteRenderer>().bounds.extents.y)
        {
            Destroy(gameObject);
        }
    }
}
