using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChangeGateManager : MonoBehaviour
{  
    public static ChangeGateManager instance;
    private CameraManager _cameraManager;
    private PlayerController _playerController;
    public GameObject _gateOrientationPanel;
    [SerializeField] private Image[] _gateCompassImages;
    [SerializeField] public Image[] _gateWarningImages;
    [SerializeField] private Light[] _gateDirectionalLights;
    [SerializeField] private string[] _gateText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;    
        _cameraManager = FindObjectOfType<CameraManager>();
        _playerController = FindObjectOfType<PlayerController>();

        for (int i = 0; i < _gateCompassImages.Length; i++)
        {
            _gateCompassImages[i].gameObject.SetActive(false);
            _gateDirectionalLights[i].gameObject.SetActive(false);
            _gateWarningImages[i].gameObject.SetActive(false);
        }

        _gateDirectionalLights[0].gameObject.SetActive(true);
        _gateCompassImages[0].gameObject.SetActive(true);
        GameManager.Instance._gateText.text = _gateText[0];
        RenderSettings.fog = true;
    }

    public void RightInputButton(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Started)
        {
            return;
        }
        RightButtonClicked();
    }

    public void LeftInputButton(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
        {
            return;
        }
        LeftButtonClicked();
    }

    public void RightButtonClicked()
    {
        if (_playerController.IsDead || GameManager.Instance.IsGamePaused || GameManager.Instance.IsCountDownActive)
        {
            return;
        }

        var cameraIndex = _cameraManager.ActiveCameraIndex;
        var newCameraIndex = cameraIndex + 1;
        var playerY = _playerController.transform.position.y;
        var playerZ = _playerController.transform.position.z;
        var playerX = _playerController.transform.position.x - Mathf.Abs(DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex]);

        if (cameraIndex < _cameraManager.CamerasGameObject.Length - 1)
        { 
            _cameraManager.ActivateCamera(newCameraIndex);
            _gateCompassImages[newCameraIndex].gameObject.SetActive(true);
            GameManager.Instance._gateText.text = _gateText[newCameraIndex];
            _gateDirectionalLights[newCameraIndex].gameObject.SetActive(true);
            GameManager.Instance.MainCamera = _cameraManager.CamerasGameObject[newCameraIndex].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[newCameraIndex];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex], playerY, playerZ);

            if(cameraIndex == 0)
            {
                _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex] + 46, playerY, playerZ);
            }
            if(newCameraIndex == 3 || DataPersistantManager.Instance.Stage >= GameManager.Instance.StageToActivateRedFog)
            {
                RenderSettings.fog = true;
            }
            else
            {
                RenderSettings.fog = false;
            }
        }
        else
        {
            _cameraManager.ActivateCamera(0);
            _gateCompassImages[0].gameObject.SetActive(true);
            GameManager.Instance._gateText.text = _gateText[0];
            _gateDirectionalLights[0].gameObject.SetActive(true);
            GameManager.Instance.MainCamera = _cameraManager.CamerasGameObject[0].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[0], playerY, playerZ);
            RenderSettings.fog = true;
        }
        _cameraManager.DeactivateCamera(cameraIndex);
        DeactivateDirectionalLight(cameraIndex);
        DeactivateCompassImage(cameraIndex);
    }

    public void LeftButtonClicked()
    {
        if (_playerController.IsDead || GameManager.Instance.IsGamePaused || GameManager.Instance.IsCountDownActive)
        {
            return;
        }
        var cameraIndex = _cameraManager.ActiveCameraIndex;
        var newCameraIndex = cameraIndex - 1;
        var playerY = _playerController.transform.position.y;
        var playerZ = _playerController.transform.position.z;
        var playerX = _playerController.transform.position.x - Mathf.Abs(DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex]);

        if (cameraIndex == 0)
        {
            _cameraManager.ActivateCamera(3);
            _gateCompassImages[3].gameObject.SetActive(true);
            GameManager.Instance._gateText.text = _gateText[3];
            _gateDirectionalLights[3].gameObject.SetActive(true);
            GameManager.Instance.MainCamera = _cameraManager.CamerasGameObject[3].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[3];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[3];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[3] + 46, playerY, playerZ);
            RenderSettings.fog = true;
        }
        else
        {
            _cameraManager.ActivateCamera(newCameraIndex);
            _gateCompassImages[newCameraIndex].gameObject.SetActive(true);
            GameManager.Instance._gateText.text = _gateText[newCameraIndex];
            _gateDirectionalLights[newCameraIndex].gameObject.SetActive(true);
            GameManager.Instance.MainCamera = _cameraManager.CamerasGameObject[newCameraIndex].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[newCameraIndex];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex], playerY, playerZ);
           
            if (newCameraIndex == 0 || DataPersistantManager.Instance.Stage >= GameManager.Instance.StageToActivateRedFog) 
            { 
                RenderSettings.fog = true; 
            }
            else 
            {  
                RenderSettings.fog = false; 
            }
        }
        _cameraManager.DeactivateCamera(cameraIndex);
        DeactivateCompassImage(cameraIndex);
        DeactivateDirectionalLight(cameraIndex);
    }

    public void ActivateWarningImage(int imageIndex)
    {
        _gateWarningImages[imageIndex].gameObject.SetActive(true);
    }
    
    public void ActivateAllWarningImages()
    {
        for (int i = 0; i < _gateWarningImages.Length; i++)
        {
            _gateWarningImages[i].gameObject.SetActive(true);
        }
    }

    private void DeactivateCompassImage(int imageIndex)
    {
        _gateCompassImages[imageIndex].gameObject.SetActive(false);
    }

    private void DeactivateDirectionalLight(int lightIndex)
    {
        _gateDirectionalLights[lightIndex].gameObject.SetActive(false);
    }

    public void DeactivateWarningImage(int imageIndex)
    {
        _gateWarningImages[imageIndex].gameObject.SetActive(false);
    }

    public void DeactivateAllWarningImages()
    {
        for (int i=0; i< _gateWarningImages.Length; i++)
        {
            _gateWarningImages[i].gameObject.SetActive(false);
        }
    }
}
