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
    public Vector2Int playerPosInt { get { return new Vector2Int((int)(player.transform.position.x /*- player.transform.position.x < 0 ? 1 : 0*/), (int)(player.transform.position.y + 0.1f)); } }

    public override void defaultAI()
    {
        if (path == null || playerPosInt.x != playerPrevPos.x || playerPosInt.y != playerPrevPos.y)
        {
            Debug.Log("****************************player moved****************************");
            FindPath();
            playerPrevPos = playerPosInt;
        }
        if (path.Count > 0 && Time.time >= nextShootTime)
        {

            instance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            instance.GetComponent<EnemyBehaviorMissile>().mother = this;

            Destroy(instance, destroyTime);

            nextShootTime = Time.time + fireCD;
        }

        
    }

    public override void inflictDamage()
    {

    }

    void FindPath()
    {
        path = FindObjectOfType<PathFinder>().FindPath( new Vector2Int((int)transform.position.x, (int)transform.position.y), new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y));
        
        if(path.Count > 0)
        {
            
            foreach (EnemyBehaviorMissile missile in FindObjectsOfType<EnemyBehaviorMissile>())
            {
                missile.pathChanged = true;
            }

            string pathString = "";
            foreach (Vector2Int node in path)
            {
                pathString += $" ({node.x}, {node.y})";
            }
            Debug.Log(pathString);
        }
        
    }

    public int GetClosestIndex(Vector2 pos)
    {
        int closest = 0;
        float distance = Vector2.Distance(pos, path[closest]);
        for(int i = 1; i < path.Count; i += 1)
        {
            if(Vector2.Distance(pos, path[i]) < distance)
            {
                distance = Vector2.Distance(pos, path[closest]);
                closest = i;
            }
        }

        return closest;
    }
}
