using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    private PlayerController _playerController;
    private KeyCode _shoot;
    private KeyCode _alternateShoot;
    [SerializeField] private float _bulletTimer;//Timer to know when to shoot again
    [SerializeField] private float _bulletDelay;//Time between bullets in continuous shooting(Fire Rate)
    [SerializeField] private Vector3 _centerBulletOffset;
    [SerializeField] private Vector3[] _doubleBulletOffset;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _alternateShoot = ControlButtons._alterShoot;
        _shoot = ControlButtons._shoot;
        _bulletTimer = 0;
        _bulletDelay = 0.2f;
        _centerBulletOffset = new Vector3(0.8f, 0, 1);
        _doubleBulletOffset = new Vector3[2];
        _doubleBulletOffset[0] = new Vector3(0.4f, 0, 1);
        _doubleBulletOffset[1] = new Vector3(1.2f, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameSettings.Instance.IsEasyModeActive || PermanentPowerUpsSettings.Instance.IsInfiniteContinuousShootActive)
        {
            ShootEasyMode();
            return;
        }
        if (Input.GetKeyDown(_shoot) || Input.GetKeyDown(_alternateShoot))
        {
            if (PermanentPowerUpsSettings.Instance.IsABulletModifierActive)
            {
                DecideShoot();
            }
            else
            {
                Shoot();
            }
        }
        if (OverHeatedManager.Instance._cannonOverHeatedTimer > 0)
        {
            OverHeatedManager.Instance.CoolCannon();
        }  
    }


    private void Shoot()
    {
        // Get an object object from the pool
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
        if (pooledProjectile != null)
        {
            pooledProjectile.SetActive(true); // activate it
            pooledProjectile.transform.position = _playerController.transform.position + _centerBulletOffset; // position it at player
            ObjectPooler.ProjectileCount--;
            GameManager.SharedInstance._projectileText.text = "" + ObjectPooler.ProjectileCount;
        }
    }

    private void ShootEasyMode()
    {
        if (OverHeatedManager.Instance.IsOverheatedCannon() && !PermanentPowerUpsSettings.Instance.IsOverHeatingUnactive)
        {
            OverHeatedManager.Instance.CoolCannon();
            return;
        }
        if (Input.GetKey(_shoot) || Input.GetKey(_alternateShoot))
        {
            if (!PermanentPowerUpsSettings.Instance.IsOverHeatingUnactive)
            {
                OverHeatedManager.Instance.HeatCannon();
            }
            _bulletTimer += Time.deltaTime;
            if (_bulletTimer >= _bulletDelay)
            {
                _bulletTimer = 0;
                if (PermanentPowerUpsSettings.Instance.IsABulletModifierActive)
                {
                    DecideShoot();
                }
                else
                {
                    Shoot();
                }
            }
        }
        else
        {
            OverHeatedManager.Instance.CoolCannon();
        }
    }

    private void DecideShoot()
    {
        if (PermanentPowerUpsSettings.Instance.IsTripleShootActive)
        {
            ShootTriple();
            return;
        }
        else if (PermanentPowerUpsSettings.Instance.IsDoubleShootActive)
        {
            ShootDouble();
            return;
        }
    }

    private void ShootTriple()
    {
        // Get an object object from the pool
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
        if (pooledProjectile != null)
        {
            pooledProjectile.SetActive(true); // activate it
            pooledProjectile.transform.position = _playerController.transform.position + _centerBulletOffset; // position it at player
            ObjectPooler.ProjectileCount--;
            GameManager.SharedInstance._projectileText.text = "" + ObjectPooler.ProjectileCount;
            Debug.Log($"TripleShooting");
        }
    }

    private void ShootDouble()
    {
        for (int i = 0; i < 2; i++)
        {
            // Get an object object from the pool
            GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
            if (pooledProjectile != null)
            {
                pooledProjectile.SetActive(true); // activate it
                pooledProjectile.transform.position = _playerController.transform.position + _doubleBulletOffset[i]; // position bullet
                ObjectPooler.ProjectileCount--;
            }
        }
        GameManager.SharedInstance._projectileText.text = "" + ObjectPooler.ProjectileCount;
    }
}
