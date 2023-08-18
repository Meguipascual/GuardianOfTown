using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeatedManager : MonoBehaviour
{
    [SerializeField] private Material _cannonMaterial;
    private PlayerController _playerController;
    private Color _previousColor;

    public static OverHeatedManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _previousColor = _cannonMaterial.color;
        _playerController = GetComponentInParent<PlayerController>();
        if(_playerController == null)
        {
            Debug.Log($"PlayerController not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void ChangeCannonMaterial(float heatPercentage)
    {
        Debug.Log($"Color: {_cannonMaterial.color.linear}");
        //_cannonMaterial.color = Color.red;
    }

}
