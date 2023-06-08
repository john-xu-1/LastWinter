using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallChunkDebugger : MonoBehaviour
{
    public static WallChunkDebugger singlton;
    List<Vector2Int> walls = new List<Vector2Int>();
    List<int> wallIDs = new List<int>();
    public Color[] colors;
    public bool displayWalls;

    private void Start()
    {
        singlton = this;
    }
    public void ClearWalls()
    {
        walls.Clear();
    }
    public void SetWalls(List<Vector2Int> walls, bool display)
    {
        this.walls = walls;
        displayWalls = display;
    }
    public void AddWalls(List<Vector2Int> walls, List<int> wallIDs, bool display)
    {
        for(int i = 0; i < walls.Count; i++) {
            if (this.walls.Contains(walls[i])) Debug.LogWarning($"{walls[i]} already in walls.");
            this.walls.Add(walls[i]);
            this.wallIDs.Add(wallIDs[i]);
        }
        displayWalls = display;
    }
    private void OnDrawGizmos()
    {
        if (!displayWalls) return;
        for (int i = 0; i < walls.Count; i++)
        {
            Vector2Int wall = walls[i];
            Color color = colors[wallIDs[i] % colors.Length];
            Gizmos.color = color;
            Gizmos.DrawCube(new Vector3(wall.x+0.5f, -wall.y+0.5f, 0), new Vector3(1, 1, 0));
        }
    }
}
