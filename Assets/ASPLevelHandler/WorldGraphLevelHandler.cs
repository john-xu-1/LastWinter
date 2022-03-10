using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class WorldGraphLevelHandler : ASPLevelHandler
{
    [SerializeField] ASPGenerator.ASPFileGenerator.ASPGeneratorBehavior generatorBehavior;

    [SerializeField] ASPGenerator.WorldGraphGenerator worldGraphGenerator;
    [SerializeField] MapWorldGraph map;
    [SerializeField] ASPMap.MapKeyPixel mapKey;

    // Start is called before the first frame update
    void Start()
    {
        worldGraphGenerator.InitializeGenerator(generatorBehavior);
        initializeGenerator(worldGraphGenerator);
        worldGraphGenerator.StartGenerator();
    }

    protected override void SATISFIABLE(Clingo.AnswerSet answerSet, string jobID)
    {
        map.DisplayMap(answerSet, mapKey);
        map.AdjustCamera();
    }

    protected override void UNSATISFIABLE(string jobID)
    {
        Debug.LogWarning("UNSATISFIABLE unimplemented");
    }

    protected override void TIMEDOUT(int time, string jobID)
    {
        Debug.LogWarning("TIMEDOUT unimplemented");
    }

    protected override void ERROR(string error, string jobID)
    {
        Debug.LogWarning("ERROR unimplemented");
    }


}
