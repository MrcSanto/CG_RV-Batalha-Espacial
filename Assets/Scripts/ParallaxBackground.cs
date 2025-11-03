using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private float manualHeight = 0f;

    private float backgroundImageHeight;

    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        backgroundImageHeight = sprite.texture.height / sprite.pixelsPerUnit;

        if (manualHeight > 0f)
            backgroundImageHeight = manualHeight;
    }

    void Update()
    {
        // Move o fundo no eixo Y
        float moveY = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(0, moveY);

        // Quando sair completamente da tela, reposiciona
        if (Mathf.Abs(transform.position.y) - backgroundImageHeight > 0)
        {
            transform.position = new Vector3(transform.position.x, 0f);
        }
    }
}
