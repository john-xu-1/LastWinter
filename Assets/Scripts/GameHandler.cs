using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameHandler : MonoBehaviour
{
    [SerializeField] protected GameObject PlayerPrefab;
    [SerializeField] protected Transform PlayerSpawnPoint;
    public abstract void PlayerDied(GameObject player);
    public abstract void EnemyDied(GameObject enemy);
}
