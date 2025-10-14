using UnityEngine;
using UnityEngine.InputSystem; // Novo sistema de input

public class PlayerControl : MonoBehaviour
{
    public GameObject PlayerBulletGO;
    public GameObject BulletPosition01;
    public GameObject BulletPosition02;

    public float speed = 5f; // Velocidade do player

    private Vector2 movement; // Direção atual de movimento

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        float x = 0f;
        float y = 0f;

        // Leitura de teclas (novo Input System)
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1f;

        movement = new Vector2(x, y).normalized;

        // Detecta se a tecla Space foi pressionada neste frame
        if (keyboard.spaceKey.wasPressedThisFrame || mouse.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }

        Move(movement);
    }

    void Shoot()
    {
        GameObject bullet1 = Instantiate(PlayerBulletGO);
        bullet1.transform.position = BulletPosition01.transform.position;

        GameObject bullet2 = Instantiate(PlayerBulletGO);
        bullet2.transform.position = BulletPosition02.transform.position;
    }

    void Move(Vector2 direction)
    {
        // Limites da tela em coordenadas de mundo
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Ajuste para o tamanho do sprite (margens)
        max.x -= 0.225f;
        min.x += 0.225f;
        max.y -= 0.285f;
        min.y += 0.285f;

        // Movimento
        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;

        // Impede de sair da tela
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }
}
