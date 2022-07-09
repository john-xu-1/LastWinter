using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameHandler : MonoBehaviour
{
    //[SerializeField] protected GameObject MainCamera;
    [SerializeField] protected GameObject playerPrefab;
    public abstract void PlayerDied(GameObject player);
    public abstract void EnemyDied(GameObject enemy);
    [SerializeField] protected Vector3 playerSpawnPoint;
    public abstract void ReAssignPlayer(GameObject player);
}