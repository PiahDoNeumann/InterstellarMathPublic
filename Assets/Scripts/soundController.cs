using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class soundController : MonoBehaviour
{
    public AudioSource src, musicaFundo;
    public AudioClip botaoPass, botaoClick, acerto, erro, abdSoltar, abdPegar;

    public Slider musicaSlider, sonsSlider;

    public void Start()
    {

        // Carregar os valores salvos para os volumes
        if (musicaFundo != null)
        {
            float musicaVolume = PlayerPrefs.GetFloat("MusicaVolume", 0.1f); // Valor padrão é 0.5
            musicaFundo.volume = musicaVolume;

            float musicaTime = PlayerPrefs.GetFloat("MusicaTime", 0f);
            musicaFundo.time = musicaTime;
            
            if (musicaSlider != null)
            {
                musicaSlider.value = musicaVolume;
                musicaSlider.onValueChanged.AddListener(UpdateMusicaVolume);
            }
        }

        if (src != null)
        {
            float sonsVolume = PlayerPrefs.GetFloat("SonsVolume", 0.1f); // Valor padrão é 0.5
            src.volume = sonsVolume;

            if (sonsSlider != null)
            {
                sonsSlider.value = sonsVolume;
                sonsSlider.onValueChanged.AddListener(UpdateSonsVolume);
            }
        }
    }

    private void UpdateMusicaVolume(float value)
    {
        if (musicaFundo != null)
        {
            musicaFundo.volume = value;
            PlayerPrefs.SetFloat("MusicaVolume", value);
        }
    }

    private void UpdateSonsVolume(float value)
    {
        if (src != null)
        {
            src.volume = value;
            PlayerPrefs.SetFloat("SonsVolume", value);
        }
    }

    public void playBotao(string nameSound)
    {
        switch (nameSound)
        {
            case "Pass":
                src.PlayOneShot(botaoPass);
                break;
            case "Click":
                src.PlayOneShot(botaoClick);
                break;
            case "Acerto":
                src.PlayOneShot(acerto);
                break;
            case "Erro":
                src.PlayOneShot(erro);
                break;
            case "AbdSoltar":
                src.PlayOneShot(abdSoltar);
                break;
            case "AbdPegar":
                src.PlayOneShot(abdPegar);
                break;
        }
    }
}
