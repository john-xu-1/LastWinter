using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallChunkDebugger : MonoBehaviour
{
    public static WallChunkDebugger singlton;
    List<Vector2Int> walls = new List<Vector2Int>();
    public bool displayWalls;

    private void Start()
    {
        singlton = this;
    }

    public void SetWalls(List<Vector2Int> walls, bool display)
    {
        this.walls = walls;
        displayWalls = display;
    }
    private void OnDrawGizmos()
    {
        foreach (Vector2Int wall in walls)
        {
            Gizmos.DrawCube(new Vector3(wall.x, wall.y, 0), new Vector3(1, 1, 0));
        }
    }
}
