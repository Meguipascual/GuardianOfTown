using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataPersistantManager : MonoBehaviour
{
    public static DataPersistantManager Instance;
    private SpawnManager spawnManager;
    private PlayerController playerController;
    
    public int Stage { get; set; }
    public int Wave { get; set; }
    public int MaxWave { get; set; }
    public int SavedPlayerLevel { get; set; }
    public int SavedPlayerHP { get; set; }
    public int SavedPlayerHpMax { get; set; }
    public int SavedPlayerAttack { get; set; }
    public int SavedPlayerDefense { get; set; }
    public int SavedPlayerCriticalRate { get; set; }
    public float SavedPlayerCriticalDamage { get; set; }
    public float SavedPlayerSpeed { get; set; }
    public Vector3 SavedPlayerPosition { get; set; }
    public List<Image> SavedTownHpShields;
    public float[] SpawnBoundariesLeft { get; private set; }
    public float[] SpawnBoundariesRight { get; private set; }

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
    private void Start()
    {
        SavedTownHpShields = new List<Image>();
        Stage = 0;
        Wave = 0;
        MaxWave = 11;
        SavedPlayerLevel = 1;
        SavedPlayerHP = 15;
        SavedPlayerHpMax = SavedPlayerHP;
        SavedPlayerAttack = 30;
        SavedPlayerDefense = 10;
        SavedPlayerSpeed = 15f;
        SavedPlayerPosition = new Vector3(0.85f, 0.9f, -10f);
        SavedPlayerCriticalRate = 10;
        SavedPlayerCriticalDamage = 1.0f;
        SpawnBoundariesRight = new float[] { 23, 1528, 3028, 4528 };
        SpawnBoundariesLeft = new float[] { -23, 1482, 2982, 4482 };

    }

    public void ChangeWave(bool isNextWaveRandom, int gate) 
    {
        if (isNextWaveRandom)
        {
            StartCoroutine(GameManager.SharedInstance.ShowWaveText($"They are attacking all our gates"));
        }
        else
        {
            StartCoroutine(GameManager.SharedInstance.ShowWaveText($"New enemies' wave incoming at Gate {gate+1}"));
        }

        SaveNextWave();
    }

    public void ChangeStage()
    {
        SaveNextStage();
        //If there are more stages activate Level up panel and when the level up ended change Stage scriptable and reload scene
        ReloadScene();
    }
    public void SavePlayerStats()
    {
        playerController = FindObjectOfType<PlayerController>();
        SavedPlayerLevel = playerController.Level;
        SavedPlayerHP = playerController.HP;
        SavedPlayerHpMax = playerController.HpMax;
        SavedPlayerAttack = playerController.Attack;
        SavedPlayerDefense = playerController.Defense;
        SavedPlayerCriticalRate = playerController.CriticalRate;
        SavedPlayerCriticalDamage = playerController.CriticalDamage;
        SavedPlayerSpeed = playerController.Speed;
        SavedPlayerPosition = playerController.transform.position;
        SaveTownHp();
    }

    public void InitializeTownHp()
    {
        SavedTownHpShields = new List<Image>();
        foreach (Image image in GameManager.SharedInstance.TownHpShields)
        {
            SavedTownHpShields.Add(image);
        }
    }

    public void SaveTownHp()
    {
        //SavedTownHpShields = new List<Image>();
        
        while (SavedTownHpShields.Count > GameManager.SharedInstance.TownHpShields.Count) 
        {
            SavedTownHpShields.RemoveAt(SavedTownHpShields.Count - 1);
        }

    }

    public void LoadTownHp()
    {
        GameManager.SharedInstance.TownHpShields = new List<Image>(SavedTownHpShields.Count);
        foreach (Image image in SavedTownHpShields)
        {
            GameManager.SharedInstance.TownHpShields.Add(image);
        }
    }

    public void LoadPlayerStats()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.Level = SavedPlayerLevel;
        playerController.HP = SavedPlayerHP;
        playerController.HpMax = SavedPlayerHpMax;
        playerController.Attack = SavedPlayerAttack;
        playerController.Defense = SavedPlayerDefense;
        playerController.CriticalRate = SavedPlayerCriticalRate;
        playerController.CriticalDamage = SavedPlayerCriticalDamage;
        playerController.Speed = SavedPlayerSpeed;
        playerController.transform.position = SavedPlayerPosition;
    }

    public void SaveNextStage()
    {
        Wave = 0;
        Stage++;
    }

    public void SaveNextWave()
    {
        Wave++;
    }

    private void ReloadScene()
    {
        SavePlayerStats();
        SceneManager.LoadScene(3);
    }
}
