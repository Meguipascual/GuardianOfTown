using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesData : MonoBehaviour
{
    [SerializeField] private List<StageWavesScriptableObjects> _stagesData;
    private int _maxOfWaves;
    private int _maxOfEnemies;
    private int _maxOfBosses;
    private int _maxLevelMultiplier;

    public List<StageWavesScriptableObjects> StagesDataList => _stagesData;

    public StageWavesScriptableObjects GenerateRandomStage(int currentStage)
    {
        if (GameSettings.Instance.IsEasyModeActive)
        {
            Debug.Log($"Easy Stage Generated");
            _maxOfWaves = 3;
            _maxOfEnemies = 20;
            _maxOfBosses = 8;
            _maxLevelMultiplier = 6;
        }
        else
        {
            Debug.Log($"Normal Stage Generated");
            _maxOfWaves = 5;
            _maxOfEnemies = 31;
            _maxOfBosses = 10;
            _maxLevelMultiplier = 10; 
        }

        var numberOfWaves = Random.Range(1, _maxOfWaves);
        int NumberOfEnemiesToCreate;
        int NumberOfBossesToCreate;
        int LevelOfEnemies;
        int LevelOfBosses;
        int NumberOfEnemiesByGate = 0;
        int Gate;
        bool IsRandomized;
        List<WaveData> waves = new List<WaveData>();

        for (int i = 0; i < numberOfWaves; i++)
        {
            NumberOfEnemiesToCreate = Random.Range(0, _maxOfEnemies + (currentStage*2));
            if (NumberOfEnemiesToCreate == 0)
            {
                NumberOfBossesToCreate = Random.Range(1, _maxOfBosses + (currentStage * 2));
            }
            else
            {
                NumberOfBossesToCreate = Random.Range(0, _maxOfBosses + (currentStage * 2));
            }

            LevelOfEnemies = LevelOfBosses = Random.Range(2 * currentStage, (_maxLevelMultiplier * currentStage));

            var isRandom = Random.Range(0, 4);

            if (isRandom != 0) 
            {
                IsRandomized = false;
                Gate = Random.Range(0, 4);
            }
            else
            {
                IsRandomized = true;
                Gate = 0;
            }
            WaveData waveData = new WaveData();
            waveData.NumberOfEnemiesToCreate = NumberOfEnemiesToCreate;
            waveData.NumberOfBossesToCreate = NumberOfBossesToCreate;
            waveData.LevelOfEnemies = LevelOfEnemies;
            waveData.LevelOfBosses = LevelOfBosses;
            waveData.NumberOfEnemiesByGate = NumberOfEnemiesByGate;
            waveData.IsRandomized = IsRandomized;
            waveData.Gate = Gate;
            waves.Add(waveData);
        }
        _stagesData[currentStage]._wavesData = waves;
        _stagesData[currentStage].Stage = 0;
        return _stagesData[currentStage];
    }
}


