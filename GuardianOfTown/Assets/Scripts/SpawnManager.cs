using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] _enemyPrefab;
    [SerializeField] private GameObject[] _bossPrefab;
    [SerializeField] private GameObject [] _powerupPrefab;
    [SerializeField] private SpawnManagerScriptableObject [] _wavesData;
    [SerializeField] private float _spawnDistanceZ;
    private PlayerController _playerController;
    private float _spawnSpeed = 5f;//the higher the speed the slower the spawn
    private float _spawnPoweupSpeed = 9; 
    public int CurrentWave { get; private set; }
    public int LevelOfEnemies { get; private set; }
    public int LevelOfBosses { get; private set; }

    private void Awake()
    {
        LevelOfBosses = _wavesData[CurrentWave].LevelOfBosses;
        LevelOfEnemies = _wavesData[CurrentWave].LevelOfEnemies;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _playerController = FindObjectOfType<PlayerController>();
        if (DataPersistantManager.Instance != null)
        {
            CurrentWave = DataPersistantManager.Instance.Wave;
            if(CurrentWave < _wavesData.Length)
            {
                GameManager.SharedInstance.NumberOfEnemiesAndBosses = _wavesData[CurrentWave].numberOfEnemiesToCreate + _wavesData[CurrentWave].numberOfBossesToCreate;
                GameManager.SharedInstance.NumberOfWavesLeft = _wavesData.Length - CurrentWave;
            }
            else
            {
                GameManager.SharedInstance.NumberOfEnemiesAndBosses = 0;
            }
        }
        else
        {
            CurrentWave = 0;
            Debug.Log("data persistant error ");
        }
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
            bossX = Random.Range(-23f, 23f);
            bossY = _bossPrefab[bossPrefab].transform.localScale.y;
            enemyPosition = new Vector3(bossX, bossY, _spawnDistanceZ);
            Instantiate(_bossPrefab[bossPrefab], enemyPosition, gameObject.transform.rotation);
            GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {amountOfBosses - (i + 1)}";
            yield return new WaitForSeconds(_spawnSpeed / (CurrentWave + 1));
        }
    }

    IEnumerator SpawnAmountOfEnemies(SpawnManagerScriptableObject spawnSettings)
    {
        Vector3 enemyPosition;
        int enemyType;
        float enemyX;
        float enemyY;

        for (int i = 0; i < spawnSettings.numberOfEnemiesToCreate; i++)
        {
            enemyType = Random.Range(0, _enemyPrefab.Length);
            enemyX = Random.Range(-23f, 23f);
            enemyY = _enemyPrefab[enemyType].transform.localScale.y;
            enemyPosition = new Vector3(enemyX, enemyY, _spawnDistanceZ);
            yield return new WaitForSeconds(_spawnSpeed / (CurrentWave + 1));
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
            powerupX = Random.Range(-23f, 23f);
            powerupPosition = new Vector3(powerupX, powerupY, _spawnDistanceZ);
            yield return new WaitForSeconds(_spawnPoweupSpeed + (CurrentWave + 1));
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

        if (CurrentWave > _wavesData.Length - 1)
        {
            FindObjectOfType<PlayerController>().IsDead = true;
            Debug.Log("Really here you might win the game, I suppose");
            // Activate win panel when it exists
            return;
        }
        else
        {
            StartCoroutine(SpawnPowerups());
            StartCoroutine(SpawnAmountOfEnemies(_wavesData[CurrentWave]));
        }
    } 
}
