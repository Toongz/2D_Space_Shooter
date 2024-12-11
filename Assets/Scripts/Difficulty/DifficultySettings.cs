using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Game/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public float timeToIncreaseDifficulty = 10f; 
    public int asteroidBaseHealth = 1;
    public int asteroidHealthIncrease = 1;
    public int bossBaseHealth = 10;
    public int bossHealthIncrease = 5;
}
