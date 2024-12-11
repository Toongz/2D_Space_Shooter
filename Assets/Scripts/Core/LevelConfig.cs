using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    public int level; 
    public float spawnInterval; 
    public SpawnRate[] spawnRates; 

    [System.Serializable]
    public struct SpawnRate
    {
        public int asteroidType; 
        public int percentage;   
    }

    public bool hasBoss; 
    public GameObject bossPrefab;
}
