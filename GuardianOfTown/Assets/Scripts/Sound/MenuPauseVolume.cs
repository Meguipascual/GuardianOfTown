using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuPauseVolume : MonoBehaviour
{
    public static MenuPauseVolume Instance;
    [SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _masterVolumeText;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private TextMeshProUGUI _sfxVolumeText;

    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
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

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetAllVolume()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        float volume = _masterSlider.value;
        int volumeInt = (int) (volume * 100);
        _masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        _masterVolumeText.text = volumeInt.ToString();
    }

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        int volumeInt = (int)(volume * 100);
        _masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        _musicVolumeText.text = volumeInt.ToString();
    }

    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        int volumeInt = (int)(volume * 100);
        _masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        _sfxVolumeText.text = volumeInt.ToString();
    }

    private void LoadVolume()
    {
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        SetAllVolume();
    }
}
