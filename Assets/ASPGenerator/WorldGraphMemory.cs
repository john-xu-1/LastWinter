using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldGraphMemory", menuName = "ASP/ASPMemory/WorldGraphMemory")]
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
        if (answerSet.Value.RawValue.Count <= 0) setup(worldGraphJson);
        Debug.Log($"roomConnections.Length {roomConnections.Length}");
        return WorldBuilder.Pathfinding.set_openings(roomConnections[roomID].boolArray);
    }

    public int[] GetRoomIDs(string roomIDKey)
    {
        List<int> roomIDs = new List<int>();
        if (answerSet.Value.RawValue.Count <= 0) setup(worldGraphJson);
        foreach (List<string> room in answerSet.Value[roomIDKey])
        {
            roomIDs.Add(int.Parse(room[0]));
        }
        return roomIDs.ToArray();
    }

    public Vector2Int GetRoomDisplacement(/*int roomID, */string widthKey/*, string heightKey*/)
    {
        int width = int.MinValue;
        
        foreach (List<string> widths in answerSet.Value[widthKey])
        {
            if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
        }
        
        int y = (roomID - 1) / width;
        int x = (roomID - 1) % width;
        return new Vector2Int(x, y);
    }

    public void SetRoomID(int roomID)
    {
        this.roomID = roomID;
    }

}
