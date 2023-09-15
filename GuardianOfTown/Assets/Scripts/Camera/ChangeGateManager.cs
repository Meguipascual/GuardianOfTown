using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGateManager : MonoBehaviour
{  
    private CameraManager _cameraManager;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void RightButtonClicked()
    {
        var cameraIndex = _cameraManager.ActiveCameraIndex;
        var newCameraIndex = cameraIndex + 1;
        var playerY = _playerController.transform.position.y;
        var playerZ = _playerController.transform.position.z;
        var playerX = _playerController.transform.position.x - Mathf.Abs(DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex]);

        if (cameraIndex < _cameraManager.CamerasGameObject.Length - 1)
        { 
            _cameraManager.ActivateCamera(cameraIndex + 1);
            GameManager.SharedInstance.MainCamera = _cameraManager.CamerasGameObject[cameraIndex + 1].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[newCameraIndex];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex], playerY, playerZ);

            if(cameraIndex == 0)
            {
                _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex] + 46, playerY, playerZ);
            }
        }
        else
        {
            _cameraManager.ActivateCamera(0);
            GameManager.SharedInstance.MainCamera = _cameraManager.CamerasGameObject[0].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[0], playerY, playerZ);
        }
        _cameraManager.DeactivateCamera(cameraIndex);
    }

    public void LeftButtonClicked()
    {
        var cameraIndex = _cameraManager.ActiveCameraIndex;
        var newCameraIndex = cameraIndex - 1;
        var playerY = _playerController.transform.position.y;
        var playerZ = _playerController.transform.position.z;
        var playerX = _playerController.transform.position.x - Mathf.Abs(DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex]);

        if (cameraIndex == 0)
        {
            _cameraManager.ActivateCamera(3);
            GameManager.SharedInstance.MainCamera = _cameraManager.CamerasGameObject[3].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[3];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[3];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[3] + 46, playerY, playerZ);
        }
        else
        {
            _cameraManager.ActivateCamera(cameraIndex - 1);
            GameManager.SharedInstance.MainCamera = _cameraManager.CamerasGameObject[cameraIndex - 1].GetComponent<Camera>();
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[newCameraIndex];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex];
            _playerController.transform.position = new Vector3(playerX + DataPersistantManager.Instance.SpawnBoundariesLeft[newCameraIndex], playerY, playerZ);
        }

        _cameraManager.DeactivateCamera(cameraIndex);
    }
}
