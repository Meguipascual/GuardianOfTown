using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PermanentPowerUpsSettings : MonoBehaviour
{
    public static PermanentPowerUpsSettings Instance;
    public bool IsABulletModifierActive {  get; private set; }
    public bool IsOverHeatingUnactive { get; set; }//ID = 0
    public bool IsFrontSwordActive { get; set; }//ID = 1
    public bool IsBackShootActive { get; set; }//ID = 2
    public bool IsContinuousShootActive { get; private set; }//ID = 3
    public bool IsDoubleShootActive { get; set; }//ID = 4
    public bool IsTripleShootActive { get; set; }//ID = 5
    public bool IsTownBarrierActive { get; set; }//ID = 6
    public bool [] AreMoreBulletsWasted { get; set; }//ID = 7
    public bool [] AreFireRateIncrementsWasted { get; set; }//ID = 8
    public bool[] AreTownRecoveryWasted { get; set; }//ID = 9
    public bool[] AreAreaOfEffectActive { get; set; }//ID = 10
    public int PowerUpIconsActivated {  get; set; }
    public List<Image> PowerUpIcons;
    private PlayerController _playerController;
    private Component[] _playerComponents;
    private GameObject _sword;
    private BarrierGroupLocator[] _townBarriers;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == Tags.WorldTouch)
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerComponents = _playerController.GetComponentsInChildren<Component>(true);
            _townBarriers = FindObjectsOfType<BarrierGroupLocator>(true);
            if (!IsTownBarrierActive)
            {
                HideTownBarrier();
            }
            else
            {
                ShowTownBarrier();
            }
            GameManager.Instance.ShowSavedIcons(PowerUpIcons);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
        if (!PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[0]))
        {
            PowerUpIcons.Add(GameManager.Instance._powerUpIcons[0]);
        }
        //GameManager.Instance.ShowPowerUpIcon(0);
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
                
                //GameManager.Instance.ShowPowerUpIcon(1);
            }
        }
        if (!PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[1]))
        {
            PowerUpIcons.Add(GameManager.Instance._powerUpIcons[1]);
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
                
                //GameManager.Instance.HidePowerUpIcon(1);
            }
        }
        if (PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[1]))
        {
            PowerUpIcons.Remove(GameManager.Instance._powerUpIcons[1]);
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
        IsContinuousShootActive = true;
        if (!PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[2]))
        {
            PowerUpIcons.Add(GameManager.Instance._powerUpIcons[2]);
        }
        //GameManager.Instance.ShowPowerUpIcon(2);
    }




    public void ActivateDoubleShoot()
    {
        ActivateABulletModifier();
        IsDoubleShootActive = true;
        if (!PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[3]))
        {
            PowerUpIcons.Add(GameManager.Instance._powerUpIcons[3]);
        }
        //GameManager.Instance.ShowPowerUpIcon(3);
    }

    public void ActivateTripleShoot()
    {
        if (!IsDoubleShootActive)
        {
            Debug.Log($"Double Shoot is needed");
            return;
        }
        ActivateABulletModifier();
        IsTripleShootActive = true; 
        
        if (!PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[4]))
        {
            PowerUpIcons.Add(GameManager.Instance._powerUpIcons[4]);
            PowerUpIcons.Remove(GameManager.Instance._powerUpIcons[3]);
        }
        //GameManager.Instance.HidePowerUpIcon(3);
        //GameManager.Instance.ShowPowerUpIcon(4);
    }


    public void ActivateTownBarrier()
    {
        ShowTownBarrier();
        IsTownBarrierActive = true;
        //TODO - animation for Construction and sound maybe
        if (!PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[5]))
        {
            PowerUpIcons.Add(GameManager.Instance._powerUpIcons[5]);
        }
    }

    public void ShowTownBarrier()
    {
        if (_townBarriers.Length == 0)
        {
            Debug.LogError($"Barrier not found {_townBarriers}");
            return;
        }

        foreach (var component in _townBarriers)
        {
            component.gameObject.SetActive(true);
        }
    }

    public void HideTownBarrier()
    {
        if (_townBarriers.Length == 0)
        {
            Debug.LogError($"Barrier not found {_townBarriers}");
            return;
        }

        foreach (var component in _townBarriers)
        {
            component.gameObject.SetActive(false);
        }
    }

    public void DeactivateTownBarrier()
    {
        HideTownBarrier();
        IsTownBarrierActive = false;
        //TODO - animation for destruction and sound maybe
        if (PowerUpIcons.Contains(GameManager.Instance._powerUpIcons[5]))
        {
            PowerUpIcons.Remove(GameManager.Instance._powerUpIcons[5]);
        }
    }



    public void ActivateTownRecovery()
    {
        for (int i = 0; i < AreTownRecoveryWasted.Length; i++)
        {
            if (!AreTownRecoveryWasted[i])
            {
                AreTownRecoveryWasted[i] = true;
                GameManager.Instance.TownHpShieldsDamaged --;
                return;
            }
        }
        Debug.Log($"All 'Town Recovery' improvements wasted");
    }

    public void ActivateMoreBullets()
    {
        for (int i = 0; i < AreMoreBulletsWasted.Length; i++)
        {
            if (!AreMoreBulletsWasted[i])
            {
                AreMoreBulletsWasted[i] = true;

                if (i == 0)
                {
                    PowerUpIcons.Add(GameManager.Instance._powerUpIcons[6]);
                }

                DataPersistantManager.Instance.SavedPlayerBullets += 10;
                GameManager.Instance.ShowPowerUpIcon(6, $"+{(i+1) * 10}");
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

                if (i == 0) 
                {
                    PowerUpIcons.Add(GameManager.Instance._powerUpIcons[7]);
                } 

                GameManager.Instance.ShowPowerUpIcon(7, $"{i + 1}");
                DataPersistantManager.Instance.SavedPlayerBulletsRate -= 0.025f;
                return;
            }
        }
        Debug.Log($"All 'FireRate' increments wasted");
    }
}
