using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpSliderManager : MonoBehaviour
{
    public TextMeshProUGUI _playerObtainedExpText;
    public TextMeshProUGUI _playerLevelText;
    private PlayerController _playerController;
    [SerializeField] private Slider _slider;
    [SerializeField] private int _levelPoints;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _previousLevel;
    public GameObject _LevelUpPanel;
    public GameObject _levelPointsAssignPanel;
    public GameObject _powerUpsPanel;
    private bool _isLevelingUp;
    private Coroutine _slideLevelingUpCoroutine;
    public Button _skipButton;
    public Button _continueButton;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _slider = GetComponentInChildren<Slider>();
        _levelPoints = _playerController.LevelPoints;
        _previousLevel = _currentLevel = _playerController.Level;
        _slider.minValue = 0;
        _isLevelingUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy && !_isLevelingUp)
        {
            WriteInitialText();
            _slideLevelingUpCoroutine = StartCoroutine(LevelUp());
            _isLevelingUp = true;
        }   
    }


    private void WriteInitialText()
    {
        _playerObtainedExpText.text = $"Exp: {_playerController.Exp}";
        _playerLevelText.text = $"Level: {DataPersistantManager.Instance.SavedPlayerLevel}";
    }

    public void SkipButton()
    {
        StopCoroutine(_slideLevelingUpCoroutine);
        var spentExp = 0;

        while ((_playerController.Exp - spentExp) >= (20 * _currentLevel))
        {
            spentExp += (20 * _currentLevel);
            _playerController.LevelPoints += 2;
            _currentLevel++;
        }

        _slider.value = 0;
        _playerController.Level = _currentLevel;
        _playerController.Exp -= spentExp;
        _playerLevelText.text = $"Level: {_currentLevel}";
        _playerObtainedExpText.text = $"Exp: {_playerController.Exp}";
        _skipButton.gameObject.SetActive(false);
        _continueButton.gameObject.SetActive(true);
    }

    public void ContinueToLevelPointsButton()
    {
        ForcePowerUpsStop();
        _powerUpsPanel.SetActive(false);
        _levelPointsAssignPanel.SetActive(true);
    }

    public void ContinueToPowerUpsButton()
    {
        //if (DataPersistantManager.Instance.Stage % 2 == 0)
        //{
        //    _LevelUpPanel.gameObject.SetActive(false);
        //    _levelPointsAssignPanel.SetActive(true);
        //}
        //else
        //{
            _LevelUpPanel.gameObject.SetActive(false);
            _powerUpsPanel.SetActive(true);
            PermanentPowerUpManager.Instance.ControlNumberOfPowerUps();
        //}

    }

    IEnumerator LevelUp()
    {
        var exp = _playerController.Exp;

        while (_playerController.Exp > 20 * _currentLevel)
        {
            _slider.value = 0;
            _slider.maxValue = 20 * _currentLevel;

            while (_slider.value < _slider.maxValue)
            {
                exp --;
                _playerObtainedExpText.text = $"Exp: {exp}";
                _slider.value ++;
                yield return new WaitForSeconds(0.5f / (_slider.maxValue + 50));//modificate time in order to be shorter when bigger the max value 
            }

            _playerController.LevelPoints += 2;
            _slider.value = 0;
            _currentLevel++;
            _playerController.Level = _currentLevel;
            _playerLevelText.text = $"Level: {_currentLevel}";
            _playerController.Exp -= (int) _slider.maxValue;
            _playerObtainedExpText.text = $"Exp: {_playerController.Exp}";
        }
        _skipButton.gameObject.SetActive(false);
        _continueButton.gameObject.SetActive(true);
    }

    private void ForcePowerUpsStop() 
    {
        StopAllCoroutines();
        PowerUpSettings.Instance.IsContinuousShootInUse = false;//Forces powerUp finalization
        if (PowerUpSettings.Instance.IsSpeedIncreased)
        {
            PowerUpSettings.Instance.IsSpeedIncreased = false;
            _playerController.Speed = PowerUpSettings.Instance.PreviousPlayerSpeed;
            PowerUpSettings.Instance.SpeedAmount = 0;
        }
        
    }
}
