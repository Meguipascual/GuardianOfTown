using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistantManager : MonoBehaviour
{
    public static DataPersistantManager Instance;
    private SpawnManager spawnManager;
    private PlayerController playerController;
    
    public int Wave { get; set; }
    public int SavedPlayerLevel { get; set; }
    public int SavedPlayerHP { get; set; }
    public int SavedPlayerHpMax { get; set; }
    public int SavedPlayerAttack { get; set; }
    public int SavedPlayerDefense { get; set; }
    public float SavedPlayerSpeed { get; set; }
    public int SavedTownHP { get; set; }
    public Vector3 SavedPlayerPosition { get; set; }

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
        Wave = 1;
        SavedPlayerLevel = 1;
        SavedPlayerHP = 100;
        SavedPlayerHpMax = SavedPlayerHP;
        SavedPlayerAttack = 10;
        SavedPlayerDefense = 10;
        SavedPlayerSpeed = 15f;
        SavedTownHP = 100;
        SavedPlayerPosition = new Vector3(0, 1, -10f);
    }
    public void ChangeStage()
    {
        SaveNextWave();
        SavePlayerStats();
        SceneManager.LoadScene(1);
    }
    public void SavePlayerStats()
    {
        playerController = FindObjectOfType<PlayerController>();
        SavedPlayerLevel = playerController.Level;
        SavedPlayerHP = playerController.HP;
        SavedPlayerHpMax = playerController.HpMax;
        SavedPlayerAttack = playerController.Attack;
        SavedPlayerDefense = playerController.Defense;
        SavedPlayerSpeed = playerController.Speed;
        SavedTownHP = playerController.TownHP;
        SavedPlayerPosition = playerController.transform.position;
    }

    public void LoadPlayerStats()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.Level = SavedPlayerLevel;
        playerController.HP = SavedPlayerHP;
        playerController.HpMax = SavedPlayerHpMax;
        playerController.Attack = SavedPlayerAttack;
        playerController.Defense = SavedPlayerDefense;
        playerController.Speed = SavedPlayerSpeed;
        playerController.TownHP = SavedTownHP;
        playerController.transform.position = SavedPlayerPosition;
    }

    public void SaveNextWave()
    {
        Wave++;
    }
}
