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
    private bool _isLevelingUp;

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

    IEnumerator LevelUp()
    {
        while (_playerController.Exp > 20 * _currentLevel)
        {
            _slider.value = 0;
            _currentLevel ++;
            _slider.maxValue = 20 * _currentLevel;
            Debug.Log($"Max value: {_slider.maxValue}");

            do
            {
                _playerController.Exp --;
                _playerObtainedExpText.text = $"Exp: {_playerController.Exp}";
                _slider.value ++;
                yield return new WaitForSeconds(0.05f);//modificate time in order to be shorter when bigger the max value
            }
            while (_slider.value < _slider.maxValue);

            Debug.Log($"Current level: {_currentLevel}");
            _playerLevelText.text = $"Level: {_currentLevel}";
            _playerController.Exp -= (int) _slider.maxValue;
        }
    }
}
