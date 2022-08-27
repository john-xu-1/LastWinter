using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GridNode> GetGridMap(UnityEngine.Tilemaps.Tilemap tilemap, int minX, int minY, int maxX, int maxY)
    {
        List<GridNode> gridMap = new List<GridNode>();
        GridNode[,] gridLayout = new GridNode[maxX - minX + 1, maxY - minY + 1];

        for(int x = minX; x <= maxX; x += 1)
        {
            for (int y = minY; y <= maxY; y += 1)
            {
                if(UtilityTilemap.GetTile(tilemap, new Vector2(x, y)) == null)
                {
                    GridNode node = new GridNode(new Vector2Int(x, y));
                    gridLayout[x - minX, y - minY] = node;
                    gridMap.Add(node);
                }
                
            }
        }

        for (int x = minX; x <= maxX; x += 1)
        {
            for (int y = minY; y <= maxY; y += 1)
            {
                if (x > minX && gridLayout[x - 1, y] != null) gridLayout[x - minX, y - minY].left = gridLayout[x - 1 - minX, y - minY];
                if (x < maxX && gridLayout[x + 1, y] != null) gridLayout[x - minX, y - minY].right = gridLayout[x + 1 - minX, y - minY];
                if (y > minX && gridLayout[x, y - 1] != null) gridLayout[x - minX, y - minY].down = gridLayout[x - minX, y - 1 - minY];
                if (y < maxX && gridLayout[x, y + 1] != null) gridLayout[x - minX, y - minY].up = gridLayout[x - minX, y + 1 - minY];
            }
        }

        return gridMap;
    }

    public List<Vector2Int> FindPath(GridNode start, GridNode end, UnityEngine.Tilemaps.Tilemap tilemap, int minX, int minY, int maxX, int maxY)
    {
        return FindPath(start, end, GetGridMap(tilemap, minX, minY, maxX, maxY));
    }

    public List<Vector2Int> FindPath(GridNode start, GridNode end, List<GridNode> gridMap)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        List<GridNode> visited = new List<GridNode>();

        List<GridNode> frontier = new List<GridNode>();

        List<GridNode> toVisit = new List<GridNode>();

        frontier.Add(start);

        start.cost = 0;

        while (frontier.Count > 0)
        {
            GridNode current = frontier[0];
            frontier.RemoveAt(0);
            visited.Add(current);
            current.GetFrontier(frontier, visited);
            if (current == end) break;
        }

        while (end != start)
        {
            path.Insert(0, end.pos);
            end = end.GetConnectedNeighbor();
        }

        return path;
    }
}

public class GridNode
{
    public int cost = -1;
    public Vector2Int pos;
    public GridNode up, right, down, left;

    public GridNode(Vector2Int pos)
    {
        this.pos = pos;
    }

    public GridNode GetConnectedNeighbor()
    {
        if (up != null && up.cost != -1 && up.cost < cost)
        {
            return up;
        }
        else if (right != null && right.cost != -1 && right.cost < cost)
        {
            return right;
        }
        else if (down != null && down.cost != -1 && down.cost < cost)
        {
            return down;
        }
        else
        {
            return left;
        }
    }

    public void GetFrontier(List<GridNode> frontier, List<GridNode> visited)
    {
        if (up != null && !visited.Contains(up))
        {
            frontier.Add(up);
            up.cost = cost + 1;
        }
        if (right != null && !visited.Contains(right))
        {
            frontier.Add(right);
            right.cost = cost + 1;
        }
        if (down != null && !visited.Contains(down))
        {
            frontier.Add(down);
            down.cost = cost + 1;
        }
        if (left != null && !visited.Contains(left))
        {
            frontier.Add(left);
            left.cost = cost + 1;
        }
    }

}
