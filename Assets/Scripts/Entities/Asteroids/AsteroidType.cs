using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidType", menuName = "Game/Asteroid Type")]
public class AsteroidType : ScriptableObject
{
    public string asteroidName;
    public GameObject prefab;
    public int baseHealth;
    public float speed;
}
