using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _cameraTopViewGameObject;
    [SerializeField] private GameObject _cameraFrontViewGameObject;
    [SerializeField] private GameObject _cameraTopViewPrefab;
    [SerializeField] private GameObject _cameraFrontViewPrefab;
    private Camera [] _cameras;

    private void Awake()
    {
        ClearCameras();
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraTopViewGameObject = Instantiate(_cameraTopViewPrefab);
        Debug.Log($"instanciada Top");
        _cameraFrontViewGameObject = Instantiate(_cameraFrontViewPrefab);
        Debug.Log($"instanciada Front");

        if (GameSettings.Instance.IsTopViewModeActive && !GameSettings.Instance.IsFrontViewModeActive)
        {
            ActivateTopView();

        }
        else
        {
            GameSettings.Instance.IsTopViewModeActive = false;
            ActivateFrontView();
        }
        
        
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

    private void ActivateFrontView()
    {
        
        _cameraFrontViewGameObject.SetActive(true);
        if (_cameraTopViewGameObject != null)
        {
            _cameraTopViewGameObject.SetActive(false);
        }
        //Change canvas' orientation for this camera
    }

    private void ActivateTopView()
    {
        _cameraTopViewGameObject.SetActive(true);
        if (_cameraTopViewGameObject != null)
        {
            _cameraFrontViewGameObject.SetActive(false);
        }


        //Change canvas' orientation for this camera
    }
}
