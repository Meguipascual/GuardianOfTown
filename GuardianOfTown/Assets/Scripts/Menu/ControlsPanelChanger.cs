using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsPanelChanger : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _touchControlsPanel;
    [SerializeField] private GameObject _keyboardControlPanel;
    [SerializeField] private GameObject _gamePadControlPanel;

    [SerializeField] private Button _settingsPanelButton;
    [SerializeField] private Button _touchControlsPanelButton;
    [SerializeField] private Button _keyboardControlPanelButton;

    public void CloseControlPanel()
    {
        _keyboardControlPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        _gamePadControlPanel.SetActive(false);
        _touchControlsPanel.SetActive(false);
        _settingsPanelButton.Select();
    }

    public void OpenControlPanel()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            _keyboardControlPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _gamePadControlPanel.SetActive(false);
            _touchControlsPanel.SetActive(true);
            _touchControlsPanelButton.Select();
        }
        else
        {
            _keyboardControlPanel.SetActive(true);
            _touchControlsPanel.SetActive(false);
            _gamePadControlPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _keyboardControlPanelButton.Select();

        }
    }
}
