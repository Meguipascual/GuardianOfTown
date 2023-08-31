using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/Stages", order = 1)]
public class StageWavesScriptableObjects : ScriptableObject
{
    public List<WaveScriptableObject> _wavesData;
    public int _stage;
}
