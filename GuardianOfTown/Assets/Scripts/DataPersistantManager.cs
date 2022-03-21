using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistantManager : MonoBehaviour
{
    public static DataPersistantManager Instance;

    private SpawnManager spawnManager;
    private PlayerController playerController;
    
    public int wave;
    public int playerLevel;
    public int playerHP;
    public int playerHpMax;
    public int playerAttack;
    public int playerDefense;
    public float playerSpeed;
    public int townHP;

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
        playerLevel = 1;
        playerHP = 100;
        playerHpMax = playerHP;
        playerAttack = 10;
        playerDefense = 10;
        playerSpeed = 10f;
        townHP = 100;
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
        playerLevel = playerController.Level;
        playerHP = playerController.HP;
        playerHpMax = playerController.HpMax;
        playerAttack = playerController.Attack;
        playerDefense = playerController.Defense;
        playerSpeed = playerController.Speed;
        townHP = playerController.TownHP;
    }

    public void LoadPlayerStats()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.Level = playerLevel;
        playerController.HP = playerHP;
        playerController.HpMax = playerHpMax;
        playerController.Attack = playerAttack;
        playerController.Defense = playerDefense;
        playerController.Speed = playerSpeed;
        playerController.TownHP = townHP;
    }

    public void SaveNextWave()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        wave = spawnManager.actualWave++;    
    }
}
