using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesData : MonoBehaviour
{
    [SerializeField] private List<StageWavesScriptableObjects> _stagesData;
    public List<StageWavesScriptableObjects> StagesDataList => _stagesData;

    public StageWavesScriptableObjects GenerateRandomStage(int currentStage)
    {
        var numberOfWaves = Random.Range(1, 5);
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
            NumberOfEnemiesToCreate = Random.Range(0, 16);
            NumberOfBossesToCreate = Random.Range(0, 5);
            LevelOfEnemies = LevelOfBosses = Random.Range(0, (5 * currentStage));

            if (Random.Range(0, 4) == 0) 
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


