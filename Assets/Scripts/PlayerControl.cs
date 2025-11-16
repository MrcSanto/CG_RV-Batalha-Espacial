using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public GameObject PlayerBulletGO;
    public GameObject BulletPosition01;
    public GameObject BulletPosition02;
    public GameObject ExplosionGO;

    public float speed = 5f;
    public float bulletSpeed = 10f;

    private Vector2 movement;
    private Vector2 shootDirection = Vector2.up; // sempre atira pra cima

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float x = 0f;
        float y = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1f;

        movement = new Vector2(x, y).normalized;
        Move(movement);

        // Disparo (tecla espaço ou botão esquerdo)
        if (keyboard.spaceKey.wasPressedThisFrame || Mouse.current?.leftButton.wasPressedThisFrame == true)
        {
            ShootForward();
        }

        // 🔹 Sempre olhar para cima
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void ShootForward()
    {
        ShootBullet(BulletPosition01.transform.position, shootDirection);
        ShootBullet(BulletPosition02.transform.position, shootDirection);
    }

    void ShootBullet(Vector3 startPos, Vector2 direction)
    {
        GameObject bullet = Instantiate(PlayerBulletGO, startPos, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        // Sempre aponta pra cima
        bullet.transform.rotation = Quaternion.identity;
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x -= 0.225f;
        min.x += 0.225f;
        max.y -= 0.285f;
        min.y += 0.285f;

        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (
            col.CompareTag("EnemyShipTag") ||
            col.CompareTag("EnemyBulletTag") ||
            col.CompareTag("AsteroidEntityTag")
        )
        {
            PlayExplosion();
            Destroy(gameObject);
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate (ExplosionGO);

        explosion.transform.position = transform.position;
    }
}
