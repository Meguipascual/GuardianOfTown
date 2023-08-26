using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _camerasGameObject;
    [SerializeField] private GameObject[] _camerasTopViewPrefab;
    [SerializeField] private GameObject[] _camerasFrontViewPrefab;
    private Camera [] _camerasToDestroy;
    private bool _isTopViewActive;

    private void Awake()
    {
        _camerasGameObject = null;
        ClearCameras();
        if (GameSettings.Instance.IsTopViewModeActive)
        {
            _isTopViewActive = true;
            _camerasGameObject = new GameObject[_camerasTopViewPrefab.Length];
        }
        else
        {
            _isTopViewActive = false;
            _camerasGameObject = new GameObject[_camerasFrontViewPrefab.Length];
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_isTopViewActive)
        {
            for (int i = 0; i < _camerasTopViewPrefab.Length; i++)
            {

                var topGameobject = Instantiate(_camerasTopViewPrefab[i]);
                _camerasGameObject[i] = topGameobject;
                Debug.Log($"instanciada Top {i}");
            }
        }
        else
        {
            for (int i = 0; i < _camerasFrontViewPrefab.Length; i++)
            {
                var frontGameobject = Instantiate(_camerasFrontViewPrefab[i]);
                _camerasGameObject[i] = frontGameobject;
                Debug.Log($"instanciada Front {i}");
            }
            GameSettings.Instance.IsTopViewModeActive = false;
        }
        ActivateCamera(0);
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

    public void ActivateCamera(int selectedCamera)
    {
        if (_camerasGameObject != null)
        {
            for (int i=0;i< _camerasGameObject.Length; i++)
            {
                _camerasGameObject[i].SetActive(false);
            }
        }
        _camerasGameObject[selectedCamera].SetActive(true);
        
        //Change canvas' orientation for this camera
    }
}
