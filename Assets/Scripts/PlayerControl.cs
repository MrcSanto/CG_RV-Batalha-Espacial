using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;

    public GameObject PlayerBulletGO;
    public GameObject BulletPosition01;
    public GameObject BulletPosition02;
    public GameObject ExplosionGO;
    public GameObject HealthBar;
    private GameObject[] healthDots;

    public float speed = 5f;
    public float bulletSpeed = 10f;

    const int MAX_LIVES = 3; // n max de vidas do jogador
    int lives; // n de vidas atual do jogador

    private Vector2 movement;
    private Vector2 shootDirection = Vector2.up; // sempre atira pra cima

    public void Init()
    {
        lives = MAX_LIVES;
        gameObject.SetActive (true);

        healthDots = new GameObject[HealthBar.transform.childCount];

        for (int i = 0; i < healthDots.Length; i++)
            healthDots[i] = HealthBar.transform.GetChild(i).gameObject;
        
        // Ativa todas as barras (reset)
        foreach (var bar in healthDots)
            bar.SetActive(true);

        // resetando a posição do jogador na tela
        transform.position = new Vector2 (0, 0);
    }

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
        // Velocidade com Shift aumenta em (50%)
        float currentSpeed = speed;
        if (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed)
        {
            currentSpeed = speed * 1.5f;
        }

        Move(movement, currentSpeed);

        // Disparo (tecla espaço ou botão esquerdo)
        if (Time.timeScale != 0f && // verificando se estamos com o game pausado
            (keyboard.spaceKey.wasPressedThisFrame || Mouse.current?.leftButton.wasPressedThisFrame == true)
        )
        {
            GetComponent<AudioSource>().Play();
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

    void Move(Vector2 direction, float currentSpeed)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x -= 0.225f;
        min.x += 0.225f;
        max.y -= 0.285f;
        min.y += 0.285f;

        Vector2 pos = transform.position;
        pos += direction * currentSpeed * Time.deltaTime;

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
            lives--;
            if (lives >= 0 && lives < healthDots.Length)
            {
                healthDots[lives].SetActive(false);
            }

            if (lives == 0)
            {
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                gameObject.SetActive(false);
            }
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate (ExplosionGO);

        explosion.transform.position = transform.position;
    }
}
