using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider sliderVolume;

    void Start()
    {
        // Carrega volume salvo (ou define padrão 1)
        sliderVolume.value = PlayerPrefs.GetFloat("volumeJogo", 1f);

        // Atualiza o AudioListener
        AudioListener.volume = sliderVolume.value;

        // Detecta mudanças no slider
        sliderVolume.onValueChanged.AddListener(AlterarVolume);
    }

    public void AlterarVolume(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("volumeJogo", valor);
    }
}//olá
