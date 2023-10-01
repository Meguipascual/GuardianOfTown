using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerUpsSettings : MonoBehaviour
{
    public static PermanentPowerUpsSettings Instance;
    public bool IsABulletModifierActive {  get; set; }
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
    private Component[] _playerComponents;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerComponents = _playerController.GetComponentsInChildren<Component>(true);
        AreMoreBulletsWasted = new bool[3];
        AreTownRecoveryWasted = new bool[3];
        AreAreaOfEffectActive = new bool[3];
        AreShootSpeedWasted = new bool[5];
        IsABulletModifierActive = true;
        IsDoubleShootActive = true;
    }

    public void CreateSword()
    {
        foreach (var component in _playerComponents)
        {
            if (component.gameObject.CompareTag(Tags.Sword))
            {
                IsFrontSwordActive = true;
                component.gameObject.SetActive(true);//activate Player's sword
            }
        }
    }

    public void DestroySword()
    {
        foreach (var component in _playerComponents)
        {
            if (component.gameObject.CompareTag(Tags.Sword))
            {
                IsFrontSwordActive = false;
                component.gameObject.SetActive(false);//Deactivate Player's sword
            }
        }
    }

    public void CreateBackCannon()
    {
        foreach (var component in _playerComponents)
        {
            if (component.gameObject.CompareTag(Tags.BackCannon))
            {
                IsBackShootActive = true;
                component.gameObject.SetActive(true);//Activate Player's BackCannon
                Debug.Log($"BackCannon found to activate");
            }
        }
    }

    public void DestroyBackCannon()
    {
        foreach (var component in _playerComponents)
        {
            if (component.gameObject.CompareTag(Tags.BackCannon))
            {
                IsBackShootActive = false;
                component.gameObject.SetActive(false);//Deactivate Player's BackCannon
                Debug.Log($"BackCannon found to Deactivate");
            }
        }
    }
}
