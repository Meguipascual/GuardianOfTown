using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject [] enemyPrefab;
    private float spawnSpeed = 3.0f;
    private int actualWave = 1;

    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("SpawnEnemies", 3.0f, spawnSpeed / actualWave);
    }

    private void SpawnEnemies()
    {
        if (FindObjectOfType<PlayerManager>().IsDead)
        {
            CancelInvoke("InvokeRepeating");
            return;
        }
        else
        {
            var enemyType = Random.Range(0, enemyPrefab.Length);
            float enemyX = Random.Range(-23f, 23f);
            var enemyY = 1f;
            var enemyZ = 20f;
            Vector3 enemyPosition = new Vector3(enemyX, enemyY, enemyZ);

            Instantiate(enemyPrefab[enemyType], enemyPosition, gameObject.transform.rotation);
        }
        
    }
}
