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
    public TextMeshProUGUI TownHpText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI wavePopUpText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI enemiesLeftText;
    public TextMeshProUGUI projectileText;
    public TextMeshProUGUI menuPlayerLevelText;
    public TextMeshProUGUI menuPlayerHPText;
    public TextMeshProUGUI menuPlayerAttackText;
    public TextMeshProUGUI menuPlayerDefenseText;
    public TextMeshProUGUI menuPlayerSpeedText;
    public TextMeshProUGUI menuPlayerCriticalRateText;
    public TextMeshProUGUI menuPlayerCriticalDamageText;
    public GameObject menuCanvas;
    private bool pauseToggle;
    private string cameraQuake = "CameraQuake";
    public int NumberOfBosses { get; set; }

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        dataPersistantManagerGameObject = DataPersistantManager.Instance.GetComponent<GameObject>();
        playerLevelText.text = "Lvl: " + DataPersistantManager.Instance.SavedPlayerLevel;
        waveText.text = "Wave: " + DataPersistantManager.Instance.Wave + 1;
        menuPlayerLevelText.text = $"Level: {DataPersistantManager.Instance.SavedPlayerLevel}";
        menuPlayerHPText.text = $"HP Max: {DataPersistantManager.Instance.SavedPlayerHpMax}";
        menuPlayerAttackText.text = $"Attack: {DataPersistantManager.Instance.SavedPlayerAttack}";
        menuPlayerDefenseText.text = $"Defense: {DataPersistantManager.Instance.SavedPlayerDefense}";
        menuPlayerSpeedText.text = $"Speed: {DataPersistantManager.Instance.SavedPlayerSpeed}";
        menuPlayerCriticalRateText.text = $"Critical Rate: {DataPersistantManager.Instance.SavedPlayerCriticalRate}%";
        menuPlayerCriticalDamageText.text = $"Critical Damage: {DataPersistantManager.Instance.SavedPlayerCriticalDamage * 100}%";
        
        if (DataPersistantManager.Instance.SavedTownHpShields.Count > 0)
        {
            DataPersistantManager.Instance.LoadTownHp();
        }else
        {
            DataPersistantManager.Instance.InitializeTownHp();
        }

        for(int i = 0; i < TownHpShields.Count; i++)
        {
            GameObject.Instantiate(TownHpShields[i], TownHpText.transform).gameObject.SetActive(true);
        }
        
        StartCoroutine(ShowWaveText());
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsDead)
        {
            gameOverText.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    IEnumerator ShowWaveText()
    {
        wavePopUpText.text = "Wave " + DataPersistantManager.Instance.Wave + 1;
        wavePopUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        wavePopUpText.gameObject.SetActive(false);
        spawnManager.ControlWavesSpawn();
        cameraStartRotation = mainCamera.transform.rotation;
    }

    private void ToggleMenu()
    {
        if (pauseToggle)
        {
            Time.timeScale = 1;
            menuCanvas.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            menuCanvas.SetActive(true);
        }
        pauseToggle = !pauseToggle;
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
        menuCanvas.SetActive(false);
        pauseToggle = !pauseToggle;
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
        mainCamera.GetComponent<Animator>().Play(cameraQuake, 0, 0.0f);
    }

    public void KillABoss()
    {
        NumberOfBosses--;
        if (NumberOfBosses < 1)
        {
            DataPersistantManager.Instance.ChangeStage();
        }
        return;
    }
}
