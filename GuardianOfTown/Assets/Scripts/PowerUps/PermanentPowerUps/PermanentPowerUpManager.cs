using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerUpManager : MonoBehaviour
{
    public List<GameObject> _powerUpPrefabs;
    [SerializeField] private int _numberOfPowerUps;
    [SerializeField] private Vector3[] _powerUpOffset;

    // Start is called before the first frame update
    void Start()
    {
        _powerUpOffset = new Vector3 [] { new Vector3(-460, 18, 0), new Vector3(0, 18, 0), new Vector3(460, 18, 0) };
        _numberOfPowerUps = 3;
        InstantiateNumberOfPowerUps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EliminatePowerUp(int powerUpToEliminate)
    {

    }

    public void InstantiateNumberOfPowerUps()
    {
        for(int i = 0; i < _numberOfPowerUps; i++)
        {
            var randomIndex = Random.Range(0, _powerUpPrefabs.Count);
            var powerUp = Instantiate(_powerUpPrefabs[randomIndex], gameObject.transform.position + _powerUpOffset[i], Quaternion.identity, gameObject.transform);
            _powerUpPrefabs.RemoveAt(randomIndex);
            Debug.Log($"Transform position {gameObject.transform.position}");
            //_powerUpOffset[i]
        }
    }
}
