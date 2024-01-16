using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowerUpTimerSliderManager : MonoBehaviour
{
    public static PowerUpTimerSliderManager Instance;
    public List<GameObject> _powerUpSliderPrefabs;
    public List <GameObject> _activePowerUpSliders;
    [SerializeField] private GameObject _parentGameObject;
    private Vector3[] _powerUpOffset;
    private int _numberOfPowerUps;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _powerUpOffset = new Vector3[] { new Vector3(0, 0, 0), new Vector3(200, 0, 0) };
        _numberOfPowerUps = 2;
    }

    public GameObject InstantiatePowerUpSliderTimer(int Index)
    {
        var slider = Instantiate(_powerUpSliderPrefabs[Index], _parentGameObject.transform);
        if (_activePowerUpSliders.Count == 0) 
        {
            slider.transform.localPosition += _powerUpOffset[0];
            _activePowerUpSliders.Add(slider);
        }
        else
        {
            slider.transform.localPosition += _powerUpOffset[1];
            _activePowerUpSliders.Add(slider);
        }
        
        if (slider.GetComponentInChildren<Slider>() != null)
        {
            Debug.Log(slider);
            return slider;
        }
        else 
        {
            Debug.Log("SliderNull");
            return null; 
        }
    }

    public void RemoveSlider(GameObject Slider)
    {
        _activePowerUpSliders.Remove(Slider);
        if(_activePowerUpSliders.Count > 0)
        {
            _activePowerUpSliders[0].gameObject.transform.localPosition -= _powerUpOffset[1];
        }
    }
}
