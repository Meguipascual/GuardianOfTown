using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerUpManager : MonoBehaviour
{
    public List<GameObject> _powerUpPrefabs;
    public List<GameObject> _notUsedPowerUpPrefabs;
    [SerializeField] private Vector3[] _powerUpOffset;
    [SerializeField] private int _numberOfPowerUps;
    
    // Start is called before the first frame update
    void Start()
    {
        _powerUpOffset = new Vector3 [] { new Vector3(-505, 18, 0), new Vector3(0, 18, 0), new Vector3(505, 18, 0) };
        _numberOfPowerUps = 3;
        InstantiateNumberOfPowerUps();
    }

    public void InstantiateNumberOfPowerUps()
    {
        if(_powerUpPrefabs.Count < 3 && _notUsedPowerUpPrefabs.Count > 3)
        {
            _powerUpPrefabs.AddRange(_notUsedPowerUpPrefabs);
            _notUsedPowerUpPrefabs.Clear();
        }
        else
        {
            Debug.LogError("There are not enough power ups");
            _numberOfPowerUps = _powerUpPrefabs.Count;
        }

        for(int i = 0; i < _numberOfPowerUps; i++)
        {
            var randomIndex = Random.Range(0, _powerUpPrefabs.Count);
            var powerUp = Instantiate(_powerUpPrefabs[randomIndex], gameObject.transform.position + _powerUpOffset[i], Quaternion.identity, gameObject.transform);
            _notUsedPowerUpPrefabs.Add(_powerUpPrefabs[randomIndex]);
            powerUp.GetComponentInChildren<PermanentPowerup>().Index = _notUsedPowerUpPrefabs.IndexOf(_powerUpPrefabs[randomIndex]);
            _powerUpPrefabs.RemoveAt(randomIndex);
        }
    }
}
