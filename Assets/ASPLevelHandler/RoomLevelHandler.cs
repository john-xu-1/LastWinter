using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLevelHandler : ASPLevelHandler
{
    [SerializeField] WorldGraphMemory worldGraphMemory;
    [SerializeField] ASPGenerator.RoomGenerator roomGenerator;
    [SerializeField] ASPMap.MapTilemap mapTilemap;
    [SerializeField] ASPMap.MapKeyTileRule mapKeyTileRule;
    [SerializeField] int roomWidth = 32, roomHeight = 18, headroom = 3, shoulderroom = 2, minCeilingHeight = 2;
    [SerializeField] int[] roomIDs;
    [SerializeField] private List<int> roomStack;
    
    // Start is called before the first frame update
    void Start()
    {
        roomIDs = worldGraphMemory.GetRoomIDs("room");
        Debug.Log(roomIDs.Length);
        roomStack = new List<int>(roomIDs);
        roomGenerator.InitializeGenerator(SATISFIABLE, UNSATISFIABLE, TIMEDOUT, ERROR);
        
        buildRoom();
    }

    private void buildRoom()
    {
        int roomID = roomStack[0];
        roomStack.RemoveAt(0);
        worldGraphMemory.SetRoomID(roomID);
        roomGenerator.InitializeGenerator(roomWidth, roomHeight, headroom, shoulderroom, minCeilingHeight);
        roomGenerator.StartGenerator();
    }


    protected override void SATISFIABLE(Clingo.AnswerSet answerSet, string jobID)
    {
        Vector2Int roomDisplacement = worldGraphMemory.GetRoomDisplacement("width");
        mapTilemap.DisplayMap(answerSet, mapKeyTileRule, roomWidth * roomDisplacement.x, roomHeight * (-roomDisplacement.y));
        mapTilemap.AdjustCamera();
        if (roomStack.Count > 0) buildRoom();
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
