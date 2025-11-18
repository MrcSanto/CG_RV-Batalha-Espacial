using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        float musicVol;
        float sfxVol;

        // Carrega valores salvos (opcional)
        if (audioMixer.GetFloat("MusicVolume", out musicVol))
            musicSlider.value = Mathf.Pow(10, musicVol / 20);

        if (audioMixer.GetFloat("SFXVolume", out sfxVol))
            sfxSlider.value = Mathf.Pow(10, sfxVol / 20);
    }

    public void OnMusicVolumeChange()
    {
        // Converte 0–1 para decibéis
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
    }

    public void OnSFXVolumeChange()
    {
        // Converte 0–1 para decibéis
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
    }
}
