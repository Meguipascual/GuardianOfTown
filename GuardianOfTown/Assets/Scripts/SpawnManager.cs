using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] enemyPrefab;
    [SerializeField] private float spawnSpeed;
    private int wavesCleared;

    // Start is called before the first frame update
    void Start()
    {
        spawnSpeed = 3.0f;
        wavesCleared = 1;
        InvokeRepeating("SpawnEnemies", 3.0f, spawnSpeed / wavesCleared);
    }

    private void SpawnEnemies()
    {
        var enemyType = Random.RandomRange(0, enemyPrefab.Length);

        Instantiate(enemyPrefab[enemyType]);
    }
}
