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

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _slider = GetComponentInChildren<Slider>();
        _levelPoints = _playerController.LevelPoints;
        _previousLevel = _currentLevel = _playerController.Level;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy == true)
        {
            WriteInitialText();
            LevelUp();
        }   
    }


    private void WriteInitialText()
    {
        _playerObtainedExpText.text = $"Exp: {_playerController.Exp}";
        _playerLevelText.text = $"Level: {DataPersistantManager.Instance.SavedPlayerLevel}";
    }

    private void LevelUp()
    {
        if (_playerController.Exp > 20 * _currentLevel)
        {

        }
    }
}
