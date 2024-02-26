using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ShootingManager : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator[] _animators;
    private CallbackContext _callback;
    [SerializeField] private float _pitchMin;
    [SerializeField] private float _pitchMax;
    [SerializeField] private float _bulletTimer;//Timer to know when to shoot again
    [SerializeField] private Vector3 _centerBulletOffset;
    [SerializeField] private Vector3[] _doubleBulletOffset;
    [SerializeField] private Vector3[] _tripleBulletOffset;
    [SerializeField] private AudioSource _popAudioClip;
    public float BulletDelay { get; set; }//Time between bullets in continuous shooting(Fire Rate)


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animators = GetComponentsInChildren<Animator>();
        _bulletTimer = 0;
        BulletDelay = DataPersistantManager.Instance.SavedPlayerBulletsRate;
        _doubleBulletOffset = new Vector3[] { new Vector3(0.4f, 0, 1), new Vector3(1.2f, 0, 1) };
        _tripleBulletOffset = new Vector3[] { new Vector3(0.8f, 0, 1), new Vector3(0.0f, 0, 1), new Vector3(1.6f, 0, 1) };
        _centerBulletOffset = new Vector3(0.8f, 2, 1);
    }

    // Update is called once per frame
    private void Update()
    {
        if (DataPersistantManager.Instance.IsStageEnded || _playerController.IsDead || GameManager.Instance.IsGamePaused || GameManager.Instance.IsCountDownActive)
        {
            return;
        }

        if (SystemInfo.deviceType == DeviceType.Handheld) { return; }

        if (_callback.phase == InputActionPhase.Started || _callback.phase == InputActionPhase.Performed) 
        { 
            ShootEasyMode(); 
        }else 
        {
            if (OverHeatedManager.Instance._cannonOverHeatedTimer > 0)
            {
                OverHeatedManager.Instance.CoolCannon();
            }
        }
    }

    public void FireButtonHold(CallbackContext context)
    {
        _callback = context;
    }

    public void TryToShoot(CallbackContext context)
    {
        if (DataPersistantManager.Instance.IsStageEnded || _playerController.IsDead || GameManager.Instance.IsGamePaused || context.phase != InputActionPhase.Started || GameManager.Instance.IsCountDownActive)
        {
            return;
        }

        if (PermanentPowerUpsSettings.Instance.IsContinuousShootActive)
        {
            return;
        }

        if (PermanentPowerUpsSettings.Instance.IsABulletModifierActive)
        {
            DecideShoot();
        }
        else
        {
            Shoot();
        }
    }

    public void TryToShoot()
    {
        if (DataPersistantManager.Instance.IsStageEnded || _playerController.IsDead || GameManager.Instance.IsGamePaused)
        {
            return;
        }

        if (PermanentPowerUpsSettings.Instance.IsContinuousShootActive)
        {
            return;
        }

        if (PermanentPowerUpsSettings.Instance.IsABulletModifierActive)
        {
            DecideShoot();
        }
        else
        {
            Shoot();
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

    private void Shoot()
    {
        // Get an object object from the pool
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
        if (pooledProjectile != null)
        {
            pooledProjectile.SetActive(true); // activate it
            pooledProjectile.transform.position = _playerController.transform.position + _centerBulletOffset; // position it at player
            ObjectPooler.ProjectileCount--;
            GameManager.Instance._projectileText.text = "" + ObjectPooler.ProjectileCount;
            _popAudioClip.pitch = Random.Range(_pitchMin, _pitchMax);
            _popAudioClip.Play();
            AnimateCannonRotation();
        }
    }

    private void ShootDouble()
    {
        var shooting = false;
        for (int i = 0; i < 2; i++)
        {
            // Get an object object from the pool
            GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
            if (pooledProjectile != null)
            {
                shooting = true;
                pooledProjectile.SetActive(true); // activate it
                pooledProjectile.transform.position = _playerController.transform.position + _doubleBulletOffset[i]; // position bullet
                ObjectPooler.ProjectileCount--;
            }
        }
        if (shooting)
        {
            GameManager.Instance._projectileText.text = "" + ObjectPooler.ProjectileCount;
            _popAudioClip.pitch = Random.Range(_pitchMin, _pitchMax);
            _popAudioClip.Play();
            AnimateCannonRotation();
        }
    }

    private void ShootTriple()
    {
        var shooting = false;   
        for (int i = 0; i < 3; i++)
        {
            // Get an object object from the pool
            GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
            if (pooledProjectile != null)
            {
                shooting = true;
                var bullet = pooledProjectile.GetComponent<BulletManager>();
                bullet.ResetStartPositionInTripleShoot();

                switch (i)
                {
                    case 0: bullet.IsLeftBullet = true; break;
                    case 2: bullet.IsRightBullet = true;break; 
                }
                pooledProjectile.transform.position = _playerController.transform.position + _tripleBulletOffset[i]; // position bullet
                pooledProjectile.SetActive(true); // activate it
                ObjectPooler.ProjectileCount--;
            }
        }
        if (shooting)
        {
            GameManager.Instance._projectileText.text = "" + ObjectPooler.ProjectileCount;
            _popAudioClip.pitch = Random.Range(_pitchMin, _pitchMax);
            _popAudioClip.Play();
            AnimateCannonRotation();
        }
    }

    public void ShootEasyMode()
    {
        if (!PermanentPowerUpsSettings.Instance.IsContinuousShootActive && !PowerUpSettings.Instance.IsContinuousShootInUse)
        {
            if (OverHeatedManager.Instance._cannonOverHeatedTimer >= 0)
            {
                OverHeatedManager.Instance.CoolCannon();
            }
            return; 
        }

        if ((OverHeatedManager.Instance.IsOverheatedCannon() && !PermanentPowerUpsSettings.Instance.IsOverHeatingUnactive) || ObjectPooler.ProjectileCount == 0)
        {
            OverHeatedManager.Instance.CoolCannon();
            return;
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (!PermanentPowerUpsSettings.Instance.IsOverHeatingUnactive)
            {
                OverHeatedManager.Instance.HeatCannon();
            }
            _bulletTimer += Time.deltaTime;
            if (_bulletTimer >= BulletDelay)
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
            if (_callback.phase == InputActionPhase.Performed || _callback.phase == InputActionPhase.Started)
            {
                if (!PermanentPowerUpsSettings.Instance.IsOverHeatingUnactive)
                {
                    OverHeatedManager.Instance.HeatCannon();
                }
                _bulletTimer += Time.deltaTime;
                if (_bulletTimer >= BulletDelay)
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
    }

    private void AnimateCannonRotation()
    {
        var animator = _animators[2];
        for (int i = 0; i < _animators.Length; i++)
        {
            if (_animators[i].name.Equals("Cannon"))
            {
                animator = _animators[i];
            }
        }
        
        if (animator.GetBool("Shoot1"))
        {
            animator.SetBool("Shoot1", false);
            animator.SetBool("Shoot2", true);
            animator.SetBool("Shoot3", false);
        }
        else if (animator.GetBool("Shoot2"))
        {
            animator.SetBool("Shoot1", false);
            animator.SetBool("Shoot2", false);
            animator.SetBool("Shoot3", true);
        }
        else 
        {
            animator.SetBool("Shoot1", true); 
            animator.SetBool("Shoot2", false);
            animator.SetBool("Shoot3", false);
        }
    }
}
