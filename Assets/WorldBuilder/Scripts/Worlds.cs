using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace WorldBuilder
{
    [CreateAssetMenu(fileName = "Worlds", menuName = "ASP/Worlds", order = 1)]
    public class Worlds : ScriptableObject
    {
        public string ASPFileName = "Testing";
        [SerializeField]
        public List<Dictionary<string, List<List<string>>>> WorldList = new List<Dictionary<string, List<List<string>>>>();
        public int WorldCount = 0;

        public List<World> BuiltWorlds = new List<World>();


        public void BuildWorld(int worldWidth, int worldHeight, int gateKeyCount, int maxGatePerKey, int startRoom, int timeout)
        {
            string aspCode = WorldMap.bidirectional_rules + WorldMap.test_text + WorldMap.gate_key_rules;
            string path = ClingoUtil.CreateFile(aspCode);
            ClingoSolver solver = FindObjectOfType<ClingoSolver>();
            solver.Solve(path, $" -c max_width={worldWidth} -c max_height={worldHeight} -c start_room={startRoom} -c key_count={gateKeyCount} -c max_gate_type_count={maxGatePerKey}  -t 1 --time-limit={timeout}");
        }

        public void BuildRoom(Vector2Int roomSize, int headroom, int shoulderroom, int minCeilingHeight, RoomConnections connections, Neighbors neighbors)
        {

            //Debug.Log(WorldStructure.max_width + " " + WorldStructure.max_height);
            string aspCode = WorldStructure.get_world_gen(roomSize.x, roomSize.y) + WorldStructure.tile_rules + WorldStructure.get_floor_rules(headroom, shoulderroom) + WorldStructure.get_chamber_rule(minCeilingHeight) + Pathfinding.movement_rules + Pathfinding.platform_rules + Pathfinding.path_rules;

            aspCode += Pathfinding.set_openings(connections.boolArray) + WorldStructure.GetDoorRules(neighbors);

            //if((connections.leftEgress || connections.leftIngress) && neighbors.left != null && !neighbors.left.isDestroyed)
            //{
            //    aspCode += Pathfinding.getPathStartRules("left", neighbors.left);
            //}
            //if ((connections.rightEgress || connections.rightIngress) && neighbors.right != null && !neighbors.right.isDestroyed)
            //{
            //    aspCode += Pathfinding.getPathStartRules("right", neighbors.right);
            //}
            //if ((connections.upEgress || connections.upIngress) && neighbors.up != null && !neighbors.up.isDestroyed)
            //{
            //    aspCode += Pathfinding.getPathStartRules("top", neighbors.up);
            //}
            //if ((connections.downEgress || connections.downIngress) && neighbors.down != null && !neighbors.down.isDestroyed)
            //{
            //    aspCode += Pathfinding.getPathStartRules("bottom", neighbors.down);
            //}
            ClingoSolver solver = FindObjectOfType<ClingoSolver>();

            string path = ClingoUtil.CreateFile(aspCode);
            solver.Solve(path, " -t 4 ");
        }

        public void AddWorld(Dictionary<string, List<List<string>>> newWorld)
        {
            WorldList.Add(newWorld);
            WorldCount = WorldList.Count;
        }

        public void AddWorld(World world)
        {
            BuiltWorlds.Add(world);
        }
    }

}
