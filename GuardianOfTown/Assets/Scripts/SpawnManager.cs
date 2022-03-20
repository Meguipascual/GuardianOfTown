using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] enemyPrefab;
    private float spawnSpeed = 1.0f;
    private int actualWave = 1;

    // Start is called before the first frame update
    void Start()
    {
        //actualWave = the wave number saved in non destruct object
        SpawnEnemies();
    }

    void SpawnAmountOfBosses(int amountOfBosses)
    {
        var bossY = 1.8f;//Because Boss is bigger
        var bossPrefab = enemyPrefab.Length - 1;
        var bossZ = 20f;
        for (int i = 0; i < amountOfBosses; i++)
        {
            var bossX = Random.Range(-23f, 23f);
            Vector3 enemyPosition = new Vector3(bossX, bossY, bossZ);
            Instantiate(enemyPrefab[bossPrefab], enemyPosition, gameObject.transform.rotation);
        }
    }

    IEnumerator SpawnAmountOfEnemies(int amountOfEnemies, int amountOfBosses)
    {
        var enemyY = 1f;
        var enemyZ = 20f;
        Vector3 enemyPosition;
        int enemyType;
        float enemyX;
        for (int i = 0; i < amountOfEnemies; i++)
        {
            if (FindObjectOfType<PlayerManager>().IsDead)
            {
                StopAllCoroutines();
            }
            else
            {
                enemyType = Random.Range(0, enemyPrefab.Length - 1);
                enemyX = Random.Range(-23f, 23f);
                enemyPosition = new Vector3(enemyX, enemyY, enemyZ);
                yield return new WaitForSeconds(spawnSpeed / actualWave);
                Instantiate(enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
            }
        }
        yield return new WaitForSeconds(10);
        SpawnAmountOfBosses(3); 
        StopCoroutine(SpawnAmountOfEnemies(amountOfEnemies, amountOfBosses));
    }

    void SpawnEnemies()
    {
        if (FindObjectOfType<PlayerManager>().IsDead)
        {
            return;
        }
        else
        {
            switch (actualWave)
            {
                case 1:
                    StartCoroutine( SpawnAmountOfEnemies(100, 3));
                    break;
                case 2: break;
            }
        }
    }
}
