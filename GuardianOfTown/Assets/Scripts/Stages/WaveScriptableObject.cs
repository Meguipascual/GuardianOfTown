using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Waves", order = 1)]
public class WaveScriptableObject : ScriptableObject
{
    public int numberOfEnemiesToCreate;
    public int numberOfBossesToCreate;
    public int LevelOfEnemies;
    public int LevelOfBosses;
    public int Gate;
}