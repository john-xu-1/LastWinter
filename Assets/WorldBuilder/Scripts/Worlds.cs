using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu(fileName = "Worlds", menuName = "ASP/Worlds", order = 1)]
public class Worlds : ScriptableObject
{
    public string ASPFileName = "Testing";
    [SerializeField]
    public List<Dictionary<string, List<List<string>>>> WorldList = new List<Dictionary<string, List<List<string>>>>();
    public int WorldCount = 0;
    public void BuildWorld(int worldWidth, int worldHeight, int gateKeyCount, int maxGatePerKey, int startRoom, int timeout)
    {
        string aspCode = WorldMap.bidirectional_rules + WorldMap.test_text + WorldMap.gate_key_rules;
        string path = ClingoUtil.CreateFile(aspCode);
        ClingoSolver solver = FindObjectOfType<ClingoSolver>();
        solver.Solve(path, $" -c max_width={worldWidth} -c max_height={worldHeight} -c start_room={startRoom} -c key_count={gateKeyCount} -c max_gate_type_count={maxGatePerKey}  -t 1 --time-limit={timeout}");
    }

    public void BuildRoom(Vector2Int roomSize, int headroom, int shoulderroom, int minCeilingHeight, bool[] connections, Neighbors neighbors) 
    {
        
        //Debug.Log(WorldStructure.max_width + " " + WorldStructure.max_height);
        string aspCode = WorldStructure.get_world_gen(roomSize.x,roomSize.y) + WorldStructure.tile_rules + WorldStructure.get_floor_rules(headroom,shoulderroom) + WorldStructure.get_chamber_rule(minCeilingHeight) + Pathfinding.movement_rules + Pathfinding.platform_rules + Pathfinding.path_rules;

        aspCode += Pathfinding.set_openings(connections) + WorldStructure.GetDoorRules(neighbors);
        
        ClingoSolver solver = FindObjectOfType<ClingoSolver>();
       
        string path = ClingoUtil.CreateFile(aspCode);
        solver.Solve(path, " -t 4 ");
    }

    public void AddWorld(Dictionary<string, List<List<string>>> newWorld)
    {
        WorldList.Add(newWorld);
        WorldCount = WorldList.Count;
    }
}

public class Neighbors
{
    public Room left, right, up, down;
}
