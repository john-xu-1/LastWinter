using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading;

public class PathFinder : MonoBehaviour
{
    List<GridNode> gridMap;
    public Tilemap tilemap;
    public int minX = 0, minY = -160, maxX = 160, maxY = 0;
    public Dictionary<Vector4, List<Vector2Int>> paths = new Dictionary<Vector4, List<Vector2Int>>();

    private Thread findPathThread;

    private void OnDestroy()
    {
        findingPath = false;
        findPathThread.Abort();
    }

    public bool debugPathfinder = false;
    private void Start()
    {
        if(debugPathfinder)SetMap(minX, minY, maxX, maxY);
        findPathThread = new Thread(FindPathThread);
        findPathThread.Start();

    }

    public void SetMap (int minX, int minY, int maxX, int maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        gridMap = GetGridMap(tilemap, minX, minY, maxX, maxY);
    }

    public List<GridNode> GetGridMap(Tilemap tilemap, int minX, int minY, int maxX, int maxY)
    {
        if (this.gridMap != null) return this.gridMap;
        Debug.Log("GetGridMap");
        gridMap = new List<GridNode>();
        GridNode[,] gridLayout = new GridNode[maxX - minX + 1, maxY - minY + 1];

        for (int x = minX; x <= maxX; x += 1)
        {
            for (int y = minY; y <= maxY; y += 1)
            {
                if (UtilityTilemap.GetTile(tilemap, new Vector2(x, y)) == null)
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
                if (gridLayout[x - minX, y - minY] != null && x > minX && gridLayout[x - 1 - minX, y - minY] != null)
                    gridLayout[x - minX, y - minY].left = gridLayout[x - 1 - minX, y - minY];
                if (gridLayout[x - minX, y - minY] != null && x < maxX && gridLayout[x + 1 - minX, y - minY] != null)
                    gridLayout[x - minX, y - minY].right = gridLayout[x + 1 - minX, y - minY];
                if (gridLayout[x - minX, y - minY] != null && y > minY && gridLayout[x - minX, y - 1 - minY] != null)
                    gridLayout[x - minX, y - minY].down = gridLayout[x - minX, y - 1 - minY];
                if (gridLayout[x - minX, y - minY] != null && y < maxY && gridLayout[x - minX, y + 1 - minY] != null)
                    gridLayout[x - minX, y - minY].up = gridLayout[x - minX, y + 1 - minY];
            }
        }

        return gridMap;
    }
    public void FindPath(Vector2Int start, Vector2Int end)
    {
        Vector4 pathID = new Vector4(start.x,start.y,end.x,end.y);
        if (!pathIDs.Contains(pathID)) pathIDs.Add(pathID);

    }
    public bool findingPath = true;
    public List<Vector4> pathIDs = new List<Vector4>();
    private void FindPathThread()
    {
        while (findingPath)
        {
            if (pathIDs.Count > 0)
            {
                Vector4 pathID = pathIDs[0];
                pathIDs.RemoveAt(0);
                List<Vector2Int> path = FindPath(new Vector2Int((int)pathID.x,(int)pathID.y), new Vector2Int ((int)pathID.z,(int)pathID.w), tilemap, minX, minY, maxX, maxY);
                /*if (path.Count > 0) */paths[pathID] = path;

                
            }
            
        }
        
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Tilemap tilemap, int minX, int minY, int maxX, int maxY)
    {
        GridNode startNode = null;
        GridNode endNode = null;
        List<GridNode> gridMap = GetGridMap(tilemap, minX, minY, maxX, maxY);
        foreach (GridNode gridNode in gridMap)
        {
            if (gridNode.pos.x == start.x && gridNode.pos.y == start.y) startNode = gridNode;
            if (gridNode.pos.x == end.x && gridNode.pos.y == end.y) endNode = gridNode;
            //else if (gridNode == null && gridNode.pos.x == end.x - 1 && gridNode.pos.y == end.y) endNode = gridNode;
            //else if (gridNode == null && gridNode.pos.x == end.x + 1 && gridNode.pos.y == end.y) endNode = gridNode;
        }
        if (startNode == null || endNode == null) return new List<Vector2Int>();
        return FindPath(startNode, endNode, gridMap);
        //if (startNode != null && endNode != null) FindPath(startNode, endNode, gridMap);
    }


    

    public List<Vector2Int> FindPath(GridNode start, GridNode end, List<GridNode> gridMap)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        List<GridNode> visited = new List<GridNode>();

        List<GridNode> frontier = new List<GridNode>();

        List<GridNode> toVisit = new List<GridNode>();
        
        frontier.Add(start);

        start.cost = 0;
        int loops = 0;
        bool targetFound = false;
        while (frontier.Count > 0 && loops < 1000)
        {
            loops += 1;
            GridNode current = frontier[0];
            frontier.RemoveAt(0);
            visited.Add(current);
            current.GetFrontier(frontier, visited, end.pos);
            if (current == end)
            {
                targetFound = true;
                break;
            }
        }
        Debug.Log($"loops: {loops}");

        while (targetFound && end != start)
        {
            path.Insert(0, end.pos);
            //path.Add(end.pos);
            end = end.GetConnectedNeighbor();
        }

        return path;
    }
}

public class GridNode
{
    public int cost = -1;
    public float distance;
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

    public void GetFrontier(List<GridNode> frontier, List<GridNode> visited, Vector2Int target)
    {
        if (up != null && !visited.Contains(up))
        {
            up.cost = cost + 1;
            up.distance = Vector2.Distance(up.pos, target);
            AddToFrontier(frontier, up);
        }
        if (right != null && !visited.Contains(right))
        {
            right.cost = cost + 1;
            right.distance = Vector2.Distance(right.pos, target);
            AddToFrontier(frontier, right);
        }
        if (down != null && !visited.Contains(down))
        {
            down.cost = cost + 1;
            down.distance = Vector2.Distance(down.pos, target);
            AddToFrontier(frontier, down);
        }
        if (left != null && !visited.Contains(left))
        {
            left.cost = cost + 1;
            left.distance = Vector2.Distance(left.pos, target);
            AddToFrontier(frontier, left);
        }
    }

    private void AddToFrontier(List<GridNode> frontier, GridNode node)
    {
        for (int i = 0; i < frontier.Count; i += 1)
        {
            if (node.distance /*+ node.cost*/ < frontier[i].distance /*+ frontier[i].cost*/)
            {
                frontier.Insert(i, node);
                break;
            }
        }
        frontier.Add(node);
    }
}

