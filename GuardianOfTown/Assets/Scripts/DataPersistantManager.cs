using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistantManager : MonoBehaviour
{
    public static DataPersistantManager Instance;

    private SpawnManager spawnManager;
    private PlayerManager playerManager;
    
    public int wave;
    public int playerLevel;
    public int playerHP;
    public int playerHpMax;
    public int playerAttack;
    public int playerDefense;
    public float playerSpeed;
    public int TownHP;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        playerLevel = 1;
        playerHP = 100;
        playerHpMax = playerHP;
        playerAttack = 10;
        playerDefense = 10;
        playerSpeed = 10f;
        TownHP = 100;
    }
    public void ChangeStage()
    {
        wave = spawnManager.actualWave++;
        SceneManager.LoadScene(1);
        //player stats= player stats in this script
    }
}
