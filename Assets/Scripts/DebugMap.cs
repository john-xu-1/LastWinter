using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DebugMap : MonoBehaviour
{
    public MapGenerator Generator;
    public int[][] MyMap;

    [TextArea(5, 10)]
    public string Bitmap;

    public string JsonMapName = "test2.txt";
    public TextAsset jsonFile;
    public bool BuildOnStart;
    public ClingoSolver Solver;
    public Worlds WorldBuilder;
    public enum MapSources
    {
        Json,
        Solver
    }
    public MapSources MapSource;

    private void Start()
    {

        if (BuildOnStart) buildMap();
    }
    bool isBuilt = false;
    private void Update()
    {
        if(MapSource == MapSources.Solver && Solver.isSolved && !isBuilt)
        {
            WorldBuilder.AddWorld(Solver.answerSet);
            Generator.ConvertMap(Solver.answerSet);
            isBuilt = true;
        }
    }

    private void buildMap()
    {
        if(MapSource == MapSources.Json)
        {
            //TextAsset jsonFile = Resources.Load<TextAsset>(JsonMapName);
            print(jsonFile);
            string jsonStr = jsonFile.text;
            Map map = JsonUtility.FromJson<Map>(jsonStr);
            print(map.dimensions.room_count_height);
            Generator.ConvertMap(map);
        }else if(MapSource == MapSources.Solver)
        {
            //Generator.ConvertMap(Solver.answerSet);

            WorldBuilder.BuildWorld();
        }
        
    }

    
}
