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
        var camera = _cameraManager.ActiveCameraIndex;
        if (camera < _cameraManager.CamerasGameObject.Length - 1)
        {
            _cameraManager.ActivateCamera(camera + 1);
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[camera + 1];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[camera + 1];
            _playerController.transform.position = new Vector3(
                _playerController.transform.position.x + DataPersistantManager.Instance.SpawnBoundariesLeft[camera + 1],
                _playerController.transform.position.y,
                _playerController.transform.position.z);
            
        }
        else
        {
            _cameraManager.ActivateCamera(0);
            _playerController.XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
            _playerController.XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
            _playerController.transform.position = new Vector3(
                _playerController.transform.position.x + DataPersistantManager.Instance.SpawnBoundariesLeft[0],
                _playerController.transform.position.y,
                _playerController.transform.position.z);
        }
        _cameraManager.DeactivateCamera(camera);
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
