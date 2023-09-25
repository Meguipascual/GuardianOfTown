using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/Stages", order = 1)]
public class StageWavesScriptableObjects : ScriptableObject
{
    public List<WaveData> _wavesData;
    public int Stage { get; set; }
}
