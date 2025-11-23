using UnityEngine;
using TMPro;

/// <summary>
/// Mostra a velocidade atual do jogador na tela.
/// Arraste a nave (com PlayerControl) e o componente TMP_Text no Inspector.
/// </summary>
public class SpeedometerUI : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private float displayMultiplier = 1f;

    private void Update()
    {
        if (player == null || speedText == null) return;

        float speed = player.CurrentSpeed * displayMultiplier;
        // exibe com uma casa decimal
        speedText.text = $"Velocidade: {speed:0.0}";
    }
}
