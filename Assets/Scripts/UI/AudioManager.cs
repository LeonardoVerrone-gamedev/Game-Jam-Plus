using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer masterMixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

 
    [Header("Parâmetros do Mixer (Strings)")]
    public string musicParameter = "MusicVolume";
    
    public string sfxParameter = "SFXVolume";

    private const float MIN_VOLUME = -80f; // Volume mínimo (Mudo -80 dB)
    private const float MAX_VOLUME = 0f;    // Volume máximo (Volume original 0 dB)
    
    void OnEnable()
    {
        SetInitialVolume();
    }

    // Define o valor inicial dos sliders baseado nos parâmetros atuais do Audio Mixer
    private void SetInitialVolume()
    {
        if (musicSlider != null)
        {
            float musicVol = GetNormalizedVolume(musicParameter);
            // Atualiza o valor do Slider sem disparar o evento OnValueChanged
            musicSlider.SetValueWithoutNotify(musicVol); 
            // Garante que o Mixer seja atualizado com o valor correto
            SetMusicVolume(musicVol); 
        }

        if (sfxSlider != null)
        {
            float sfxVol = GetNormalizedVolume(sfxParameter);
            sfxSlider.SetValueWithoutNotify(sfxVol);
            SetSFXVolume(sfxVol); // Garante que o Mixer seja atualizado
        }
    }
    

    public void SetMusicVolume(float normalizedVolume)
    {
        // 1. Converte o valor normalizado (0 a 1) do slider para a escala logarítmica (dB)
        float volumeDB;
        
        if (normalizedVolume <= 0.0001f)
        {
            // Se o slider estiver em zero, defina o volume como Mínimo (Mudo)
            volumeDB = MIN_VOLUME;
        }
        else
        {
            volumeDB = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, normalizedVolume);
        }

        masterMixer.SetFloat(musicParameter, volumeDB);
    }

    public void SetSFXVolume(float normalizedVolume)
    {
        float volumeDB;

        if (normalizedVolume <= 0.0001f)
        {
            volumeDB = MIN_VOLUME;
        }
        else
        {
            volumeDB = Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, normalizedVolume);
        }
        
        masterMixer.SetFloat(sfxParameter, volumeDB);
    }
    
    public float GetNormalizedVolume(string parameterName)
    {
        float volumeDB;
        if (masterMixer.GetFloat(parameterName, out volumeDB))
        {
            // Se o volume estiver no mínimo ou abaixo, retorna 0 (mudo)
            if (volumeDB <= MIN_VOLUME) return 0f;
            
            // Reverte o mapeamento de DB para 0-1 (escala do Slider)
            return (volumeDB - MIN_VOLUME) / (MAX_VOLUME - MIN_VOLUME);
        }
        // Se falhar ao obter o parâmetro, retorna o valor máximo (1) como fallback
        return 1f; 
    }
}