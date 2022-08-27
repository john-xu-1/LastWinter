using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
