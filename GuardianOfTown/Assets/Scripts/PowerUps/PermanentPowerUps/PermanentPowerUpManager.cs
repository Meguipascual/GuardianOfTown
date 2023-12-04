using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PermanentPowerUpManager : MonoBehaviour
{
    public static PermanentPowerUpManager Instance;
    public List<GameObject> _powerUpPrefabs;
    public List<GameObject> _notUsedPowerUpPrefabs;
    [SerializeField] private GameObject _parentGameObject;
    [SerializeField] private Vector3[] _powerUpOffset;
    [SerializeField] private int _numberOfPowerUps;
    [SerializeField] private TextMeshProUGUI _noPowerUpsText;
 
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0) 
        {
            _parentGameObject = FindObjectOfType<PermanentPowerUpParent>(true).gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _powerUpOffset = new Vector3 [] { new Vector3(300, 12, 0), new Vector3(0, 12, 0), new Vector3(-300, 12, 0) };
        _numberOfPowerUps = 3;
    }

    public void ControlNumberOfPowerUps()
    {
        if (_powerUpPrefabs.Count >= 3)
        {
            InstantiateNumberOfPowerUps();
        }
        else if(_powerUpPrefabs.Count < 3 && (_powerUpPrefabs.Count + _notUsedPowerUpPrefabs.Count) > 3)
        {
            _powerUpPrefabs.AddRange(_notUsedPowerUpPrefabs);
            _notUsedPowerUpPrefabs.Clear();
            InstantiateNumberOfPowerUps();
        }
        else
        {
            Debug.LogError("There are not enough power ups");
            _powerUpPrefabs.AddRange(_notUsedPowerUpPrefabs);
            _notUsedPowerUpPrefabs.Clear();

            if (_powerUpPrefabs.Count > 0)
            {
                _numberOfPowerUps = _powerUpPrefabs.Count;
                InstantiateNumberOfPowerUps();
            }
            else
            {
                Debug.LogError("Message should exist");
                Instantiate(_noPowerUpsText, _parentGameObject.transform.position + (_powerUpOffset[2] + new Vector3(-500,0,0)), Quaternion.identity, _parentGameObject.transform);
            }
            
        }
    }

    private void InstantiateNumberOfPowerUps()
    {
        for (int i = 0; i < _numberOfPowerUps; i++)
        {
            var randomIndex = Random.Range(0, _powerUpPrefabs.Count);
            var powerUp = Instantiate(_powerUpPrefabs[randomIndex], _parentGameObject.transform.position + _powerUpOffset[i], Quaternion.identity, _parentGameObject.transform);
            _notUsedPowerUpPrefabs.Add(_powerUpPrefabs[randomIndex]);
            powerUp.GetComponentInChildren<PermanentPowerup>().Index = _notUsedPowerUpPrefabs.IndexOf(_powerUpPrefabs[randomIndex]);
            _powerUpPrefabs.RemoveAt(randomIndex);
        }
    }
}
