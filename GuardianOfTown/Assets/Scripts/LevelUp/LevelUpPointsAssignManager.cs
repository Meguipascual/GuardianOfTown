using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPointsAssignManager : MonoBehaviour
{
    private PlayerController _playerController;
    public TextMeshProUGUI _levelPointsText;
    public TextMeshProUGUI _playerLevelText;
    public TextMeshProUGUI _playerHPText;
    public TextMeshProUGUI _playerAttackText;
    public TextMeshProUGUI _playerDefenseText;
    public TextMeshProUGUI _playerCritRateText;
    public TextMeshProUGUI _playerCritDamageText;
    public TextMeshProUGUI _playerSpeedText;
    public Button _hpButton;
    public Button _attackButton;
    public Button _defenseButton;
    public Button _critRateButton; 
    public Button _critDamageButton;
    public Button _speedButton;
    public Button _nextLevelButton;
    private int _increment;
    private int _critDamageIncrement;
    private int _levelPoints;
    private int _currentLevelPoints;
    private float[] _increments;
    private float[] _playerStatsCopy;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _increments = new float[6];
        _playerStatsCopy = new float[6];
        _increment = 5;
        _critDamageIncrement = 10;
        _currentLevelPoints = 0; 
        _levelPoints = _playerController.LevelPoints;
        CopyPlayerStats();
        _levelPointsText.text = $"LevelUp Points: {_levelPoints}";
        _playerLevelText.text = $"Level: {_playerController.Level}";
        _playerHPText.text = $"HP Max: {_playerStatsCopy[0]}";
        _playerAttackText.text = $"Attack: {_playerStatsCopy[1]}";
        _playerDefenseText.text = $"Defense: {_playerStatsCopy[2]}";
        _playerCritRateText.text = $"Critical Rate: {_playerStatsCopy[3]}";
        _playerCritDamageText.text = $"Critical Damage: {_playerStatsCopy[4]}";
        _playerSpeedText.text = $"Speed: {_playerStatsCopy[5]}";
        TryToDisableButtons();
    }

    private void CopyPlayerStats()
    {
        _playerStatsCopy[0] = _playerController.HpMax;
        _playerStatsCopy[1] = _playerController.Attack;
        _playerStatsCopy[2] = _playerController.Defense;
        _playerStatsCopy[3] = _playerController.CriticalRate;
        _playerStatsCopy[4] = _playerController.CriticalDamage;
        _playerStatsCopy[5] = _playerController.Speed;
    }

    private void TryToDisableButtons()
    {
        if (_levelPoints <=_currentLevelPoints)
        {
            _hpButton.interactable = false;
            _hpButton.interactable = false;
            _attackButton.interactable = false;
            _defenseButton.interactable = false;
            _critRateButton.interactable = false;
            _critDamageButton.interactable = false;
            _speedButton.interactable = false;
        }
    }

    public void Undo()
    {
        _increments = new float[6];
        CopyPlayerStats();
        _currentLevelPoints = 0;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints}";
        _playerHPText.text = $"HP Max: {_playerStatsCopy[0]}";
        _playerAttackText.text = $"Attack: {_playerStatsCopy[1]}";
        _playerDefenseText.text = $"Defense: {_playerStatsCopy[2]}";
        _playerCritRateText.text = $"Critical Rate: {_playerStatsCopy[3]}";
        _playerCritDamageText.text = $"Critical Damage: {_playerStatsCopy[4]}";
        _playerSpeedText.text = $"Speed: {_playerStatsCopy[5]}";
        if (!_hpButton.interactable && (_levelPoints>0))
        {
            _hpButton.interactable = true;
            _attackButton.interactable = true;
            _defenseButton.interactable = true;
            _critRateButton.interactable = true;
            _critDamageButton.interactable = true;
            _speedButton.interactable = true;
        }
    }

    public void increaseHP()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[0] += _increment;
        _playerHPText.text = $"HP Max: {_playerStatsCopy[0]}+{_increments[0]}";
        TryToDisableButtons();
    }
    public void increaseAttack()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[1] += _increment;
        _playerAttackText.text = $"Attack: {_playerStatsCopy[1]}+{_increments[1]}";
        TryToDisableButtons();
    }
    public void increaseDefense()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[2] += _increment;
        _playerDefenseText.text = $"Defense: {_playerStatsCopy[2]}+{_increments[2]}";
        TryToDisableButtons();
    }
    public void increaseCritRate()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[3] += _increment;
        _playerCritRateText.text = $"Critical Rate: {_playerStatsCopy[3]}+{_increments[3]}";
        TryToDisableButtons();
    }
    public void increaseCritDamage()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[4] += _critDamageIncrement;
        _playerCritDamageText.text = $"Critical Damage: {_playerStatsCopy[4]}+{_increments[4]}";
        TryToDisableButtons();
    }
    public void increaseSpeed()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[5] += _increment;
        _playerSpeedText.text = $"Speed: {_playerStatsCopy[5]}+{_increments[5]}";
        TryToDisableButtons();
    }
    public void NextStage()
    {
        ConfirmIncrements();
        GameManager.Instance.IsGamePaused = false;
        DataPersistantManager.Instance.ReloadScene();
    }

    private void ConfirmIncrements()
    {
        _playerController.HpMax += (int)_increments[0];
        _playerController.Attack += (int)_increments[1];
        _playerController.Defense += (int)_increments[2];
        _playerController.CriticalRate += (int)_increments[3];
        _playerController.CriticalDamage += (int)_increments[4];
        _playerController.Speed += _increments[5];
        _playerController.LevelPoints -= _currentLevelPoints;
    }
}
