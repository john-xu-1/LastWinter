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
    public void BuildWorld()
    {
        string aspCode = WorldStructure.world_gen + WorldStructure.tile_rules + WorldStructure.floor_rules + WorldStructure.chamber_rule + Pathfinding.movement_rules + Pathfinding.platform_rules + Pathfinding.path_rules;
        
        File.WriteAllText("Assets/Resources/ASPFiles/" + ASPFileName + ".txt", aspCode);

        TextAsset aspFile = Resources.Load<TextAsset>("ASPFiles/" + ASPFileName);
        //TextAsset aspFile = AssetDatabase.;
        Debug.Log(aspFile.name);
        ClingoSolver solver = FindObjectOfType<ClingoSolver>();
        //AssetDatabase.CreateAsset(aspFile, "Assets/ASP/ASPFiles/" + ASPFileName + ".txt");
        solver.aspFile = aspFile;
        
        solver.solveUsingThread();
    }

    public void AddWorld(Dictionary<string, List<List<string>>> newWorld)
    {
        WorldList.Add(newWorld);
        WorldCount = WorldList.Count;
    }
}
