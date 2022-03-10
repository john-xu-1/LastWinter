using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLevelHandler : ASPLevelHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void SATISFIABLE(Clingo.AnswerSet answerSet, string jobID)
    {
        Debug.LogWarning("SATISFIABLE unimplemented");
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
