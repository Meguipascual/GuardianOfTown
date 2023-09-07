using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeatedManager : MonoBehaviour
{
    [SerializeField] private Material _cannonMaterial;
    [SerializeField] private Material _cannonColdMaterial;
    [SerializeField] private Material _overheatedCannonMaterial;
    private PlayerController _playerController;

    public static OverHeatedManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _playerController = GetComponentInParent<PlayerController>();
        if(_playerController == null)
        {
            Debug.Log($"PlayerController not Found");
        }
    }

    public void ChangeCannonMaterial(float heatPercentage)
    {
        if(heatPercentage < 0) { return; }

        Debug.Log($"Heat Percentage: {heatPercentage}");
        _cannonMaterial.Lerp(_cannonColdMaterial, _overheatedCannonMaterial, heatPercentage);
    }

}
