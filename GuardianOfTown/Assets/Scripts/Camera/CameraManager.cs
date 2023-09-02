using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject[] CamerasGameObject { get; set; }
    public int ActiveCameraIndex {  get; set; }
    [SerializeField] private GameObject[] _camerasTopViewPrefab;
    [SerializeField] private GameObject[] _camerasFrontViewPrefab;
    private Camera [] _camerasToDestroy;
    private bool _isTopViewActive;

    private void Awake()
    {
        CamerasGameObject = null;
        ClearCameras();
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCamerasGameObject();
        SetUpCameras();
    }

    private void SetCamerasGameObject()
    {
        if (GameSettings.Instance.IsTopViewModeActive)
        {
            _isTopViewActive = true;
            CamerasGameObject = new GameObject[_camerasTopViewPrefab.Length];
            for (int i = 0; i < _camerasTopViewPrefab.Length; i++)
            {
                var topGameobject = Instantiate(_camerasTopViewPrefab[i]);
                CamerasGameObject[i] = topGameobject;
                Debug.Log($"instanciada Top {i}");
            }
        }
        else
        {
            _isTopViewActive = false;
            CamerasGameObject = new GameObject[_camerasFrontViewPrefab.Length];
            for (int i = 0; i < _camerasFrontViewPrefab.Length; i++)
            {
                var frontGameobject = Instantiate(_camerasFrontViewPrefab[i]);
                CamerasGameObject[i] = frontGameobject;
            }
            GameSettings.Instance.IsTopViewModeActive = false;
        }
    }

    private void ClearCameras()
    {
        _camerasToDestroy = null;
        _camerasToDestroy = FindObjectsOfType<Camera>();

        if (_camerasToDestroy != null)
        {
            for (int i = 0; i < _camerasToDestroy.Length; i++)
            {
                Destroy(_camerasToDestroy[i].gameObject);
            }
        }
    }


    //Activate and Deactivate the cameras for an initial set up
    private void SetUpCameras()
    {
        if (CamerasGameObject != null)
        {
            for (int i = 0; i < CamerasGameObject.Length; i++)
            {
                DeactivateCamera(i);
            }
            ActivateCamera(0);
        }
    }

    public void ActivateCamera(int selectedCamera)
    {
        CamerasGameObject[selectedCamera].SetActive(true);
        ActiveCameraIndex = selectedCamera;
        //Change canvas' orientation for this camera
    }
    public void DeactivateCamera(int selectedCamera)
    {
        CamerasGameObject[selectedCamera].SetActive(false);
    }
}
