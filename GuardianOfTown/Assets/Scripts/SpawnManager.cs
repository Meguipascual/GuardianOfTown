using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] enemyPrefab;
    [SerializeField] private GameObject[] powerupPrefab;
    private float spawnSpeed = 5f;//the higher the speed the slower the spawn
    private float spawnPoweupSpeed = 9; 
    public int ActualWave { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (DataPersistantManager.Instance != null)
        {
            ActualWave = DataPersistantManager.Instance.Wave;
        }
        else
        {
            ActualWave = 1;
            Debug.Log("data persistant error ");
        } 
    }

    IEnumerator SpawnAmountOfBosses(int amountOfBosses)
    {
        var bossY = 1.8f;//Because Boss is bigger
        var bossPrefab = enemyPrefab.Length - 1;
        var bossZ = 20f;
        for (int i = 0; i < amountOfBosses; i++)
        {
            var bossX = Random.Range(-23f, 23f);
            Vector3 enemyPosition = new Vector3(bossX, bossY, bossZ);
            Instantiate(enemyPrefab[bossPrefab], enemyPosition, gameObject.transform.rotation);
            GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {amountOfBosses - (i + 1)}";
            yield return new WaitForSeconds(spawnSpeed / ActualWave);
        }
    }

    IEnumerator SpawnAmountOfEnemies(int amountOfEnemies, int amountOfBosses)
    {
        var enemyY = 1f;
        var enemyZ = 20f;
        Vector3 enemyPosition;
        int enemyType;
        float enemyX;
        GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {amountOfEnemies + amountOfBosses}";
        for (int i = 0; i < amountOfEnemies; i++)
        {
            if (FindObjectOfType<PlayerController>().IsDead)
            {
                StopAllCoroutines();
            }
            else
            {
                enemyType = Random.Range(0, enemyPrefab.Length - 1);
                enemyX = Random.Range(-23f, 23f);
                enemyPosition = new Vector3(enemyX, enemyY, enemyZ);
                yield return new WaitForSeconds(spawnSpeed / ActualWave);
                Instantiate(enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
                GameManager.SharedInstance.enemiesLeftText.text = $"Enemies Left: {(amountOfEnemies - (i+1)) + amountOfBosses}";
            }
        }
        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnAmountOfBosses(amountOfBosses));
    }

    IEnumerator SpawnPowerups()
    {
        var powerupY = 1f;
        var powerupZ = 20f;
        Vector3 powerupPosition;
        int powerupType;
        float powerupX;
        if (FindObjectOfType<PlayerController>().IsDead)
        {
            StopAllCoroutines();
        }
        else
        {
            while (!FindObjectOfType<PlayerController>().IsDead)
            {
                powerupType = Random.Range(0, powerupPrefab.Length);
                powerupX = Random.Range(-23f, 23f);
                powerupPosition = new Vector3(powerupX, powerupY, powerupZ);
                yield return new WaitForSeconds(spawnPoweupSpeed + ActualWave);
                Instantiate(powerupPrefab[powerupType], powerupPosition, gameObject.transform.rotation);
            }
        }
        yield return null;
    }

    public void ControlWavesSpawn()
    {
        if (FindObjectOfType<PlayerController>().IsDead)
        {
            return;
        }
        else
        {
            switch (ActualWave)
            {
                case 1:
                    StartCoroutine(SpawnAmountOfEnemies(6, 1));//6
                    StartCoroutine(SpawnPowerups());
                    break;
                case 2:
                    StartCoroutine(SpawnAmountOfEnemies(12, 2));//12 
                    StartCoroutine(SpawnPowerups());
                    break;
                case 3:
                    StartCoroutine(SpawnAmountOfEnemies(20, 1));//20
                    StartCoroutine(SpawnPowerups());
                    break;
                case 4:
                    StartCoroutine(SpawnAmountOfEnemies(40, 1));//40
                    StartCoroutine(SpawnPowerups());
                    break;
                case 5:
                    StartCoroutine(SpawnAmountOfEnemies(65, 1));//65
                    StartCoroutine(SpawnPowerups());
                    break;
                case 6:
                    StartCoroutine(SpawnAmountOfEnemies(80, 1));//80
                    StartCoroutine(SpawnPowerups());
                    break;
                case 7:
                    StartCoroutine(SpawnAmountOfEnemies(120, 1));//120
                    StartCoroutine(SpawnPowerups());
                    break;
                case 8:
                    StartCoroutine(SpawnAmountOfEnemies(200, 1));//200
                    StartCoroutine(SpawnPowerups());
                    break;
                case 9:
                    StartCoroutine(SpawnAmountOfEnemies(220, 1));//220
                    StartCoroutine(SpawnPowerups());
                    break;
                case 10:
                    StartCoroutine(SpawnAmountOfEnemies(250, 1));//250
                    StartCoroutine(SpawnPowerups());
                    break;
                case 11:
                    GameManager.SharedInstance.wavePopUpText.text = "Hero, you saved the town.\nHurray!";
                    GameManager.SharedInstance.wavePopUpText.gameObject.SetActive(true);
                    Time.timeScale = 0;
                    break;
                default:
                    FindObjectOfType<PlayerController>().IsDead = true;
                    //Really here you might win the game, I suppose
                    break;
            }
        }
    } 
}
