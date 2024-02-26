using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    public delegate void DieAction();
    public static event DieAction OnDie;
    public AudioSource moveAudioSource;
    [SerializeField] private AudioSource _leveUpAudioSource;
    [SerializeField] private AudioSource _powerUpAudioSource;
    [SerializeField] private AudioSource _damageReceivedSource;
    [SerializeField] private AudioSource _swordHitSource;
    [SerializeField] private float _pitch;
    private int _realTimeLevel;
    private int _realTimeLVP;
    private int _realTimeEXP;
    private FillHealthBar _fillHealthBar;
    private PermanentPowerUpsSettings _permanentPowerUpsSettings;
    public Animator [] _animators {  get; private set; }
    [SerializeField] private ParticleSystem _shieldParticleSystem;
    [SerializeField] private GameObject _swordParticleSystemPrefab;
    public Image wiiImage;
    public Image yeiiImage;
    public Image ouchImage;
    public bool IsDead { get; set; }
    public int Exp { get; set; }
    public int LevelPoints { get; set; }
    public int CriticalRate { get; set; }
    public int Damage { get; set; }
    public int CriticalDamage { get; set; }
    public float TimeScale { get; set; }
    public float XLeftBound { get; set; }
    public float XRightBound { get; set; }

    // Start is called before the first frame update
    void Start()    
    {
        XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
        XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
        _fillHealthBar = FindObjectOfType<FillHealthBar>();
        DataPersistantManager.Instance.LoadPlayerStats();
        _animators = GetComponentsInChildren<Animator>();
        _permanentPowerUpsSettings = PermanentPowerUpsSettings.Instance;

        if (_permanentPowerUpsSettings.IsFrontSwordActive)
        {
            _permanentPowerUpsSettings.ActivateSword();
        }
        if (_permanentPowerUpsSettings.IsBackShootActive)
        {
            _permanentPowerUpsSettings.ActivateBackCannon();
        }
        _realTimeLevel = Level;
        _realTimeLVP = LevelPoints;
        _realTimeEXP = Exp;
        _leveUpAudioSource.volume = .5f;
    }

    public override void Die()
    {
        Debug.Log("GameOver");
        IsDead = true;
        if (OnDie != null) { OnDie(); }
        //GameOver
    }

    public void ActivateSwordParticles(Vector3 position)
    {
        var rotation = Random.Range(0.0f, 360f);
        var _swordParticleSystem = Instantiate(_swordParticleSystemPrefab);
        _swordParticleSystem.transform.position = position;
        _swordParticleSystem.transform.Rotate(0, 0, rotation);
        PLaySwordHitSound();
        //Activate slash sound
    }
    public void ActivateShieldParticles()
    {
        _shieldParticleSystem.Play();
    }

    public override void TryToMove()
    {
        throw new System.NotImplementedException();
    }

    public void ComprobateLifeRemaining ()
    {
        _fillHealthBar.FillSliderValue();
        if (HP <= 0)
        {
            Die();
        }
    }

    public void TownReceiveDamage()
    {
        var shields = GameManager.Instance._townHpText.GetComponentsInChildren(
            GameManager.Instance.TownHpShields[GameManager.Instance.TownHpShields.Count - 1].GetType()
            );

        GameManager.Instance.TownHpShieldsDamaged++;
        shields[shields.Length - GameManager.Instance.TownHpShieldsDamaged].GetComponent<Image>().sprite 
            = GameManager.Instance.TownDamagedShieldImages[0].GetComponent<Image>().sprite;

        if(GameManager.Instance.TownHpShields.Count == GameManager.Instance.TownHpShieldsDamaged)
        {
            Die();
        }
    }

    public bool IsCritical()
    {
        var isCritical = false;
        var random = Random.Range(0, 100);
        float criticalDamageFloat = (float)CriticalDamage / 100;
        if (CriticalRate > random)
        {
            isCritical = true;
            Damage = (int)(Attack * (criticalDamageFloat));
        }
        else
        {
            Damage = Attack;
        }
        return isCritical;
    }

    public void FillSliderValue()
    {
        _fillHealthBar.FillSliderValue();
    }

    public void AccelerateTime(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Started) {  return; }

        if (TimeScale < 20)
        {
            TimeScale *= 2;
            Time.timeScale = TimeScale;
        }
        var text = $"TimeScale x{TimeScale}";
        GameManager.Instance.ChangeAndShowDevText(text);
    }

    public void DecelerateTime(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) { return; }

        if (TimeScale > .25f)
        {
            TimeScale /= 2;
            Time.timeScale = TimeScale;
        }
        var text = $"TimeScale x{TimeScale}";
        GameManager.Instance.ChangeAndShowDevText(text);
    }

    public void RealTimeLevelUp(int exp)
    {
        _realTimeEXP += exp;

        if (_realTimeEXP > 20 * _realTimeLevel)
        {
            _realTimeEXP -= 20 * _realTimeLevel;
            _realTimeLVP += 2;
            _realTimeLevel++;
            PlayLevelUpSound();
            StartCoroutine(GameManager.Instance.ShowLevelUpText());
            GameManager.Instance._playerLevelPointsText.text = $": {_realTimeLVP}"; 
            GameManager.Instance._playerLevelText.text = $"Lvl: {_realTimeLevel}";
        }
    }

    public void PlayMoveSound()
    {
        if (!moveAudioSource.isPlaying)
        {
            moveAudioSource.pitch = _pitch;
            moveAudioSource.Play();
        }
    }

    public void PlayLevelUpSound()
    {
        _leveUpAudioSource.Play();
    }

    public void PlayDamageReceivedSound()
    {
        _damageReceivedSource.Play();
    }

    public void PLaySwordHitSound()
    {
        _swordHitSource.Play();
    }

    public void PlayPowerUpSound()
    {
        _powerUpAudioSource.pitch = .9f;
        _powerUpAudioSource.Play();
    }

    public void ShowWiiImageInSeconds(float seconds)
    {
        StartCoroutine(ShowWiiImage(seconds));
    }

    public void ShowYeiiImageInSeconds(float seconds)
    {
        StartCoroutine(ShowYeiiImage(seconds));
    }

    public void ShowOuchImageInSeconds(float seconds)
    {
        StartCoroutine(ShowOuchImage(seconds));
    }

    private IEnumerator ShowWiiImage(float seconds)
    {
        wiiImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        wiiImage.gameObject.SetActive(false);
    }

    private IEnumerator ShowYeiiImage(float seconds)
    {
        yeiiImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        yeiiImage.gameObject.SetActive(false);
    }

    private IEnumerator ShowOuchImage(float seconds)
    {
        ouchImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        ouchImage.gameObject.SetActive(false);
    }
}
