using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataPersistantManager : MonoBehaviour
{
    public static DataPersistantManager Instance;
    private StagesData _stagesData;
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
    public int SavedPlayerExp { get; set; }
    public int SavedTownHpShieldsDamaged {  get; set; }
    public int SavedPlayerCriticalDamage { get; set; }
    public float SavedPlayerSpeed { get; set; }
    public int SavedPlayerLevelPoints {  get; set; }
    public Vector3 SavedPlayerPosition { get; set; }
    public List<Image> SavedTownHpShields;
    public List<Image> SavedTownDamagedShield;
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
        Initialice();
        DontDestroyOnLoad(gameObject); 
    }
    
    private void Start()
    {
        FindSpawnManager();
        SceneManager.activeSceneChanged += (scene1, scene2) => FindSpawnManager();
    }

    private void FindSpawnManager()
    {
        _stagesData = FindObjectOfType<StagesData>();
    }

    private void Initialice()
    {
        SavedTownHpShields = new List<Image>();
        Stage = 0;
        Wave = 0;
        MaxWave = 11;
        SavedPlayerLevel = 1;
        SavedPlayerHP = 15;
        SavedPlayerHpMax = SavedPlayerHP;
        SavedPlayerAttack = 15;
        SavedPlayerDefense = 10;
        SavedPlayerSpeed = 15f;
        SavedPlayerPosition = new Vector3(0.85f, 0.9f, -10f);
        SavedPlayerCriticalRate = 10;
        SavedPlayerCriticalDamage = 100;
        SavedPlayerExp = 500;
        SavedPlayerLevelPoints = 0;
        SpawnBoundariesRight = new float[] { 23, 1528, 3028, 4528 };
        SpawnBoundariesLeft = new float[] { -23, 1482, 2982, 4482 };

    }

    public void ChangeWave(bool isFirstWave, bool isNextWaveRandom, int gate) 
    {
        if (isNextWaveRandom)
        {
            StartCoroutine(GameManager.SharedInstance.ShowWaveText($"They are attacking all our gates"));
        }
        else
        {
            string Gate = "";
            switch (gate)
            {
                case 0: Gate = "North";break;
                case 1: Gate = "East";break;
                case 2: Gate = "South";break;
                case 3: Gate = "West";break;
            }
            StartCoroutine(GameManager.SharedInstance.ShowWaveText($"New enemies' wave incoming at {Gate} Gate "));
        }

        if(!isFirstWave)
        {
            SaveNextWave();
        }
    }

    public void ChangeStage()
    {
        StopAllCoroutines();
        SaveNextStage();
        GameManager.SharedInstance.IsGamePaused = true;

        if ((Stage >= _stagesData.StagesDataList.Count))
        {
            Debug.Log("you win, son?");
            GameManager.SharedInstance._WinCanvas.SetActive(true);
            return;
        }

        if (_stagesData.StagesDataList[Stage]._wavesData.Count == 0)
        {
            Debug.Log($"Stage scriptable {Stage+1} empty");
            Debug.LogError($"Stage scriptable {Stage + 1} empty");
            //Maybe Generate a random wave to fix the problem
            _stagesData.StagesDataList[Stage] = _stagesData.GenerateRandomStage(Stage);
            Debug.Log($"Number of waves that have been created random {_stagesData.StagesDataList[Stage]._wavesData.Count}");
        }
        
        GameManager.SharedInstance.LevelUp();
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
        SavedPlayerExp = playerController.Exp;
        SavedPlayerLevelPoints = playerController.LevelPoints;
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
        SavedTownHpShieldsDamaged = GameManager.SharedInstance.TownHpShieldsDamaged;
    }

    public void LoadTownHp()
    {
        GameManager.SharedInstance.TownHpShieldsDamaged = SavedTownHpShieldsDamaged;
        GameManager.SharedInstance.TownHpShields = new List<Image>(SavedTownHpShields.Count);
        for (int i = 0; i < SavedTownHpShields.Count; i++)
        {
            if (i < (SavedTownHpShields.Count - SavedTownHpShieldsDamaged))
            {
                GameManager.SharedInstance.TownHpShields.Add(SavedTownHpShields[i].GetComponent<Image>());
            }
            else
            {
                GameManager.SharedInstance.TownHpShields.Add(SavedTownDamagedShield[i]);
            }
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
        playerController.Exp = SavedPlayerExp;
        playerController.LevelPoints = SavedPlayerLevelPoints;
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

    public void ReloadScene()
    {
        SavePlayerStats();
        SceneManager.LoadScene(1);
    }
}
