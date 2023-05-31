using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public List<Vector2Int> walls = new List<Vector2Int>();
    public bool displayWalls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        foreach(Vector2Int wall in walls)
        {
            Gizmos.DrawCube(new Vector3(wall.x, wall.y, 0), new Vector3(1, 1, 0));
        }
        
    }
}
