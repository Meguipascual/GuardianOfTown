using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] _enemyPrefab;
    [SerializeField] private GameObject [] _powerupPrefab;
    [SerializeField] private SpawnManagerScriptableObject [] _wavesData;
    private PlayerController _playerController;
    private float _spawnSpeed = 5f;//the higher the speed the slower the spawn
    private float _spawnPoweupSpeed = 9; 
    public int CurrentWave { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
        _playerController = FindObjectOfType<PlayerController>();
        if (DataPersistantManager.Instance != null)
        {
            CurrentWave = DataPersistantManager.Instance.Wave;
            GameManager.SharedInstance.NumberOfBosses = _wavesData[CurrentWave].numberOfBossesToCreate;
        }
        else
        {
            CurrentWave = 1;
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
        var bossY = 1.8f;//Because Boss is bigger
        var bossPrefab = _enemyPrefab.Length - 1;
        var bossZ = 20f;
        for (int i = 0; i < amountOfBosses; i++)
        {
            var bossX = Random.Range(-23f, 23f);
            Vector3 enemyPosition = new Vector3(bossX, bossY, bossZ);
            Instantiate(_enemyPrefab[bossPrefab], enemyPosition, gameObject.transform.rotation);
            GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {amountOfBosses - (i + 1)}";
            yield return new WaitForSeconds(_spawnSpeed / (CurrentWave + 1));
        }
    }

    IEnumerator SpawnAmountOfEnemies(SpawnManagerScriptableObject spawnSettings)
    {
        var enemyY = 1f;
        var enemyZ = 20f;
        Vector3 enemyPosition;
        int enemyType;
        float enemyX;
        GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {spawnSettings.numberOfEnemiesToCreate + spawnSettings.numberOfBossesToCreate}";
        for (int i = 0; i < spawnSettings.numberOfEnemiesToCreate; i++)
        {
            enemyType = Random.Range(0, _enemyPrefab.Length - 1);
            enemyX = Random.Range(-23f, 23f);
            enemyPosition = new Vector3(enemyX, enemyY, enemyZ);
            yield return new WaitForSeconds(_spawnSpeed / (CurrentWave + 1));
            Instantiate(_enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
            GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {(spawnSettings.numberOfEnemiesToCreate - (i+1)) + spawnSettings.numberOfBossesToCreate}";
        }
        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnAmountOfBosses(spawnSettings.numberOfBossesToCreate));
    }

    IEnumerator SpawnPowerups()
    {
        var powerupY = 1f;
        var powerupZ = 20f;
        Vector3 powerupPosition;
        int powerupType;
        float powerupX;
        
        while (!_playerController.IsDead)
        {
            powerupType = Random.Range(0, _powerupPrefab.Length);
            powerupX = Random.Range(-23f, 23f);
            powerupPosition = new Vector3(powerupX, powerupY, powerupZ);
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
        else
        {
            if (CurrentWave <= _wavesData.Length - 1)
            {
                StartCoroutine(SpawnPowerups());
                StartCoroutine(SpawnAmountOfEnemies(_wavesData[CurrentWave]));
            }
            else
            {
                FindObjectOfType<PlayerController>().IsDead = true;
                //Really here you might win the game, I suppose
            }
        }
    } 
}
