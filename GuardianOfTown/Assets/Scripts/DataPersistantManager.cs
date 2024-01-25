using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataPersistantManager : MonoBehaviour
{
    public static DataPersistantManager Instance;
    public delegate void WinAction();
    public static event WinAction OnWin;
    private StagesData _stagesData;
    private PlayerController _playerController;
    
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
    public int SavedTotalPlayerExp { get; set; }
    public int SavedTownHpShieldsDamaged {  get; set; }
    public int SavedPlayerCriticalDamage { get; set; }
    public float SavedPlayerSpeed { get; set; }
    public float SavedPlayerTimeScale { get; set; }
    public int SavedPlayerBullets { get; set; }
    public float SavedPlayerBulletsRate { get; set; }
    public int SavedPlayerLevelPoints {  get; set; }
    public Vector3 SavedPlayerPosition { get; set; }
    public List<Image> SavedTownHpShields;
    public List<Image> SavedTownDamagedShield;
    public float[] SpawnBoundariesLeft { get; private set; }
    public float[] SpawnBoundariesRight { get; private set; }
    public bool IsStageEnded { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Initialize();
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

    private void Initialize()
    {
        SavedTownHpShields = new List<Image>();
        Stage = 0;
        Wave = 0;
        MaxWave = 11;
        SavedPlayerLevel = 1;
        SavedPlayerHP = 15;
        SavedPlayerHpMax = SavedPlayerHP;
        SavedPlayerAttack = 1500;
        SavedPlayerDefense = 10;
        SavedPlayerSpeed = 25f;
        SavedPlayerPosition = new Vector3(0.85f, 0.9f, -10f);
        SavedPlayerCriticalRate = 10;
        SavedPlayerCriticalDamage = 100;
        SavedPlayerExp = 0;
        SavedTotalPlayerExp = 0;    
        SavedPlayerLevelPoints = 0;
        SavedPlayerBullets = 20;
        SavedPlayerBulletsRate = 0.2f;
        SavedPlayerTimeScale = 1;
        SpawnBoundariesRight = new float[] { 23, 1528, 3028, 4528 };
        SpawnBoundariesLeft = new float[] { -23, 1482, 2982, 4482 };
    }

    public void ChangeWave(bool isFirstWave, bool isNextWaveRandom, int gate) 
    {
        if (isNextWaveRandom)
        {
            StartCoroutine(GameManager.Instance.ShowWaveText($"They are attacking all our gates"));
        }
        else
        {
            string Gate = "";
            switch (gate)
            {
                case 0: Gate = "North";
                    break;
                case 1: Gate = "East";
                    break;
                case 2: Gate = "South";
                    break;
                case 3: Gate = "West";
                    break;
            }
            StartCoroutine(GameManager.Instance.ShowWaveText($"New enemies' wave incoming at {Gate} Gate "));
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
        GameManager.Instance.IsGamePaused = true;
        IsStageEnded = true;

        if (Stage >= _stagesData.StagesDataList.Count)
        {
            if (OnWin != null)
            {
                OnWin();
            }
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
        
        GameManager.Instance.LevelUp();
    }

    public void SavePlayerStats()
    {
        _playerController = FindObjectOfType<PlayerController>();
        SavedPlayerLevel = _playerController.Level;
        SavedPlayerHP = _playerController.HP;
        SavedPlayerHpMax = _playerController.HpMax;
        SavedPlayerAttack = _playerController.Attack;
        SavedPlayerDefense = _playerController.Defense;
        SavedPlayerCriticalRate = _playerController.CriticalRate;
        SavedPlayerCriticalDamage = _playerController.CriticalDamage;
        SavedPlayerSpeed = _playerController.Speed;
        SavedPlayerPosition = _playerController.transform.position;
        SavedPlayerExp = _playerController.Exp;
        SavedPlayerLevelPoints = _playerController.LevelPoints;
        SavedPlayerTimeScale = _playerController.TimeScale;
        SaveTownHp();
    }

    public void InitializeTownHp()
    {
        SavedTownHpShields = new List<Image>();
        foreach (Image image in GameManager.Instance.TownHpShields)
        {
            SavedTownHpShields.Add(image);
        }
    }

    public void SaveTownHp()
    {
        SavedTownHpShieldsDamaged = GameManager.Instance.TownHpShieldsDamaged;
    }

    public void LoadTownHp()
    {
        GameManager.Instance.TownHpShieldsDamaged = SavedTownHpShieldsDamaged;
        GameManager.Instance.TownHpShields = new List<Image>(SavedTownHpShields.Count);
        for (int i = 0; i < SavedTownHpShields.Count; i++)
        {
            if (i < (SavedTownHpShields.Count - SavedTownHpShieldsDamaged))
            {
                GameManager.Instance.TownHpShields.Add(SavedTownHpShields[i].GetComponent<Image>());
            }
            else
            {
                GameManager.Instance.TownHpShields.Add(SavedTownDamagedShield[i]);
            }
        }
    }

    public void LoadPlayerStats()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerController.Level = SavedPlayerLevel;
        _playerController.HP = SavedPlayerHP;
        _playerController.HpMax = SavedPlayerHpMax;
        _playerController.Attack = SavedPlayerAttack;
        _playerController.Defense = SavedPlayerDefense;
        _playerController.CriticalRate = SavedPlayerCriticalRate;
        _playerController.CriticalDamage = SavedPlayerCriticalDamage;
        _playerController.Speed = SavedPlayerSpeed;
        _playerController.transform.position = SavedPlayerPosition;
        _playerController.Exp = SavedPlayerExp;
        _playerController.TimeScale = SavedPlayerTimeScale;
        _playerController.LevelPoints = SavedPlayerLevelPoints;
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
