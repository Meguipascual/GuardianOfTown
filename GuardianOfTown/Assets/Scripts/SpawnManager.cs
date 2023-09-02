using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] _enemyPrefab;
    [SerializeField] private GameObject[] _bossPrefab;
    [SerializeField] private GameObject [] _powerupPrefab;
    [SerializeField] private StageWavesScriptableObjects [] _stagesData;
    [SerializeField] private float _spawnDistanceZ;
    private PlayerController _playerController;
    private float _spawnSpeed = 5f;//the higher the speed the slower the spawn
    private float _spawnPoweupSpeed = 9; 
    public int CurrentStage { get; private set; }
    public int CurrentWave { get; private set; }
    public int LevelOfEnemies { get; private set; }
    public int LevelOfBosses { get; private set; }

    private void Awake()
    {
        LoadLevelOfEnemies();
    }

    private void LoadLevelOfEnemies()
    {
        LevelOfBosses = _stagesData[CurrentStage]._wavesData[CurrentWave].LevelOfBosses;
        LevelOfEnemies = _stagesData[CurrentStage]._wavesData[CurrentWave].LevelOfEnemies;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        if (DataPersistantManager.Instance != null)
        {
            CurrentStage = DataPersistantManager.Instance.Stage;
            CurrentWave = DataPersistantManager.Instance.Wave;
            if(CurrentStage < _stagesData.Length)
            {
                LoadNumberOfEnemies();
            }
            else
            {
                GameManager.SharedInstance.NumberOfEnemiesAndBosses = 0;
            }
        }
        else
        {
            CurrentStage = 0;
            Debug.Log("data persistant error ");
        }
    }

    private void LoadNumberOfEnemies()
    {
        GameManager.SharedInstance.NumberOfEnemiesAndBosses = _stagesData [CurrentStage]._wavesData [CurrentWave].numberOfEnemiesToCreate + _stagesData [CurrentStage]._wavesData [CurrentWave].numberOfBossesToCreate;
        GameManager.SharedInstance.NumberOfStagesLeft = _stagesData.Length - CurrentStage;
        GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {GameManager.SharedInstance.NumberOfEnemiesAndBosses}";
    }

    private void Update()
    {
        if (_playerController.IsDead)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnAmountOfBosses(int amountOfBosses)
    {
        float bossY;
        float bossX;
        int bossPrefab;
        Vector3 enemyPosition;

        for (int i = 0; i < amountOfBosses; i++)
        {
            bossPrefab = Random.Range(0, _bossPrefab.Length);
            bossX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[_stagesData[CurrentStage]._wavesData[CurrentWave].Gate], 
                DataPersistantManager.Instance.SpawnBoundariesRight[_stagesData[CurrentStage]._wavesData[CurrentWave].Gate]);//-23 23
            bossY = _bossPrefab[bossPrefab].transform.localScale.y;
            enemyPosition = new Vector3(bossX, bossY, _spawnDistanceZ);
            Instantiate(_bossPrefab[bossPrefab], enemyPosition, gameObject.transform.rotation);
            GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {amountOfBosses - (i + 1)}";
            yield return new WaitForSeconds(_spawnSpeed / (CurrentStage + 1));
        }
    }

    IEnumerator SpawnAmountOfEnemies(WaveScriptableObject spawnSettings)
    {
        Vector3 enemyPosition;
        int enemyType;
        float enemyX;
        float enemyY;

        for (int i = 0; i < spawnSettings.numberOfEnemiesToCreate; i++)
        {
            enemyType = Random.Range(0, _enemyPrefab.Length);
            enemyX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[_stagesData[CurrentStage]._wavesData[CurrentWave].Gate],
                DataPersistantManager.Instance.SpawnBoundariesRight[_stagesData[CurrentStage]._wavesData[CurrentWave].Gate]);//-23 23
            enemyY = _enemyPrefab[enemyType].transform.localScale.y;
            enemyPosition = new Vector3(enemyX, enemyY, _spawnDistanceZ);
            yield return new WaitForSeconds(_spawnSpeed / (CurrentStage + 1));
            Instantiate(_enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
        }
        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnAmountOfBosses(spawnSettings.numberOfBossesToCreate));
    }

    IEnumerator SpawnPowerups()
    {
        var powerupY = 1f;
        Vector3 powerupPosition;
        int powerupType;
        float powerupX;
        
        while (!_playerController.IsDead)
        {
            powerupType = Random.Range(0, _powerupPrefab.Length);
            powerupX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[_stagesData[CurrentStage]._wavesData[CurrentWave].Gate],
                DataPersistantManager.Instance.SpawnBoundariesRight[_stagesData[CurrentStage]._wavesData[CurrentWave].Gate]); //-23 23
            powerupPosition = new Vector3(powerupX, powerupY, _spawnDistanceZ);
            yield return new WaitForSeconds(_spawnPoweupSpeed + (CurrentStage + 1));
            Instantiate(_powerupPrefab[powerupType], powerupPosition, gameObject.transform.rotation);
        }
        yield return null;
    }

    public void ControlWavesSpawn()
    {
        if (_playerController.IsDead)
        {
            return;
        }

        if (CurrentStage > _stagesData.Length - 1)
        {
            FindObjectOfType<PlayerController>().IsDead = true;
            Debug.Log("Really here you might win the game, I suppose");
            // Activate win panel when it exists
            return;
        }
        else
        {
            StartCoroutine(SpawnPowerups());
            StartCoroutine(SpawnAmountOfEnemies(_stagesData[CurrentStage]._wavesData[CurrentWave]));
        }
    }
    
    public void ChangeWave()
    {
        if (CurrentWave < _stagesData[CurrentStage]._wavesData.Count-1)
        {
            CurrentWave++;
            LoadLevelOfEnemies();
            LoadNumberOfEnemies();
            ControlWavesSpawn();
            Debug.Log($"there are some waves left");
            DataPersistantManager.Instance.ChangeWave();
        }
        else
        {
            CurrentStage++;
            Debug.Log($"there are'nt any waves left");
            DataPersistantManager.Instance.ChangeStage();
        }
    }
}
