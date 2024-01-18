using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] _enemyPrefab;
    [SerializeField] private GameObject[] _bossPrefab;
    [SerializeField] private GameObject [] _powerupPrefab;
    [SerializeField] private StagesData _stagesData;
    [SerializeField] private float _spawnDistanceZ;
    private List<StageWavesScriptableObjects> _stagesDataList;
    private PlayerController _playerController;
    private float _spawnSpeed = 1f;//the higher the speed the slower the spawn
    private float _spawnPoweupSpeed = 9;
    private int[] _enemiesByGate;
    public int CurrentStage { get; private set; }
    public int CurrentWave { get; private set; }
    public int LevelOfEnemies { get; private set; }
    public int LevelOfBosses { get; private set; }

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _stagesDataList = _stagesData.StagesDataList;
        LoadLevelOfEnemies();
        if (DataPersistantManager.Instance != null)
        {
            LoadStageAndWaveCurrentIndex();
        }
    }

    private void LoadLevelOfEnemies()
    {
        LevelOfBosses = _stagesDataList[CurrentStage]._wavesData[CurrentWave].LevelOfBosses;
        LevelOfEnemies = _stagesDataList[CurrentStage]._wavesData[CurrentWave].LevelOfEnemies;
    }
    private void LoadStageAndWaveCurrentIndex()
    {
        CurrentStage = DataPersistantManager.Instance.Stage;
        CurrentWave = DataPersistantManager.Instance.Wave;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (DataPersistantManager.Instance != null)
        {
            CurrentStage = DataPersistantManager.Instance.Stage;
            if (CurrentStage < _stagesDataList.Count)
            {
                LoadNumberOfEnemies();
            }
            else
            {
                GameManager.Instance.NumberOfEnemiesAndBosses = 600;
            }
        }
        else
        {
            CurrentStage = 0;
            Debug.Log("data persistant error ");
        }
        Enemy.OnEnemyDie += DecreaseEnemiesByGate;
        _enemiesByGate = new int[] { 0, 0, 0, 0 };
    }

    private void OnDestroy()
    {
        Unsuscribe();
    }

    private void OnDisable()
    {
        Unsuscribe();
    }
    private void Unsuscribe()
    {
        Enemy.OnEnemyDie -= DecreaseEnemiesByGate;
    }

    private void LoadNumberOfEnemies()
    {
        GameManager.Instance.NumberOfEnemiesAndBosses = _stagesDataList [CurrentStage]._wavesData [CurrentWave].NumberOfEnemiesToCreate + _stagesDataList [CurrentStage]._wavesData [CurrentWave].NumberOfBossesToCreate;
        GameManager.Instance.NumberOfStagesLeft = _stagesDataList.Count - CurrentStage;
        GameManager.Instance._enemiesLeftText.text = $": {GameManager.Instance.NumberOfEnemiesAndBosses}";
    }

    private void Update()
    {
        if (_playerController.IsDead)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnAmountOfBosses(WaveData spawnSettings)
    {
        float bossY;
        float bossX;
        int bossPrefab;
        Vector3 enemyPosition;
        if (spawnSettings.IsRandomized)
        {
            for (int i = 0; i < spawnSettings.NumberOfBossesToCreate; i++)
            {
                var gate = Random.Range(0, 4);
                bossPrefab = Random.Range(0, _bossPrefab.Length);
                bossX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[gate], DataPersistantManager.Instance.SpawnBoundariesRight[gate]);
                bossY = _bossPrefab[bossPrefab].transform.localScale.y;
                enemyPosition = new Vector3(bossX, bossY, _spawnDistanceZ);
                var enemy = Instantiate(_bossPrefab[bossPrefab], enemyPosition, gameObject.transform.rotation);
                enemy.GetComponent<Enemy>().Gate = gate;
                _enemiesByGate[gate]++;
                ChangeGateManager.instance.ActivateWarningImage(gate);
                yield return new WaitForSeconds(_spawnSpeed / (CurrentStage + 1));
            }
        }
        else
        {
            for (int i = 0; i < spawnSettings.NumberOfBossesToCreate; i++)
            {
                var gate = _stagesDataList[CurrentStage]._wavesData[CurrentWave].Gate;
                bossPrefab = Random.Range(0, _bossPrefab.Length);
                bossX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[gate], DataPersistantManager.Instance.SpawnBoundariesRight[gate]);
                bossY = _bossPrefab[bossPrefab].transform.localScale.y;
                enemyPosition = new Vector3(bossX, bossY, _spawnDistanceZ);
                var enemy = Instantiate(_bossPrefab[bossPrefab], enemyPosition, gameObject.transform.rotation);
                enemy.GetComponent<Enemy>().Gate = gate;
                _enemiesByGate[gate]++;
                ChangeGateManager.instance.ActivateWarningImage(gate);
                yield return new WaitForSeconds(_spawnSpeed / (CurrentStage + 1));
            }
        }
    }

    private void DecreaseEnemiesByGate(int gate)
    {
        _enemiesByGate[gate] -= 1;
        if (_enemiesByGate[gate] <= 0)
        {
            ChangeGateManager.instance.DeactivateWarningImage(gate);
        } 
    }

    IEnumerator SpawnAmountOfEnemies(WaveData spawnSettings)
    {
        Vector3 enemyPosition;
        int enemyType;
        float enemyX;
        float enemyY;

        if (!spawnSettings.IsRandomized)
        {
            for (int i = 0; i < spawnSettings.NumberOfEnemiesToCreate; i++)
            {
                var gate = _stagesDataList[CurrentStage]._wavesData[CurrentWave].Gate;
                enemyType = Random.Range(0, _enemyPrefab.Length);
                enemyX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[gate], DataPersistantManager.Instance.SpawnBoundariesRight[gate]);
                enemyY = _enemyPrefab[enemyType].transform.localScale.y;
                enemyPosition = new Vector3(enemyX, enemyY, _spawnDistanceZ);
                yield return new WaitForSeconds(_spawnSpeed / (CurrentStage + 1));
                var enemy = Instantiate(_enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
                enemy.GetComponent<Enemy>().Gate = gate;
                _enemiesByGate[gate]++;
                ChangeGateManager.instance.ActivateWarningImage(gate);
            }
        }
        else
        {
            for (int i = 0; i < spawnSettings.NumberOfEnemiesToCreate; i++)
            {
                var gate = Random.Range(0, 4);
                enemyType = Random.Range(0, _enemyPrefab.Length);
                enemyX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[gate], DataPersistantManager.Instance.SpawnBoundariesRight[gate]);
                enemyY = _enemyPrefab[enemyType].transform.localScale.y;
                enemyPosition = new Vector3(enemyX, enemyY, _spawnDistanceZ);
                yield return new WaitForSeconds(_spawnSpeed / (CurrentStage + 1));
                var enemy = Instantiate(_enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
                enemy.GetComponent<Enemy>().Gate = gate;
                _enemiesByGate[gate]++;
                ChangeGateManager.instance.ActivateWarningImage(gate);
            }
        }

        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnAmountOfBosses(spawnSettings));
    }

    IEnumerator SpawnPowerups(WaveData spawnSettings)
    {
        var powerupY = 1f;
        Vector3 powerupPosition;
        int powerupType;
        float powerupX;
        
        while (!_playerController.IsDead && !GameManager.Instance.IsGamePaused)
        {
            if (GameSettings.Instance.IsEasyModeActive || PermanentPowerUpsSettings.Instance.IsInfiniteContinuousShootActive)
            {
                powerupType = Random.Range(1, _powerupPrefab.Length);//the first position is continuous shoot powerUp, in easy mode is not needed
            }
            else
            {
                powerupType = Random.Range(0, _powerupPrefab.Length);
            }

            if (spawnSettings.IsRandomized)
            {
                var gate = Random.Range(0, 4);
                powerupX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[gate], DataPersistantManager.Instance.SpawnBoundariesRight[gate]);
            }
            else
            {
                powerupX = Random.Range(DataPersistantManager.Instance.SpawnBoundariesLeft[_stagesDataList[CurrentStage]._wavesData[CurrentWave].Gate],
                DataPersistantManager.Instance.SpawnBoundariesRight[_stagesDataList[CurrentStage]._wavesData[CurrentWave].Gate]);
            }
            
            powerupPosition = new Vector3(powerupX, powerupY, _spawnDistanceZ);
            yield return new WaitForSeconds(_spawnPoweupSpeed + (CurrentStage));
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

        if (CurrentStage > _stagesDataList.Count - 1 || (_stagesDataList[CurrentStage] == null))
        {
            _playerController.IsDead = true;
            return;
        }
        else
        {
            StartCoroutine(SpawnPowerups(_stagesDataList[CurrentStage]._wavesData[CurrentWave]));
            StartCoroutine(SpawnAmountOfEnemies(_stagesDataList[CurrentStage]._wavesData[CurrentWave]));
            if(CurrentWave == 0)
            {
                DataPersistantManager.Instance.ChangeWave(true, _stagesDataList[CurrentStage]._wavesData[CurrentWave].IsRandomized, _stagesDataList[CurrentStage]._wavesData[CurrentWave].Gate);
            }
        }
    }
    
    public void ChangeWave()
    {
        if (CurrentWave < _stagesDataList[CurrentStage]._wavesData.Count-1)
        {
            CurrentWave++;
            LoadLevelOfEnemies();
            LoadNumberOfEnemies();
            ControlWavesSpawn();
            Debug.Log($"there are some waves left");
            DataPersistantManager.Instance.ChangeWave(false, _stagesDataList[CurrentStage]._wavesData[CurrentWave].IsRandomized, _stagesDataList[CurrentStage]._wavesData[CurrentWave].Gate);
        }
        else
        {
            CurrentStage++;
            GameManager.Instance.NumberOfStagesLeft = _stagesDataList.Count - CurrentStage;
            Debug.Log($"there are'nt any waves left");
            DataPersistantManager.Instance.ChangeStage();
        }
    }
}
