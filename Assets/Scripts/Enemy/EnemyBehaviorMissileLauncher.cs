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

    public Transform firepoint;

    public List<Vector2Int> path;
    public Vector2Int playerPrevPos;
    public Vector2Int playerPosInt { get { return new Vector2Int((int)(Mathf.Floor(player.transform.position.x) ), (int)(Mathf.Floor(player.transform.position.y/* + 0.1f*/))); } }

    public override void defaultAI()
    {
        if (path == null || (playerPosInt.x != playerPrevPos.x || playerPosInt.y != playerPrevPos.y)) FindPath();
        if (path.Count > 0 && Time.time >= nextShootTime)
        {

            instance = Instantiate(bulletPrefab, firepoint.position, Quaternion.identity);

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
        path = FindObjectOfType<PathFinder>().FindPath(new Vector2Int((int)firepoint.position.x, (int)firepoint.position.y), new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y));
        if (path.Count > 0)
        {
            foreach (EnemyBehaviorMissile missile in FindObjectsOfType<EnemyBehaviorMissile>())
            {
                missile.pathChanged = true;
            }
        }
       
    }

    public int GetClosestIndex(Vector2 pos)
    {
        int closest = 0;
        float distance = Vector2.Distance(pos, path[closest]);
        for (int i = 1; i < path.Count; i += 1)
        {
            if (Vector2.Distance(pos, path[closest]) < distance)
            {
                distance = Vector2.Distance(pos, path[closest]);
                closest = i;
            }
        }

        return closest;
    }
}

