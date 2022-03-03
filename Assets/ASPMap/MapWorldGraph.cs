using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapWorldGraph : ASPMap.Map2D
{
    [SerializeField] WorldBuilder.node nodePrefab;
    [SerializeField] WorldBuilder.edge edgePrefab;
    [SerializeField] float edgeOffset = 0.5f;
    WorldBuilder.node[,] map;

    public override void DisplayMap(Clingo.AnswerSet answerset, ASPMap.ASPMapKey mapKey)
    {
        parseMapDimensions(answerset, mapKey.widthKey, mapKey.heightKey);
        map = new WorldBuilder.node[width, height];

        Clingo.Value dict = answerset.Value;
        handleNodes(dict, (ASPMap.MapKeyPixel)mapKey);
        handleEdges(dict, (ASPMap.MapKeyPixel)mapKey);
        handleGates(dict, (ASPMap.MapKeyPixel)mapKey);
    }

    void handleGates(Clingo.Value dict, ASPMap.MapKeyPixel mapKey)
    {
        foreach (List<string> gate in dict["gate"])
        {
            //gate(KeyID, RoomID, RoomIDExit)
            int keyID = int.Parse(gate[0]);
            int source = int.Parse(gate[1]);
            int destination = int.Parse(gate[2]);
            Vector2Int sourceIndex = WorldBuilder.Utility.roomID_to_index(source, width, height);
            Color gateColor = mapKey.colorDict[keyID.ToString()];
            map[sourceIndex.x, height - 1 - sourceIndex.y].SetColor(gateColor);
            map[sourceIndex.x, height - 1 - sourceIndex.y].SetType("gate");

            Vector2Int destinationIndex = WorldBuilder.Utility.roomID_to_index(destination, width, height);

            if (destination == source + 1)
            {
                map[sourceIndex.x, height - 1 - sourceIndex.y].rightExit.SetColor(gateColor);
                map[destinationIndex.x, height - 1 - destinationIndex.y].removeLeft();
            }
            else if (destination == source - 1)
            {
                map[sourceIndex.x, height - 1 - sourceIndex.y].leftExit.SetColor(gateColor);
                map[destinationIndex.x, height - 1 - destinationIndex.y].removeRight();
            }
            else if (destination > source)
            {
                map[sourceIndex.x, height - 1 - sourceIndex.y].downExit.SetColor(gateColor);
                map[destinationIndex.x, height - 1 - destinationIndex.y].removeUp();
            }
            else if (destination < source)
            {
                map[sourceIndex.x, height - 1 - sourceIndex.y].upExit.SetColor(gateColor);
                map[destinationIndex.x, height - 1 - destinationIndex.y].removeDown();
            }
        }
    }

    void handleEdges(Clingo.Value dict, ASPMap.MapKeyPixel mapKey)
    {
        foreach (List<string> door in dict["door"])
        {
            Vector2Int sourceIndex = WorldBuilder.Utility.roomID_to_index(int.Parse(door[0]), width, height);
            Vector2Int destinationIndex = WorldBuilder.Utility.roomID_to_index(int.Parse(door[1]), width, height);

            float x = sourceIndex.x * TileSpacing;
            float y = (height - sourceIndex.y - 1) * TileSpacing ;
            
            if (destinationIndex.x > sourceIndex.x)
            {
                WorldBuilder.edge edge = Instantiate(edgePrefab, new Vector3(x + edgeOffset, y), Quaternion.identity);
                edge.SetDirection(0);
                edge.transform.parent = transform;
                edge.transform.localPosition = new Vector3(x + edgeOffset, y);
                map[sourceIndex.x, height - 1 - sourceIndex.y].rightExit = edge;
            }
            else if (destinationIndex.x < sourceIndex.x)
            {
                WorldBuilder.edge edge = GameObject.Instantiate(edgePrefab, new Vector3(x - edgeOffset, y), Quaternion.identity);
                edge.SetDirection(180);
                edge.transform.parent = transform;
                edge.transform.localPosition = new Vector3(x - edgeOffset, y);
                map[sourceIndex.x, height - 1 - sourceIndex.y].leftExit = edge;
            }
            else if (destinationIndex.y < sourceIndex.y)
            {
                WorldBuilder.edge edge = GameObject.Instantiate(edgePrefab, new Vector3(x,y + edgeOffset), Quaternion.identity);
                edge.SetDirection(90);
                edge.transform.parent = transform;
                edge.transform.localPosition = new Vector3(x, y + edgeOffset);
                map[sourceIndex.x, height - 1 - sourceIndex.y].upExit = edge;
            }
            else if (destinationIndex.y > sourceIndex.y)
            {
                WorldBuilder.edge edge = GameObject.Instantiate(edgePrefab, new Vector3(x,y-edgeOffset), Quaternion.identity);
                edge.SetDirection(270);
                edge.transform.parent = transform;
                edge.transform.localPosition = new Vector3(x, y - edgeOffset);
                map[sourceIndex.x, height - 1 - sourceIndex.y].downExit = edge;
            }
        }
    }

    void handleNodes(Clingo.Value dict, ASPMap.MapKeyPixel mapKey)
    {
        foreach (List<string> room in dict["room_grid"])
        {
            int x = int.Parse(room[0]) - 1;
            int y = height - int.Parse(room[1]);
            int id = int.Parse(room[2]);
            WorldBuilder.node roomNode = Instantiate(nodePrefab, transform);
            map[x, y] = roomNode;


            roomNode.SetText(id.ToString());
            roomNode.transform.localPosition = new Vector2(x * TileSpacing, y * TileSpacing);


            if (findKey(id, dict) != null)
            {
                List<string> key = findKey(id, dict);
                Color keyColor = mapKey.colorDict[key[0]];
                roomNode.SetColor(keyColor);
                roomNode.SetType("key");
            }

        }

        //start(RoomID)
        int startRoomID = int.Parse(dict["start"][0][0]);
        Debug.Log($"startRoomID: {startRoomID}");
        Vector2Int startIndex = WorldBuilder.Utility.roomID_to_index(startRoomID, width, height);
        map[startIndex.x, height - startIndex.y - 1].SetColor(Color.green);

        //boss_room(RoomID)
        foreach (List<string> bossRoom in dict["boss_room"])
        {
            int bossRoomID = int.Parse(bossRoom[0]);
            Vector2Int bossRoomIndex = WorldBuilder.Utility.roomID_to_index(bossRoomID, width, height);
            map[bossRoomIndex.x,height - 1 - bossRoomIndex.y].SetColor(Color.gray);
        }
    }

    List<string> findGate(int roomID, Clingo.Value dict)
    {
        foreach(List<string> gate in dict["gate"])
        {
            //gate(KeyID, RoomID, RoomIDExit)
            if (int.Parse(gate[1]) == roomID) return gate;
        }
        return null;
    }

    List<string> findKey(int roomID, Clingo.Value dict)
    {
        foreach (List<string> key in dict["key"])
        {
            //key(KeyID, RoomID)
            if (int.Parse(key[1]) == roomID) return key;
        }
        return null;
    }
}
