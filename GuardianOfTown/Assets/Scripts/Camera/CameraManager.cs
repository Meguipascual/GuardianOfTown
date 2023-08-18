using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _camerasTopViewGameObject;
    [SerializeField] private GameObject[] _camerasFrontViewGameObject;
    [SerializeField] private GameObject[] _camerasTopViewPrefab;
    [SerializeField] private GameObject[] _camerasFrontViewPrefab;
    private Camera [] _camerasToDestroy;

    private void Awake()
    {
        ClearCameras();
        _camerasFrontViewGameObject = new GameObject[_camerasFrontViewPrefab.Length];
        _camerasTopViewGameObject = new GameObject[_camerasTopViewPrefab.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<_camerasFrontViewPrefab.Length;i++)
        {

            var topGameobject = Instantiate(_camerasTopViewPrefab[i]);
            _camerasTopViewGameObject[i] = topGameobject;
            Debug.Log($"instanciada Top");

            var frontGameobject = Instantiate(_camerasFrontViewPrefab[i]);
            _camerasFrontViewGameObject[i] = frontGameobject;
            Debug.Log($"instanciada Front");
        }
        
        if (GameSettings.Instance.IsTopViewModeActive && !GameSettings.Instance.IsFrontViewModeActive)
        {
            ActivateTopView(0);

        }
        else
        {
            GameSettings.Instance.IsTopViewModeActive = false;
            ActivateFrontView(0);
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

    private void ActivateFrontView(int selectedCamera)
    {
        if (_camerasTopViewGameObject != null)
        {
            for (int i=0;i< _camerasTopViewGameObject.Length; i++)
            {
                _camerasTopViewGameObject[i].SetActive(false);
            }
        }
        _camerasFrontViewGameObject[selectedCamera].SetActive(true);
        
        //Change canvas' orientation for this camera
    }

    private void ActivateTopView(int selectedCamera)
    {

        if (_camerasFrontViewGameObject != null)
        {
            for (int i = 0; i < _camerasTopViewGameObject.Length; i++)
            {
                _camerasFrontViewGameObject[i].SetActive(false);
            }
        }
        _camerasTopViewGameObject[selectedCamera].SetActive(true);

        //Change canvas' orientation for this camera
    }
}
