using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Vector3 SavedPlayerPosition { get; set; }
    public List<Image> SavedTownHpShields { get; set; }

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
        SavedTownHpShields = new List<Image>(5);
        Wave = 1;
        SavedPlayerLevel = 1;
        SavedPlayerHP = 15;
        SavedPlayerHpMax = SavedPlayerHP;
        SavedPlayerAttack = 10;
        SavedPlayerDefense = 10;
        SavedPlayerSpeed = 15f;
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
        SavedPlayerPosition = playerController.transform.position;
        SavedTownHpShields = GameManager.SharedInstance.TownHpShields.ToList();
    }
    public void InitialiceTownHp()
    {
        for (int i = 0; i < GameManager.SharedInstance.TownHpShields.Count; i++)
        {
            SavedTownHpShields[i] = GameManager.SharedInstance.TownHpShields[i];
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
        playerController.Speed = SavedPlayerSpeed;
        playerController.transform.position = SavedPlayerPosition;
        GameManager.SharedInstance.TownHpShields = SavedTownHpShields.ToList();
    }

    public void SaveNextWave()
    {
        Wave++;
    }
}
