﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap 
{
    public static void DisplayGraph(Dictionary<string, List<List<string>>> world, GameObject nodePrefab, GameObject edgePrefab)
    {
        int worldWidth = UtilityBuildWorld.Max(world["width"]);
        int worldHeight = UtilityBuildWorld.Max(world["height"]);
        node[,] worldGrid = new node[worldWidth, worldHeight];
        foreach (List<string> room in world["room"])
        {
            int roomID = int.Parse(room[0]);
            Vector2Int index = UtilityBuildWorld.roomID_to_index(roomID, worldWidth, worldHeight);
            node node = GameObject.Instantiate(nodePrefab,new Vector3(index.x+1, worldHeight - index.y, 0),Quaternion.identity).GetComponent<node>();
            node.SetText(roomID);
            worldGrid[index.x, index.y] = node;
        }

        foreach(List<string> door in world["door"])
        {
            Vector2Int sourceIndex = UtilityBuildWorld.roomID_to_index(int.Parse(door[0]), worldWidth, worldHeight);
            Vector2Int destinationIndex = UtilityBuildWorld.roomID_to_index(int.Parse(door[1]), worldWidth, worldHeight);

            if(destinationIndex.x > sourceIndex.x)
            {
                edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1.5f, worldHeight - sourceIndex.y), Quaternion.identity).GetComponent<edge>();
                edge.SetDirection(0);
                worldGrid[sourceIndex.x, sourceIndex.y].rightExit = edge;
            }else if (destinationIndex.x < sourceIndex.x)
            {
                edge edge = GameObject.Instantiate(edgePrefab, new Vector3(destinationIndex.x + 1.5f, worldHeight - sourceIndex.y), Quaternion.identity).GetComponent<edge>();
                edge.SetDirection(180);
                worldGrid[sourceIndex.x, sourceIndex.y].leftExit = edge;
            }
            else if (destinationIndex.y < sourceIndex.y)
            {
                edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1f, worldHeight - destinationIndex.y - 0.5f), Quaternion.identity).GetComponent<edge>();
                edge.SetDirection(90);
                worldGrid[sourceIndex.x, sourceIndex.y].upExit = edge;
            }
            else if (destinationIndex.y > sourceIndex.y)
            {
                edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1f, worldHeight -  sourceIndex.y - 0.5f), Quaternion.identity).GetComponent<edge>();
                edge.SetDirection(270);
                worldGrid[sourceIndex.x, sourceIndex.y].downExit = edge;
            }
        }
        foreach (List<string> gate in world["gate"])
        {
            int key = int.Parse(gate[0]);
            int source = int.Parse(gate[1]);
            int destination = int.Parse(gate[2]);
            Color gateColor = Color.magenta;
            if (key == 1) gateColor = Color.blue;
            else if (key == 2) gateColor = Color.red;
            else if (key == 3) gateColor = Color.yellow;
            Vector2Int sourceIndex = UtilityBuildWorld.roomID_to_index(source, worldWidth, worldHeight);
            worldGrid[sourceIndex.x,sourceIndex.y].SetColor(gateColor);
            if (destination == source + 1)
            {
                worldGrid[sourceIndex.x, sourceIndex.y].rightExit.SetColor(gateColor);
            }else if(destination == source - 1)
            {
                worldGrid[sourceIndex.x, sourceIndex.y].leftExit.SetColor(gateColor);
            }else if(destination > source)
            {
                worldGrid[sourceIndex.x, sourceIndex.y].downExit.SetColor(gateColor);
            }
            else if (destination < source)
            {
                worldGrid[sourceIndex.x, sourceIndex.y].upExit.SetColor(gateColor);
            }
        }

    }

    public static RoomConnections[] get_room_connections(Dictionary<string,List<List<string>>> world_graph)
    {
        int height = UtilityBuildWorld.Max(world_graph["height"]);
        int width = UtilityBuildWorld.Max(world_graph["width"]);
        RoomConnections[] connections = RoomConnections.GetRoomConnectionsArray(height * width + 1); 
        foreach(List<string> door in world_graph["door"])
        {
            int source = int.Parse(door[0]);
            int destination = int.Parse(door[1]);
            if (destination == source + 1) //right
            {
                connections[source].rightEgress = true;
                connections[destination].leftIngress = true;
            }
            else if (destination == source - 1)//left
            {
                connections[source].leftEgress = true;
                connections[destination].rightIngress = true;
            }
            else if (destination < source) //up
            {
                connections[source].upEgress = true;
                connections[destination].downIngress = true;
            }
            else if (destination > source)//down
            {
                connections[source].downEgress = true;
                connections[destination].upIngress = true;
            }

        }
        return connections;
    }

    public static List<string> get_path_start(string path, List<List<string>> map)
    {
        foreach(List<string> node in map)
        {
            if(node.Count == 4 && node[3] == path)
            {
                return node;
            }
        }
        return null;
    }
    public static string get_path_start_rules(string path, List<List<string>> map)
    {
        string rule = "";
        string inversePath = "";

        if (path == "top")
        {
            inversePath = "bottom";
            List<string> node = get_path_start(inversePath, map);
            rule += $@"
                bottom_node :- path({node[0]} -1,_,top,top).
                bottom_node :- path({node[0]} +1,_,top,top).
                :- not bottom_node.
            ";
        }
        else if (path == "bottom")
        {
            inversePath = "top";
            List<string> node = get_path_start(inversePath, map);
            rule += $@"
                top_node :- path({node[0]} -1, _, bottom,bottom).
                top_node :- path({node[0]} +1, _, bottom,bottom).
                :- not top_node.
            ";
        }
        else if (path == "right")
        {
            inversePath = "left";
            List<string> node = get_path_start(inversePath, map);
            rule += $@"
                :- not path(_, {node[1]}, right,right).
            ";
        }
        else if (path == "left")
        {
            inversePath = "right";
            List<string> node = get_path_start(inversePath, map);
            rule += $@"
                :- not path(_, {node[1]}, left,left).
            ";
        }

        return rule;
    }

    public static string test_text = @"
        #const max_width = 4.
        #const max_height = 3.

        #const start_room = 1.


        width(1..max_width).
        height(1..max_height).
        roomID(1..max_width*max_height).
        1{room_grid(XX,YY, ID)}1 :- width(XX), height(YY), ID = (YY - 1) * max_width + XX.
        %1{room(RoomID)}1 :- roomID(RoomID).
        room(ID) :- room_grid(XX,YY,ID).

        %{door(RoomID1, RoomID2)}1 :- room(RoomID1), room(RoomID2).
        {door(RoomID1, RoomID2)}1 :- room_grid(XX,YY, RoomID1), room_grid(XX+1, YY, RoomID2).
        {door(RoomID1, RoomID2)}1 :- room_grid(XX,YY, RoomID1), room_grid(XX-1, YY, RoomID2).
        {door(RoomID1, RoomID2)}1 :- room_grid(XX,YY, RoomID1), room_grid(XX, YY+1, RoomID2).
        {door(RoomID1, RoomID2)}1 :- room_grid(XX,YY, RoomID1), room_grid(XX, YY-1, RoomID2).

        path(RoomID, Type) :- path(RoomIDSource, Type), door(RoomIDSource, RoomID), roomID(Type).
        start(start_room).
        path(RoomID, Type) :- roomID(RoomID), Type = RoomID.
        :- room(RoomID), not path(RoomID, Type), roomID(Type).

    ";

    public static string bidirectional_rules = @"

        door_east_exit(RoomID) :- door(RoomID, NeighboorID), room_grid(XX, YY, RoomID), room_grid(XX + 1, YY, NeighboorID). 
        door_west_exit(RoomID) :- door(RoomID, NeighboorID), room_grid(XX, YY, RoomID), room_grid(XX - 1, YY, NeighboorID).
        door_north_exit(RoomID) :- door(RoomID, NeighboorID), room_grid(XX, YY, RoomID), room_grid(XX, Y2, NeighboorID), Y2<YY.
        door_south_exit(RoomID) :- door(RoomID, NeighboorID), room_grid(XX, YY, RoomID), room_grid(XX, Y2, NeighboorID), Y2 > YY.

        door_east_entrance(RoomID) :- door(NeighboorID, RoomID), room_grid(XX, YY, RoomID), room_grid(XX + 1, YY, NeighboorID). 
        door_west_entrance(RoomID) :- door(NeighboorID, RoomID), room_grid(XX, YY, RoomID), room_grid(XX - 1, YY, NeighboorID).
        door_north_entrance(RoomID) :- door(NeighboorID, RoomID), room_grid(XX, YY, RoomID), room_grid(XX, Y2, NeighboorID), Y2<YY.
        door_south_entrance(RoomID) :- door(NeighboorID, RoomID), room_grid(XX, YY, RoomID), room_grid(XX, Y2, NeighboorID), Y2 > YY.

        door_east(RoomID) :- door_east_exit(RoomID).
        door_east(RoomID) :- door_east_entrance(RoomID).
        door_west(RoomID) :- door_west_exit(RoomID).
        door_west(RoomID) :- door_west_entrance(RoomID).
        door_north(RoomID) :- door_north_exit(RoomID).
        door_north(RoomID) :- door_north_entrance(RoomID).
        door_south(RoomID) :- door_south_exit(RoomID).
        door_south(RoomID) :- door_south_entrance(RoomID).

        door_east_soft_lock(RoomID) :- door_east_exit(RoomID), not door_east_entrance(RoomID).
        door_east_soft_lock(RoomID) :- door_east_entrance(RoomID), not door_east_exit(RoomID).
        door_west_soft_lock(RoomID) :- door_west_exit(RoomID), not door_west_entrance(RoomID).
        door_west_soft_lock(RoomID) :- door_west_entrance(RoomID), not door_west_exit(RoomID).
        door_north_soft_lock(RoomID) :- door_north_exit(RoomID), not door_north_entrance(RoomID).
        door_north_soft_lock(RoomID) :- door_north_entrance(RoomID), not door_north_exit(RoomID).
        door_south_soft_lock(RoomID) :- door_south_exit(RoomID), not door_south_entrance(RoomID).
        door_south_soft_lock(RoomID) :- door_south_entrance(RoomID), not door_south_exit(RoomID).

        door_count(RoomID, Count) :- Count = {door_east(RoomID); door_west(RoomID); door_north(RoomID); door_south(RoomID)}, roomID(RoomID).
        door_soft_lock_count(RoomID, Count) :- Count = {door_east_soft_lock(RoomID); door_west_soft_lock(RoomID); door_north_soft_lock(RoomID); door_south_soft_lock(RoomID)}, roomID(RoomID).
    
    
        %:- door_count(RoomID, Count), roomID(RoomID), Count > 3.
    
        %#show door_count/2.

        %if room has a directional door, it can only have two doors
        :- door_soft_lock_count(RoomID, Count), Count > 0, door_count(RoomID, Count2), Count2 > 2.
        %world must have at least one directional door
        :- {door_soft_lock_count(RoomID, Count) : roomID(RoomID), Count > 0} < 5.
        %neighboring door to a room with directional door cannot have a directional door
        :- door_soft_lock_count(RoomID, Count), Count > 1.

        :- not door_east_soft_lock(_).
        :- not door_west_soft_lock(_).
        :- not door_north_soft_lock(_).
        :- not door_south_soft_lock(_).

    ";

    public static string gate_key_rules = @"
        #const key_count = 3.
        #const max_gate_type_count = 2.

        keys_types(1..key_count).

        1{key(KeyID, RoomID) : roomID(RoomID)}1 :- keys_types(KeyID).
        1 {gate(KeyID, RoomID, RoomIDExit) : door(RoomID, RoomIDExit)} max_gate_type_count :- keys_types(KeyID).

      
        poi(RoomID, Count) :- roomID(RoomID), Count = {key(_, RoomID); start(RoomID); gate(_, RoomID, RoomIDLocked)}.

        %one point of interst per room
        :- poi(RoomID, Count), roomID(RoomID), Count > 1.

        %no point of interest can be next door to another
        :- poi(RoomID, Count), Count > 0, door(RoomID, RoomID2), poi(RoomID2, Count2), Count2 > 0.

        %no gates can have the same room gated
        :- gate(_, RoomID1, RoomID), gate(_, RoomID2, RoomID), RoomID1 != RoomID2.

        path_order(RoomID, T+1) :- door(RoomSourceID, RoomID), path_order(RoomSourceID, T), not gate(_, RoomID, _), T<max_width*max_height.
        path_order(RoomID, T+1) :- door(RoomSourceID, RoomID), path_order(RoomSourceID, T), gate(KeyID, RoomID, _), have_key(KeyID, T2), T>=T2, T<max_width*max_height.

        path_order(RoomID, 0) :- start(RoomID).

        agrogate(min; max;avg).
        path_order(RoomID, min, Min) :- Min = #min{T: path_order(RoomID,T)}, roomID(RoomID).

        %:- 2{path_order(RoomID, _)}, roomID(RoomID).
        :- {path_order(RoomID, _)}0, roomID(RoomID).

        %take the lowest path_order
        %:- path_order(RoomID, T), door(R1, RoomID), path_order(R1, T1), door(R2, RoomID), path_order(R2, T2), T1<T2, not T<T2.
        :- gate(_, RoomSourceID, RoomGatedID), path_order(RoomSourceID, min, T1), path_order(RoomGatedID, min, T2), not T1<T2.

        have_key(KeyID, T) :- path_order(RoomID, T), key(KeyID, RoomID).
        %have_key(KeyID, RoomID, T+1) :- door(RoomSourceID, RoomID), have_key(KeyID, RoomSourceID, T).

    ";
}
