using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WorldGraphMemory", menuName = "ASP/ASPMemory/WorldGraphMemory")]
public class WorldGraphMemory : ASPMemory
{
    [SerializeField] string worldGraphJson;
    Clingo.AnswerSet answerSet;
    WorldBuilder.RoomConnections[] roomConnections;
    [SerializeField] int roomID = 1;

    public WorldGraphMemory(Clingo.AnswerSet worldGraphAnswerSet)
    {
        setup(worldGraphAnswerSet);
    }
    public WorldGraphMemory(string worldGraphJson)
    {
        setup(worldGraphJson);
    }
    protected virtual void setup(string worldGraphJson)
    {
        string answersetJson = WorldBuilder.SaveUtility.GetFile(WorldBuilder.SaveUtility.DataFilePath + "/" + worldGraphJson);
        Clingo.AnswerSet answerSet = JsonUtility.FromJson<Clingo.AnswerSet>(answersetJson);
        setup(answerSet);
    }

    protected virtual void setup(Clingo.AnswerSet worldGraphAnswerSet)
    {
        answerSet = worldGraphAnswerSet;
        roomConnections = WorldBuilder.WorldMap.get_room_connections(worldGraphAnswerSet);
    }

    protected override string getASPCode()
    {
        //if (answerSet == null)
            setup(worldGraphJson);
        Debug.Log($"roomConnections.Length {roomConnections.Length}");
        return WorldBuilder.Pathfinding.set_openings(roomConnections[roomID].boolArray);
    }

}
