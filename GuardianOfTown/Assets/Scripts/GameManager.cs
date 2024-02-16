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
    public TextMeshProUGUI _gateText;
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
    public GameObject _menuPanel;
    public GameObject _generalPanel;
    public GameObject _levelEndPanel;
    public GameObject _rewardedAdPanel;
    public GameObject _gameOverPanel;
    public GameObject _winPanel;
    public GameObject _powerUpIconsPanel;
    public GameObject _levelUpPanel;
    public GameObject _keyboardControlsPanel;
    public GameObject _touchControlsInGamePanel;
    public GameObject _gamepadControlsPanel;
    public Button _retryButton, _winMainMenuButton, _resumeButton, _adButton;
    private Coroutine _previousCoroutine;
    private float[] _iconsOffsetX;
    private Vector3 _iconLocalScale;
    private string _cameraQuake = "CameraQuake";
    public bool IsGamePaused {  get; set; }
    public int NumberOfEnemiesAndBosses { get; set; }
    public int NumberOfStagesLeft { get; set; }
    public int TownHpShieldsDamaged { get; set; }
    public int StageToActivateRedFog { get; set; }

    private void Awake()
    {
        Instance = this;
        _iconsOffsetX = new float[] { -700, -500, -300, -100, 100, 300, 500, 700 };
        _iconLocalScale = new Vector3(6.3f, 1.6f, 1.6f);
        StageToActivateRedFog = 6;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(NumberOfStagesLeft < 0)
        {
            return;
        }
        
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
        DataPersistantManager.Instance.IsStageEnded = false;

        PlayerController.OnDie += ShowRewardedAdPanel;
        DataPersistantManager.OnWin += ShowWinPanel;
        Enemy.OnEnemyDie += DecreaseNumberOfEnemies;

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
            if(SystemInfo.deviceType == DeviceType.Handheld && DataPersistantManager.Instance.Stage == 0)
            {
                ShowControls();
            }
            else
            {
                StartCoroutine(ShowStageText());
            }
        }

        if (DataPersistantManager.Instance.Stage >= StageToActivateRedFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.49f,0,0.06f);
            //Maybe increase the difficulty
        }
    }

    private void ShowWinPanel()
    {
        if(_winPanel == null)
        {
            Debug.Log($"win canvas null");
        }
        Debug.Log("you win, son?");

        _rewardedAdPanel.gameObject.SetActive(false);
        _generalPanel.gameObject.SetActive(true);
        _winMainMenuButton.Select();
        _gameOverPanel.gameObject.SetActive(false);
        _menuPanel.gameObject.SetActive(false); 
        _levelEndPanel.gameObject.SetActive(false); 
        _winPanel.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void Unsubscribe()
    {
        PlayerController.OnDie -= ShowRewardedAdPanel;
        DataPersistantManager.OnWin -= ShowWinPanel;
        Enemy.OnEnemyDie -= DecreaseNumberOfEnemies;
    }

    public void RevivePlayerReward()
    {
        playerController.HP = playerController.HpMax;
        playerController.FillSliderValue();

        if (TownHpShieldsDamaged == 0)
        {
            _rewardedAdPanel.gameObject.SetActive(false);
            StartCoroutine("ResumeCountDown");
            return;
        }

        var shields = _townHpText.GetComponentsInChildren(TownHpShields[TownHpShields.Count - 1].GetType());
        shields[shields.Length - TownHpShieldsDamaged].GetComponent<Image>().sprite
            = TownHpShields[0].GetComponent<Image>().sprite;
        TownHpShieldsDamaged--;
        _rewardedAdPanel.gameObject.SetActive(false);
        StartCoroutine("ResumeCountDown");
    }

    IEnumerator ResumeCountDown()
    {
        Debug.Log("entra");
        _stagePopUpText.gameObject.SetActive(true);
        var count = 3;
        while(count > 0)
        {
            _stagePopUpText.text = $"{count}";
            yield return new WaitForSeconds(1);
            count--;
        }
        _stagePopUpText.gameObject.SetActive(false);
        playerController.IsDead = false;
    }

    private void ShowRewardedAdPanel()
    {
        if (playerController.IsDead)
        {
            _rewardedAdPanel.gameObject.SetActive(true);
            _generalPanel.gameObject.SetActive(true);
            _gameOverPanel.gameObject.SetActive(false);
            _menuPanel.gameObject.SetActive(false);
            _levelEndPanel.gameObject.SetActive(false);
            _winPanel.gameObject.SetActive(false);
            _adButton.Select();
        }
    }

    public void ShowGameOverPanel()
    {
        _gameOverPanel.gameObject.SetActive(true);
        _generalPanel.gameObject.SetActive(true);
        _menuPanel.gameObject.SetActive(false);
        _levelEndPanel.gameObject.SetActive(false);
        _winPanel.gameObject.SetActive(false);
        _rewardedAdPanel.gameObject.SetActive(false);
        _retryButton.Select();
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

    public void ShowControls()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
        _touchControlsInGamePanel.SetActive(true);
    }

    public void HideControls()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
        _touchControlsInGamePanel.SetActive(false);
        StartCoroutine(ShowStageText());
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
        if (playerController.IsDead || DataPersistantManager.Instance.IsStageEnded) { return; }

        if (IsGamePaused)
        {
            Time.timeScale = 1;
            _menuPanel.SetActive(false);
            _keyboardControlsPanel.SetActive(false);
            _gamepadControlsPanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            _menuPanel.SetActive(true);
        }
        _resumeButton.Select();
        IsGamePaused = !IsGamePaused;
    }

    public void ResumeGame()
    {
        if (playerController.IsDead || DataPersistantManager.Instance.IsStageEnded) { return; }
        Time.timeScale = 1;
        _menuPanel.SetActive(false);
        _keyboardControlsPanel.SetActive(false);
        _gamepadControlsPanel.SetActive(false);
        _resumeButton.Select();
        IsGamePaused = false;
    }

    public void RetryButton()
    {
        Time.timeScale = 1;
        Destroy(PermanentPowerUpsSettings.Instance.gameObject);
        Destroy(DataPersistantManager.Instance.gameObject);
        Destroy(PermanentPowerUpManager.Instance.gameObject);
        Destroy(FindObjectOfType<DontDestroyOnLoad>());
        PermanentPowerUpsSettings.Instance.PowerUpIcons.Clear();
        SceneManager.LoadScene(Tags.WorldTouch);
    }


    public void ReturnToMainMenuButton()
    {
        Time.timeScale = 1;
        Destroy(PermanentPowerUpsSettings.Instance.gameObject);
        Destroy(DataPersistantManager.Instance.gameObject);
        Destroy(PermanentPowerUpManager.Instance.gameObject);
        Destroy(FindObjectOfType<DontDestroyOnLoad>().gameObject);
        PermanentPowerUpsSettings.Instance.PowerUpIcons.Clear();
        SceneManager.LoadScene(Tags.Menu);
    }
    
    public void ContinueToRankingButton()
    {
        SceneManager.LoadScene(Tags.Ranking);
    }

    public void ResumeButton()
    {
        ToggleMenu();
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

    public void DecreaseNumberOfEnemies(int gate)
    {
        NumberOfEnemiesAndBosses--;
        _enemiesLeftText.text = $": {NumberOfEnemiesAndBosses}";
        if (NumberOfEnemiesAndBosses == 0 && !playerController.IsDead)
        {
            spawnManager.ChangeWave();
        }
    }

    public void LevelUp()
    {
        _generalPanel.SetActive(false);
        _levelEndPanel.SetActive(true);
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
