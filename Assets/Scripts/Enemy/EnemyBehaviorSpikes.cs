using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBehaviorSpikes : EnemyBehaviorBase
{
    public bool isAttack = false;

    public Tilemap tmap;

    public TileBase tb;

    public float yOffset = -1;

    public int xIncrement = 1;

    int dir;

    List<Vector3Int> POSs = new List<Vector3Int>();

    protected override void ChildStart()
    {
        tmap = FindObjectOfType<CollisionMap>().tilemap;
    }

    public override void defaultAI()
    {
        Vector3 target = p.transform.position;
        Vector2 direction = (target - transform.position).normalized;




        if (direction.x > 0)
        {
            dir = 1;
        }
        else if (direction.x < 0)
        {
            dir = -1;
        }


        if (Vector2.Distance(transform.position, p.transform.position) <= angerRange)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }

        if (isAttack)
        {
            
            for (int i = 1; i < attackRange; i += xIncrement)
            {
                TileBase curTile = UtilityTilemap.GetTile(tmap, new Vector2(transform.  position.x + i, p.transform.position.y + yOffset));

                if (curTile == null)
                {
                    Vector3Int pos = tmap.WorldToCell(new Vector2(transform.position.x + i * dir, p.transform.position.y + yOffset));
                    UtilityTilemap.PlaceTile(tmap, pos, tb);

                    POSs.Add(pos);

                }
                
                

            }
        }
        else
        {
            rb.velocity = new Vector2(direction.x, rb.velocity.y);

            if (POSs.Count > 0)
            {
                for (int i = 0; i < POSs.Count; i++)
                {
                    UtilityTilemap.DestroyTile(tmap, POSs[i]);
                }

                POSs.Clear();
            }
            

            
        }

    }
}
