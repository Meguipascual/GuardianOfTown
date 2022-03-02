using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] enemyPrefab;
    private float spawnSpeed = 3.0f;
    private int actualWave = 6;

    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("SpawnEnemies", 3.0f, spawnSpeed / actualWave);
    }

    private void SpawnEnemies()
    {
        var enemyType = Random.Range(0, enemyPrefab.Length);
        var enemyX = Random.Range(-23, 23);
        var enemyY = 1;
        var enemyZ = 20f;
        Vector3 enemyPosition = new Vector3(enemyX, enemyY, enemyZ);

        Instantiate(enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
    }
}
