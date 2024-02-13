using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuVolume : MonoBehaviour
{
    public static MenuVolume Instance;
    [SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _masterVolumeNumberText;
    [SerializeField] private TextMeshProUGUI _musicVolumeNumberText;
    [SerializeField] private TextMeshProUGUI _sfxVolumeNumberText;

    // Start is called before the first frame update

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetAllVolume();
        }
    }

    private void SetAllVolume()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        if(SceneManager.GetActiveScene().name == Tags.Prologue)
        {
            _masterMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
            return;
        }
        float volume = _masterSlider.value;
        int volumeInt = (int) (volume * 100);
        _masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        _masterVolumeNumberText.text = volumeInt.ToString();
    }

    public void SetMusicVolume()
    {
        if (SceneManager.GetActiveScene().name == Tags.Prologue)
        {
            _masterMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
            return;
        }
        float volume = _musicSlider.value;
        int volumeInt = (int)(volume * 100);
        _masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        _musicVolumeNumberText.text = volumeInt.ToString();
    }

    public void SetSFXVolume()
    {
        if (SceneManager.GetActiveScene().name == Tags.Prologue)
        {
            _masterMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
            return;
        }
        float volume = _sfxSlider.value;
        int volumeInt = (int)(volume * 100);
        _masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        _sfxVolumeNumberText.text = volumeInt.ToString();
    }

    private void LoadVolume()
    {
        if (SceneManager.GetActiveScene().name == Tags.Prologue)
        {

            SetAllVolume();
            return;
        }
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        SetAllVolume();
    }
}
