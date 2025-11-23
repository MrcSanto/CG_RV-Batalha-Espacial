using UnityEngine;
using UnityEngine.UI;

public class SpeedometerNeedleUI : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private PlayerControl player;      // nave
    [SerializeField] private RectTransform needle;      // RectTransform da agulha

    [Header("Configuração de Escala")]
    [SerializeField] private float minSpeed = 0f;
    [SerializeField] private float maxSpeed = 10f;

    // Ângulos em graus para o mínimo e máximo (ajuste conforme o seu layout)
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;

    private void Update()
    {
        if (player == null || needle == null) return;

        // pega a velocidade atual da nave
        float speed = Mathf.Clamp(player.CurrentSpeed, minSpeed, maxSpeed);

        // normaliza para 0..1
        float t = (speed - minSpeed) / (maxSpeed - minSpeed);

        // interpola entre minAngle e maxAngle
        float angle = Mathf.Lerp(450f, -270f, t);

        // aplica rotação no eixo Z
        Vector3 rotation = needle.localEulerAngles;
        rotation.z = angle;
        needle.localEulerAngles = rotation;
    }
}
