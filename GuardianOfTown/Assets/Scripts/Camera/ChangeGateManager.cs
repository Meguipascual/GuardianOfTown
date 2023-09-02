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
        if (cameraIndex < _cameraManager.CamerasGameObject.Length - 1)
        { 
            _cameraManager.ActivateCamera(cameraIndex + 1);
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[cameraIndex + 1];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex + 1];
            _playerController.transform.position = new Vector3(
                (_playerController.transform.position.x - Mathf.Abs(DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex]))
                + DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex + 1],
                _playerController.transform.position.y,
                _playerController.transform.position.z);
            Debug.Log(_playerController.transform.position.x);
            if(cameraIndex == 0)
            {
                Debug.Log(_playerController.transform.position.x);
                _playerController.transform.position = new Vector3(_playerController.transform.position.x + 46, _playerController.transform.position.y, _playerController.transform.position.z);
            }
            
        }
        else
        {
            _cameraManager.ActivateCamera(0);
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
            _playerController.transform.position = new Vector3(
                (_playerController.transform.position.x - Mathf.Abs(DataPersistantManager.Instance.SpawnBoundariesLeft[cameraIndex]))
                + DataPersistantManager.Instance.SpawnBoundariesLeft[0],
                _playerController.transform.position.y,
                _playerController.transform.position.z);
        }
        _cameraManager.DeactivateCamera(cameraIndex);
        _playerController.gameObject.SetActive(false);
        _playerController.gameObject.SetActive(true);
    }

    public void LeftButtonClicked()
    {
        var camera = _cameraManager.ActiveCameraIndex;
        if (camera == 0)
        {
            _cameraManager.ActivateCamera(3);
        }
        else
        {
            _cameraManager.ActivateCamera(camera - 1);
        }
        _cameraManager.DeactivateCamera(camera);

    }
}
