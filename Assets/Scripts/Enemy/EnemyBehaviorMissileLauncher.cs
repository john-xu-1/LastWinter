using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorMissileLauncher : EnemyBehaviorBase
{
    public GameObject bulletPrefab;
    private float nextShootTime;

    public float fireCD;

    public float destroyTime;

    GameObject instance;

    public List<Vector2Int> path;
    public Vector2Int playerPrevPos;
    public Vector2Int playerPosInt { get { return new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y); } }

    public override void defaultAI()
    {
        if (path == null || (playerPosInt.x != playerPrevPos.x || playerPosInt.y != playerPrevPos.y)) FindPath();
        if (Time.time >= nextShootTime)
        {

            instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            instance.GetComponent<EnemyBehaviorMissile>().mother = this;

            Destroy(instance, destroyTime);

            nextShootTime = Time.time + fireCD;
        }

        playerPrevPos = playerPosInt;
    }

    public override void inflictDamage()
    {

    }

    void FindPath()
    {
        path = FindObjectOfType<PathFinder>().FindPath(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y), new Vector2Int((int)transform.position.x, (int)transform.position.y));
        //path = FindObjectOfType<PathFinder>().FindPath( new Vector2Int((int)transform.position.x, (int)transform.position.y), new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y));
        string pathString = "";
        foreach (Vector2Int node in path)
        {
            pathString += $" ({node.x}, {node.y})";
        }
        Debug.Log(pathString);
    }
}
