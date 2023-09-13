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
    public Button _hpButton;
    public Button _attackButton;
    public Button _defenseButton;
    public Button _critRateButton; 
    public Button _critDamageButton;
    public Button _speedButton;
    public Button _nextLevelButton;
    private int _increment;
    private float _critDamageIncrement;
    private int _levelPoints;
    private int _currentLevelPoints;
    private float[] _increments;
    private float[] _playerStatsCopy;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _increment = 5;
        _critDamageIncrement = 0.1f;
        _currentLevelPoints = 0;
        _levelPoints = _playerController.LevelPoints;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints}";
        _playerLevelText.text = $"Level: {_playerController.Level}";
        _increments = new float[6];
        _playerStatsCopy = new float[6];
        CopyPlayerStats();
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
        if (_levelPoints.Equals(_currentLevelPoints))
        {
            _hpButton.enabled = false;
            _attackButton.enabled = false;
            _defenseButton.enabled = false;
            _critRateButton.enabled = false;
            _critDamageButton.enabled = false;
            _speedButton.enabled = false;
        }
    }

    public void Undo()
    {
        if(!_hpButton.enabled)
        {
            _hpButton.enabled = true;
            _attackButton.enabled = true;
            _defenseButton.enabled = true;
            _critRateButton.enabled = true;
            _critDamageButton.enabled = true;
            _speedButton.enabled = true;
        }
        _increments = new float[6];
        CopyPlayerStats();
        _currentLevelPoints = 0;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
    }

    public void increaseHP()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[0] += _increment;
        TryToDisableButtons();
    }
    public void increaseAttack()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[1] += _increment;
        TryToDisableButtons();
    }
    public void increaseDefense()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[2] += _increment;
        TryToDisableButtons();
    }
    public void increaseCritRate()
    {
        _playerController.CriticalRate += _increment; 
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[3] += _increment;
        TryToDisableButtons();
    }
    public void increaseCritDamage()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[4] += _critDamageIncrement;
        TryToDisableButtons();
    }
    public void increaseSpeed()
    {
        _currentLevelPoints++;
        _levelPointsText.text = $"LevelUp Points: {_levelPoints - _currentLevelPoints}";
        _increments[5] += _increment;
        TryToDisableButtons();
    }
    public void NextStage()
    {
        ConfirmIncrements();
        for(int i=0; i< _increments.Length; i++)
        {
            Debug.Log($"Stat {i}: {_increments[i]}");
        }
        DataPersistantManager.Instance.ReloadScene();
    }

    private void ConfirmIncrements()
    {
        _playerController.HpMax += (int)_increments[0];
        _playerController.Attack += (int)_increments[1];
        _playerController.Defense += (int)_increments[2];
        _playerController.CriticalRate += (int)_increments[3];
        _playerController.CriticalDamage += _increments[4];
        _playerController.Speed += _increments[5];
        _playerController.LevelPoints -= _currentLevelPoints;
    }
}
