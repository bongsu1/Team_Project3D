using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider bgmVolume;
    [SerializeField] Slider sfxVolume;

    private void OnEnable()
    {
        audioMixer.GetFloat("Master", out float m_Volume);
        audioMixer.GetFloat("BGM", out float b_Volume);
        audioMixer.GetFloat("SFX", out float s_Volume);
        masterVolume.value = m_Volume;
        bgmVolume.value = b_Volume;
        sfxVolume.value = s_Volume;
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", value);
    }
    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM", value);
    }
    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", value);
    }
}
