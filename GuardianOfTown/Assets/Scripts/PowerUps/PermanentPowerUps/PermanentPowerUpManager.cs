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
        _powerUpOffset = new Vector3 [] { new Vector3(-505, 18, 0), new Vector3(0, 18, 0), new Vector3(505, 18, 0) };
        _numberOfPowerUps = 3;
        InstantiateNumberOfPowerUps();
    }

    public void InstantiateNumberOfPowerUps()
    {
        for(int i = 0; i < _numberOfPowerUps; i++)
        {
            var randomIndex = Random.Range(0, _powerUpPrefabs.Count);
            var powerUp = Instantiate(_powerUpPrefabs[randomIndex], gameObject.transform.position + _powerUpOffset[i], Quaternion.identity, gameObject.transform);
            //powerUp.transform.position = _powerUpOffset[i];
            _powerUpPrefabs.RemoveAt(randomIndex);
            Debug.Log($"Transform position {gameObject.transform.position}");
        }
    }
}
