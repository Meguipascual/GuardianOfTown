using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Waves", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    public int numberOfEnemiesToCreate;
    public int numberOfBossesToCreate;
    public int LevelOfEnemies;
    public int LevelOfBosses;
}