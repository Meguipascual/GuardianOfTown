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
    public Vector3 playerPosition;

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
        wave = 1;
        playerLevel = 1;
        playerHP = 100;
        playerHpMax = playerHP;
        playerAttack = 10;
        playerDefense = 10;
        playerSpeed = 12f;
        townHP = 100;
        playerPosition = new Vector3(0, 1, -10);
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
        playerPosition = playerController.transform.position;
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
        playerController.transform.position = playerPosition;
    }

    public void SaveNextWave()
    {
        wave++;
    }
}
