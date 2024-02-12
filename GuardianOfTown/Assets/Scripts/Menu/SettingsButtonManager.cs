using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _optionChangedAlertText;
    [SerializeField] private int _messageDuration = 2;
    [SerializeField] private Image _easySelectedImage;
    [SerializeField] private Image _normalSelectedImage;
    [SerializeField] private Image _hardSelectedImage;
    private GameSettings _gameSettings;
    private int _savedGameMode;

    private void Start()
    {
        _gameSettings = GameSettings.Instance;
        if (PlayerPrefs.HasKey("Gamemode"))
        {
            _savedGameMode = PlayerPrefs.GetInt("Gamemode");

            if(_savedGameMode == 1) { ActivateEasyMode(); }
            else if(_savedGameMode == 2){ ActivateNormalMode(); }
            else { ActivateHardMode(); }
        }
        else
        {
            _savedGameMode = 2;
            ActivateNormalMode();
        }
    }

    public void ActivateEasyMode()
    {
        _gameSettings.IsEasyModeActive = true;
        _gameSettings.IsNormalModeActive = false;
        _gameSettings.IsHardModeActive = false;
        _easySelectedImage.gameObject.SetActive(true);
        _normalSelectedImage.gameObject.SetActive(false);
        _hardSelectedImage.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Gamemode", 1);
        _savedGameMode = 1;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Easy Gamemode (Not Rank)"));
    }
   
    public void ActivateNormalMode()
    {
        _gameSettings.IsEasyModeActive = false;
        _gameSettings.IsNormalModeActive = true;
        _gameSettings.IsHardModeActive = false;
        _easySelectedImage.gameObject.SetActive(false);
        _normalSelectedImage.gameObject.SetActive(true);
        _hardSelectedImage.gameObject.SetActive(false);
        PlayerPrefs.SetInt("Gamemode", 2);
        _savedGameMode = 2;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Normal Gamemode"));
    }

    public void ActivateHardMode()
    {
        _gameSettings.IsEasyModeActive = false;
        _gameSettings.IsNormalModeActive = false;
        _gameSettings.IsHardModeActive = true;
        _easySelectedImage.gameObject.SetActive(false);
        _normalSelectedImage.gameObject.SetActive(false);
        _hardSelectedImage.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Gamemode", 3);
        _savedGameMode = 3;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Hard Gamemode Score x2"));
    }

    public void ActivateFront()
    {
        _gameSettings.IsTopViewModeActive = false;
        _gameSettings.IsFrontViewModeActive = true;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Front viewpoint activated"));
    }

    public void ActivateTop()
    {
        _gameSettings.IsTopViewModeActive = true;
        _gameSettings.IsFrontViewModeActive = false;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Top viewpoint activated"));
    }

    public void ToggleDeveloperMode()
    {
        //GameSettings.Instance.IsDeveloperModeActive = !GameSettings.Instance.IsDeveloperModeActive;
        //GameSettings.Instance.IsEasyModeActive = false;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Dev Gamemode activated" +
            $"(not developed yet)"));
    }

    IEnumerator ShowOptionChangeMessage(string message)
    {
        _optionChangedAlertText.text = message;
        _optionChangedAlertText.gameObject.SetActive(true);
        yield return new WaitForSeconds (_messageDuration);
        _optionChangedAlertText.gameObject.SetActive(false);
    }
}
