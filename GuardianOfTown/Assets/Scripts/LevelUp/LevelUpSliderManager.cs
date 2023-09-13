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
    private bool _isLevelingUp;
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
        if(gameObject.activeInHierarchy == true && !_isLevelingUp)
        {
            WriteInitialText();
            StartCoroutine(LevelUp());
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
        var spentExp = 0;
        StopAllCoroutines();

        while (_playerController.Exp - spentExp > 20 * _currentLevel)
        {
            spentExp += 20 * _currentLevel;
            _playerController.LevelPoints += 2;
            _currentLevel++;
        }

        _slider.value = 0;
        _currentLevel--;
        _playerController.Level = _currentLevel;
        _playerController.Exp -= spentExp;
        _playerLevelText.text = $"Level: {_currentLevel}";
        _playerObtainedExpText.text = $"Exp: {_playerController.Exp}";
        _skipButton.gameObject.SetActive(false);
        _continueButton.gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        Debug.Log($"it's continuing, look how fast I continue");
        _LevelUpPanel.gameObject.SetActive(false);
        _levelPointsAssignPanel.SetActive(true);
}

    IEnumerator LevelUp()
    {
        var exp = _playerController.Exp;

        while (_playerController.Exp > 20 * _currentLevel)
        {
            _slider.value = 0;
            _currentLevel ++;
            _slider.maxValue = 20 * _currentLevel;

            do
            {
                exp --;
                _playerObtainedExpText.text = $"Exp: {exp}";
                _slider.value ++;
                yield return new WaitForSeconds(1/_slider.maxValue);//modificate time in order to be shorter when bigger the max value
            }
            while (_slider.value < _slider.maxValue);

            _playerController.LevelPoints += 2;
            _slider.value = 0;
            _playerController.Level = _currentLevel;
            _playerLevelText.text = $"Level: {_currentLevel}";
            _playerController.Exp -= (int) _slider.maxValue;
            _playerObtainedExpText.text = $"Exp: {_playerController.Exp}";
        }
        _skipButton.gameObject.SetActive(false);
        _continueButton.gameObject.SetActive(true);
    }
}
