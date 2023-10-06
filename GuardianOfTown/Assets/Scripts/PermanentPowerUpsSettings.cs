using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerUpsSettings : MonoBehaviour
{
    public static PermanentPowerUpsSettings Instance;
    public bool IsABulletModifierActive {  get; private set; }
    public bool IsOverHeatingUnactive { get; set; }
    public bool IsFrontSwordActive { get; set; }
    public bool IsBackShootActive { get; set; }
    public bool IsInfiniteContinuousShootActive { get; private set; }
    public bool IsDoubleShootActive { get; set; }
    public bool IsTripleShootActive { get; set; }
    public bool IsTownBarrierActive { get; set; }
    public bool [] AreMoreBulletsWasted { get; set; }
    public bool [] AreFireRateIncrementsWasted { get; set; }
    public bool[] AreTownRecoveryWasted { get; set; }
    public bool[] AreAreaOfEffectActive { get; set; }
    private PlayerController _playerController;
    private ShootingManager _shootingManager;
    private Component[] _playerComponents;
    private GameObject _sword;
    private GameObject _backCannon;
    private ObjectPooler _objectPooler;

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
        _objectPooler = FindObjectOfType<ObjectPooler>();
        _shootingManager = FindObjectOfType<ShootingManager>();
        _playerComponents = _playerController.GetComponentsInChildren<Component>(true);
        AreMoreBulletsWasted = new bool[3];
        AreTownRecoveryWasted = new bool[3];
        AreAreaOfEffectActive = new bool[3];
        AreFireRateIncrementsWasted = new bool[5];
    }

    public void ActivateABulletModifier()
    {
        IsABulletModifierActive = true;
    }
    public void DeactivateABulletModifier()
    {
        IsABulletModifierActive = true;
    }



    public void DeactivateOverHeating()
    {
        IsOverHeatingUnactive = true;
    }



    public void ActivateSword()
    {
        if (_sword != null)
        {
            IsFrontSwordActive = true;
            _sword.SetActive(true);
            return;
        }
        foreach (var component in _playerComponents)
        {
            if (component.gameObject.CompareTag(Tags.Sword))
            {
                IsFrontSwordActive = true;
                _sword = component.gameObject;
                _sword.SetActive(true);
            }
        }
    }

    public void DeactivateSword()
    {
        if (_sword != null)
        {
            _sword.SetActive(false);
            IsFrontSwordActive = false;
            return;
        }
        foreach (var component in _playerComponents)
        {
            if (component.gameObject.CompareTag(Tags.Sword))
            {
                IsFrontSwordActive = false;
                component.gameObject.SetActive(false);//Deactivate Player's sword
            }
        }
    }


    public void ActivateBackCannon()
    {
        foreach (var component in _playerComponents)
        {
            if (component.gameObject.CompareTag(Tags.BackCannon))
            {
                IsBackShootActive = true;
                component.gameObject.SetActive(true);//Activate Player's BackCannon
            }
        }
    }

    public void DeactivateBackCannon()
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


    public void ActivateInfiniteContinuousShoot()
    {
        IsInfiniteContinuousShootActive = true;
    }




    public void ActivateDoubleShoot()
    {
        ActivateABulletModifier();
        IsDoubleShootActive = true;
    }

    public void ActivateTripleShoot()
    {
        ActivateABulletModifier();
        IsTripleShootActive = true;
    }


    

    public void ActivateMoreBullets()
    {
        for (int i = 0; i < AreMoreBulletsWasted.Length; i++)
        {
            if (!AreMoreBulletsWasted[i])
            {
                AreMoreBulletsWasted[i] = true;
                _objectPooler.InstantiatePool(_objectPooler.amountToPool+10);
                return;
            }
        }
        Debug.Log($"All 'MoreBullets' improvements wasted");
    }

    public void ActivateFireRateIncrement()
    {
        for (int i = 0; i < AreFireRateIncrementsWasted.Length; i++)
        {
            if (!AreFireRateIncrementsWasted[i])
            {
                AreFireRateIncrementsWasted[i] = true;
                _shootingManager.BulletDelay -= 0.025f;
                return;
            }
        }
        Debug.Log($"All 'FireRate' increments wasted");
    }
}
