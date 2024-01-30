using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverHeatedManager : MonoBehaviour
{
    public static OverHeatedManager Instance;
    [SerializeField] private Material _cannonMaterial;
    [SerializeField] private Material _cannonColdMaterial;
    [SerializeField] private Material _overheatedCannonMaterial;
    [SerializeField] private AudioClip[] _overHeatSounds;
    [SerializeField] public float _cannonOverHeatedTimer;
    [SerializeField] private float _cannonOverHeatedLimit;
    [SerializeField] private float _coolDownDelay;
    private PlayerController _playerController;
    private AudioSource _audioSource;
    public static bool _isCannonOverheated;
    public Slider _cannonHeatSlider;
    public Image _cannonOverHeatingImage;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _playerController = GetComponentInParent<PlayerController>();
        if(_playerController == null)
        {
            Debug.Log($"PlayerController not Found");
            return;
        }
        ChangeCannonMaterial(0);
    }

    public void ChangeCannonMaterial(float heatPercentage)
    {
        if(heatPercentage < 0) { heatPercentage = 0; }

        _cannonMaterial.Lerp(_cannonColdMaterial, _overheatedCannonMaterial, heatPercentage);
    }

    private void FillHeatSlider(float heatPercentage)
    {
        if(heatPercentage <= 0)
        {
            _cannonHeatSlider.gameObject.SetActive(false);
        }
        else
        {
            _cannonHeatSlider.gameObject.SetActive(true);
            _cannonHeatSlider.value = heatPercentage;
        }
    }

    public void CoolCannon()
    {
        _cannonOverHeatedTimer -= Time.deltaTime;
        if(_cannonOverHeatedTimer < 0) { _cannonOverHeatedTimer = 0;}

        if (IsOverheatedCannon())
        {
            ChangeCannonMaterial(_cannonOverHeatedTimer / _coolDownDelay);
            FillHeatSlider(_cannonOverHeatedTimer / _coolDownDelay);
        }
        else
        {
            ChangeCannonMaterial(_cannonOverHeatedTimer / _cannonOverHeatedLimit);
            FillHeatSlider(_cannonOverHeatedTimer / _cannonOverHeatedLimit);
        }
            
        
    }

    public void HeatCannon()
    {
        _cannonOverHeatedTimer += Time.deltaTime;
        ChangeCannonMaterial(_cannonOverHeatedTimer / _cannonOverHeatedLimit);
        FillHeatSlider(_cannonOverHeatedTimer / _cannonOverHeatedLimit);
        IsOverheatedCannon();
    }

    public bool IsOverheatedCannon()
    {
        if(_cannonOverHeatedTimer >= _cannonOverHeatedLimit)
        {
            var randomIndex = Random.Range(0,_overHeatSounds.Length);
            _isCannonOverheated = true;
            _cannonOverHeatingImage.gameObject.SetActive(true);
            _audioSource.clip = _overHeatSounds[randomIndex];
            _audioSource.Play();
            _cannonOverHeatedTimer = _coolDownDelay;
        }
        if (_cannonOverHeatedTimer <= 0)
        {
            _isCannonOverheated = false;
            _cannonOverHeatingImage.gameObject.SetActive(false);
        }
        return _isCannonOverheated;
    }
}
