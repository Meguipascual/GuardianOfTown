using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Camera MainCamera {get; set;}
    private Quaternion cameraStartRotation;
    private PlayerController playerController;
    private SpawnManager spawnManager;
    private GameObject dataPersistantManagerGameObject;
    public List<Image> TownHpShields;
    public List<Image> TownDamagedShieldImages;
    public TextMeshProUGUI _devText;
    public TextMeshProUGUI _townHpText;
    public TextMeshProUGUI _stageText;
    public TextMeshProUGUI _stagePopUpText; 
    public TextMeshProUGUI _wavePopUpText;
    public TextMeshProUGUI _playerLevelText;
    public TextMeshProUGUI _playerLevelPointsText;
    public TextMeshProUGUI _enemiesLeftText;    
    public TextMeshProUGUI _projectileText;
    public TextMeshProUGUI _menuPlayerLevelText;
    public TextMeshProUGUI _menuPlayerHPText;       
    public TextMeshProUGUI _menuPlayerAttackText;
    public TextMeshProUGUI _menuPlayerDefenseText;
    public TextMeshProUGUI _menuPlayerSpeedText;
    public TextMeshProUGUI _menuPlayerCriticalRateText;
    public TextMeshProUGUI _menuPlayerCriticalDamageText;
    public Image[] _powerUpIcons; 
    public GameObject _menuCanvas;
    public GameObject _generalCanvas;
    public GameObject _levelEndCanvas;
    public GameObject _gameOverCanvas;
    public GameObject _WinCanvas;
    public GameObject _powerUpIconsPanel;
    public GameObject _levelUpPanel;
    private Coroutine _previousCoroutine;
    private int _stageToActivateRedFog;
    private float[] _iconsOffsetX;
    private Vector3 _iconLocalScale;
    private string _cameraQuake = "CameraQuake";
    public bool IsGamePaused {  get; set; }
    public int NumberOfEnemiesAndBosses { get; set; }
    public int NumberOfStagesLeft { get; set; }
    public int TownHpShieldsDamaged { get; set; }

    private void Awake()
    {
        Instance = this;
        _iconsOffsetX = new float[] { -700, -500, -300, -100, 100, 300, 500, 700 };
        _iconLocalScale = new Vector3(6.3f, 1.6f, 1.6f);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(NumberOfStagesLeft < 0)
        {
            return;
        }
        _stageToActivateRedFog = 6;

        playerController = FindObjectOfType<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        MainCamera = FindObjectOfType<Camera>();
        dataPersistantManagerGameObject = DataPersistantManager.Instance.GetComponent<GameObject>();
        _playerLevelText.text = "Lvl: " + DataPersistantManager.Instance.SavedPlayerLevel;
        _stageText.text = "Stage\n" + (DataPersistantManager.Instance.Stage + 1);
        _menuPlayerLevelText.text = $"Level: {DataPersistantManager.Instance.SavedPlayerLevel}";
        _menuPlayerHPText.text = $"HP Max: {DataPersistantManager.Instance.SavedPlayerHpMax}";
        _menuPlayerAttackText.text = $"Attack: {DataPersistantManager.Instance.SavedPlayerAttack}";
        _menuPlayerDefenseText.text = $"Defense: {DataPersistantManager.Instance.SavedPlayerDefense}";
        _menuPlayerSpeedText.text = $"Speed: {DataPersistantManager.Instance.SavedPlayerSpeed}";
        _menuPlayerCriticalRateText.text = $"Critical Rate: {DataPersistantManager.Instance.SavedPlayerCriticalRate}%";
        _menuPlayerCriticalDamageText.text = $"Critical Damage: {DataPersistantManager.Instance.SavedPlayerCriticalDamage}%";
        _playerLevelPointsText.text = $": {DataPersistantManager.Instance.SavedPlayerLevelPoints}";
        _enemiesLeftText.text = $": {NumberOfEnemiesAndBosses}";

        if (DataPersistantManager.Instance.SavedTownHpShields.Count > 0)
        {
            DataPersistantManager.Instance.LoadTownHp();
        }else
        {
            DataPersistantManager.Instance.InitializeTownHp();
        }

        for(int i = 0; i < TownHpShields.Count; i++)
        {
            Instantiate(TownHpShields[i], _townHpText.transform).gameObject.SetActive(true);
        }

        spawnManager.ControlWavesSpawn();
        cameraStartRotation = MainCamera.transform.rotation;

        if (DataPersistantManager.Instance.Wave == 0 && !playerController.IsDead)
        {
            StartCoroutine(ShowStageText());
        }

        if (DataPersistantManager.Instance.Stage >= _stageToActivateRedFog)
        {
            UnityEngine.RenderSettings.fog = true;
            UnityEngine.RenderSettings.fogColor = new Color(0.49f,0,0.06f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsDead)
        {
            _gameOverCanvas.gameObject.SetActive(true);
            _generalCanvas.gameObject.SetActive(true);
            _menuCanvas.gameObject.SetActive(false);
            _levelEndCanvas.gameObject.SetActive(false);
            _WinCanvas.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(ControlButtons._menu) || Input.GetButtonDown("Pause"))
        {
            ToggleMenu();
        }
    }

    public IEnumerator ShowWaveText(string text)
    {
        _wavePopUpText.text = text;
        _wavePopUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        _wavePopUpText.gameObject.SetActive(false);
    }

    IEnumerator ShowStageText()
    {
        _stagePopUpText.text = $"Stage {(DataPersistantManager.Instance.Stage + 1)}";
        _stagePopUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        _stagePopUpText.gameObject.SetActive(false);
    }

    public IEnumerator ShowLevelUpText()
    {
        float levelUpTimer = 0;
        var speed = 200;
        var initialPosition = _levelUpPanel.transform.localPosition;
        _levelUpPanel.gameObject.SetActive(true);
        playerController.ShowWiiImageInSeconds(.5f);
        
        while (levelUpTimer < .5f)
        {
            var pos =_levelUpPanel.transform.position += Vector3.up * speed * Time.deltaTime;
            levelUpTimer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _levelUpPanel.transform.localPosition = initialPosition;
        _levelUpPanel.gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        if (playerController.IsDead) { return; }

        if (IsGamePaused)
        {
            Time.timeScale = 1;
            _menuCanvas.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            _menuCanvas.SetActive(true);
        }
        IsGamePaused = !IsGamePaused;
    }

    public void RetryButton()
    {
        Time.timeScale = 1;
        Destroy(PermanentPowerUpsSettings.Instance.gameObject);
        Destroy(DataPersistantManager.Instance.gameObject);
        Destroy(PermanentPowerUpManager.Instance.gameObject);
        PermanentPowerUpsSettings.Instance.PowerUpIcons.Clear();
        SceneManager.LoadScene(1);
    }


    public void ReturnToMainMenuButton()
    {
        Time.timeScale = 1;
        Destroy(PermanentPowerUpsSettings.Instance.gameObject);
        Destroy(DataPersistantManager.Instance.gameObject);
        Destroy(PermanentPowerUpManager.Instance.gameObject);
        PermanentPowerUpsSettings.Instance.PowerUpIcons.Clear();
        SceneManager.LoadScene(0);
    }
    
    public void ResumeButton()
    {
        Time.timeScale = 1;
        _menuCanvas.SetActive(false);
        IsGamePaused = !IsGamePaused;
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity player
        #endif
    }

    public void ShakeCamera()
    {
        if (MainCamera != null)
        {
            MainCamera.GetComponent<Animator>().Play(_cameraQuake, 0, 0.0f);
        }
    }

    public void DecreaseNumberOfEnemies()
    {
        NumberOfEnemiesAndBosses--;
        _enemiesLeftText.text = $": {NumberOfEnemiesAndBosses}";
        if (NumberOfEnemiesAndBosses == 0)
        {
            spawnManager.ChangeWave();
        }
    }

    public void LevelUp()
    {
        _generalCanvas.SetActive(false);
        _levelEndCanvas.SetActive(true);
    }

    public void ChangeAndShowDevText(string Text)
    {
        _devText.text = Text;
        if(_previousCoroutine != null)
        {
            StopCoroutine(_previousCoroutine);
        }
        _previousCoroutine = StartCoroutine(ShowDevText());
    }

    IEnumerator ShowDevText()
    {
        _devText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _devText.gameObject.SetActive(false);
    }
    public void ShowSavedIcons(List <Image> PowerUpIcons)
    {
        Image icons;
        for (int i = 0; i < PowerUpIcons.Count; i++)
        {
            icons = Instantiate(PowerUpIcons[i], _powerUpIconsPanel.gameObject.transform);
            icons.transform.localScale = _iconLocalScale;
            icons.transform.localPosition = new Vector3(_iconsOffsetX[i], 30, 0);
            icons.gameObject.SetActive(true);
        }
    }

    public void ShowPowerUpIcon(int icon)
    {
        _powerUpIcons[icon].gameObject.SetActive(true);
    }

    public void ShowPowerUpIcon(int icon, String text)
    {
        _powerUpIcons[icon].gameObject.SetActive(true);
        _powerUpIcons[icon].GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void HidePowerUpIcon(int icon)
    {
        _powerUpIcons[icon].gameObject.SetActive(false);
    }
}
