using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerUpsSettings : MonoBehaviour
{
    public static PermanentPowerUpsSettings instance;
    public bool IsOverHeatingUnactive { get; set; }
    public bool IsFrontSwordActive { get; set; }
    public bool IsBackShootActive { get; set; }
    public bool IsInfiniteContinuousShootActive { get; set; }
    public bool IsDoubleShootActive { get; set; }
    public bool IsTripleShootActive { get; set; }
    public bool IsTownBarrierActive { get; set; }
    public bool [] AreMoreBulletsWasted { get; set; }
    public bool [] AreShootSpeedWasted { get; set; }
    public bool[] AreTownRecoveryWasted { get; set; }
    public bool[] AreAreaOfEffectActive { get; set; }
    private PlayerController _playerController;
    private GameObject[] _playerComponents;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerComponents = GetComponentsInChildren<GameObject>();
        AreMoreBulletsWasted = new bool[3];
        AreTownRecoveryWasted = new bool[3];
        AreAreaOfEffectActive = new bool[3];
        AreShootSpeedWasted = new bool[5];
    }

    public void CreateSword()
    {
        foreach (var component in _playerComponents)
        {
            if (component.CompareTag(Tags.Sword))
            {
                IsFrontSwordActive = true;
                component.gameObject.SetActive(true);//activate Player's sword
            }
            else
            {
                Debug.Log($"Sword Not found to activate");
            }
        }
    }

    public void DestroySword()
    {
        foreach (var component in _playerComponents)
        {
            if (component.CompareTag(Tags.Sword))
            {
                IsFrontSwordActive = false;
                component.gameObject.SetActive(false);//Deactivate Player's sword
            }
            else
            {
                Debug.Log($"Sword Not found to deactivate");
            }
        }
    }
}
