using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    [SerializeField] private bool _isEasyModeActive; //easy mode with autoshooting
    [SerializeField] private bool _isDeveloperModeActive; //activate buttons that can create any enemy, powerup or wave
    [SerializeField] private bool _isFrontViewModeActive; //activate the 3 person perspective camera and pannels
    [SerializeField] private bool _isTopViewModeActive; //activate top view
    private Camera[] _cameras;
    [SerializeField] private GameObject _cameraTopViewPrefab;
    [SerializeField] private GameObject _cameraFrontViewPrefab;
    [SerializeField] private GameObject _cameraTopViewGameObject;
    [SerializeField] private GameObject _cameraFrontViewGameObject;

    private void ComprobateInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Awake()
    {
        ComprobateInstance();
        DontDestroyOnLoad(gameObject);
    }

    private void ClearCameras()
    {
        _cameras = null;
        _cameras = FindObjectsOfType<Camera>();

        if (_cameras != null)
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                Destroy(_cameras[i].gameObject);
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1) 
        {
            ClearCameras();
            if (_isTopViewModeActive && !_isFrontViewModeActive)
            {
                _cameraTopViewGameObject = Instantiate(_cameraTopViewPrefab);
                Debug.Log($"instanciada Top");
                _isTopViewModeActive = true;
                _isFrontViewModeActive = false;
                ActivateTopView();
            }
            else
            {
                _cameraFrontViewGameObject = Instantiate(_cameraFrontViewPrefab);
                Debug.Log($"instanciada Front");
                _isTopViewModeActive = false;
                _isFrontViewModeActive = true;
                ActivateFrontView();
            }
        }
        else
        {
            Debug.Log($"You are in level {level}");
        }
        
    }

    private void ActivateFrontView()
    {
        _cameraFrontViewGameObject.SetActive(true);
        if(_cameraTopViewGameObject != null)
        {
            _cameraTopViewGameObject.SetActive(false);
        }
        //Change canvas' orientation for this camera
    }
    
    private void ActivateTopView()
    {
        _cameraTopViewGameObject.SetActive(true);
        if(_cameraTopViewGameObject != null)
        {
            _cameraFrontViewGameObject.SetActive(false);
        }
        
        
        //Change canvas' orientation for this camera
    }
}
