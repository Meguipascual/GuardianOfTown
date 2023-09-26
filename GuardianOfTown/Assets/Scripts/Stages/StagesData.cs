using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesData : MonoBehaviour
{
    [SerializeField] private List<StageWavesScriptableObjects> _stagesData;
    public List<StageWavesScriptableObjects> StagesDataList => _stagesData;

    public void GenerateRandomStage()
    {
        //Tomorrow
    }
}


