using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Waves", order = 1)]
public class WaveScriptableObject : ScriptableObject
{
    public int _numberOfEnemiesToCreate;
    public int _numberOfBossesToCreate;
    public int _levelOfEnemies;
    public int _levelOfBosses;
    public int _numberOfEnemiesByGate;
    public int Gate;
    public bool _isRandomized;
}

