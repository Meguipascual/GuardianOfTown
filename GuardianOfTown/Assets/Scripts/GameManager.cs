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
    public static GameManager SharedInstance;
    [SerializeField] private Camera mainCamera;
    private Quaternion cameraStartRotation;
    private PlayerController playerController;
    private SpawnManager spawnManager;
    private GameObject dataPersistantManagerGameObject;
    public List<Image> TownHpShields;
    public TextMeshProUGUI _townHpText;
    public TextMeshProUGUI _gameOverText;
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
    public GameObject _menuCanvas;
    public GameObject _generalCanvas;
    public GameObject _levelEndCanvas;
    private bool _pauseToggle;
    private string _cameraQuake = "CameraQuake";
    public int NumberOfEnemiesAndBosses { get; set; }
    public int NumberOfStagesLeft { get; set; }

    private void Awake()
    {
        SharedInstance = this;
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
        mainCamera = FindObjectOfType<Camera>();
        dataPersistantManagerGameObject = DataPersistantManager.Instance.GetComponent<GameObject>();
        _playerLevelText.text = "Lvl: " + DataPersistantManager.Instance.SavedPlayerLevel;
        _stageText.text = "Stage: " + (DataPersistantManager.Instance.Stage + 1);
        _menuPlayerLevelText.text = $"Level: {DataPersistantManager.Instance.SavedPlayerLevel}";
        _menuPlayerHPText.text = $"HP Max: {DataPersistantManager.Instance.SavedPlayerHpMax}";
        _menuPlayerAttackText.text = $"Attack: {DataPersistantManager.Instance.SavedPlayerAttack}";
        _menuPlayerDefenseText.text = $"Defense: {DataPersistantManager.Instance.SavedPlayerDefense}";
        _menuPlayerSpeedText.text = $"Speed: {DataPersistantManager.Instance.SavedPlayerSpeed}";
        _menuPlayerCriticalRateText.text = $"Critical Rate: {DataPersistantManager.Instance.SavedPlayerCriticalRate}%";
        _menuPlayerCriticalDamageText.text = $"Critical Damage: {DataPersistantManager.Instance.SavedPlayerCriticalDamage * 100}%";
        _playerLevelPointsText.text = $"LP: {DataPersistantManager.Instance.SavedPlayerLevelPoints}";
        _enemiesLeftText.text = $"Enemies Left: {NumberOfEnemiesAndBosses}";
        //Trying leveling up
        //LevelUp();
        


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
        cameraStartRotation = mainCamera.transform.rotation;
        
        if (DataPersistantManager.Instance.Wave == 0 && !playerController.IsDead)
        {
            StartCoroutine(ShowStageText());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsDead)
        {
            _gameOverText.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
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

    private void ToggleMenu()
    {
        if (_pauseToggle)
        {
            Time.timeScale = 1;
            _menuCanvas.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            _menuCanvas.SetActive(true);
        }
        _pauseToggle = !_pauseToggle;
    }

    public void ReturnToMainMenuButton()
    {
        Time.timeScale = 1;
        Destroy(dataPersistantManagerGameObject);
        SceneManager.LoadScene(0);
    }
    
    public void ResumeButton()
    {
        Time.timeScale = 1;
        _menuCanvas.SetActive(false);
        _pauseToggle = !_pauseToggle;
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
        if (mainCamera != null)
        {
            mainCamera.GetComponent<Animator>().Play(_cameraQuake, 0, 0.0f);
        }
    }

    public void DecreaseNumberOfEnemies()
    {
        NumberOfEnemiesAndBosses--;
        _enemiesLeftText.text = $"Enemies Left: {NumberOfEnemiesAndBosses}";
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
}
