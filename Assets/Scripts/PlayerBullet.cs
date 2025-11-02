using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 8f;
    private Vector2 direction;

    // Chamado logo após o Instantiate, pelo PlayerControl
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // Move na direção definida
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Destroi se sair da tela
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyShipTag") || col.CompareTag("AsteroidEntityTag"))
        {
            Destroy(gameObject);
        }
    }
}
