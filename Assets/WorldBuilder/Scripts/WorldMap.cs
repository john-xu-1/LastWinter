using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class WorldMap
    {
        public static Graph ConvertGraph(Dictionary<string, List<List<string>>> world)
        {
            Graph graph = new Graph();
            //Door[] doors;
            //Key[] keys;
            //Gate[] gates;
            //int startRoomID;
            //int width;
            //int height;

            graph.startRoomID = int.Parse(world["start"][0][0]);
            graph.width = Utility.Max(world["width"]);
            graph.height = Utility.Max(world["height"]);

            List<Door> doors = new List<Door>();
            foreach(List<string> door in world["door"])
            {
                Door newDoor = new Door();
                newDoor.name = door[0] + "->" + door[1];
                newDoor.source = int.Parse(door[0]);
                newDoor.destination = int.Parse(door[1]);
                doors.Add(newDoor);
            }
            graph.doors = Utility.GetArray(doors);

            List<Key> keys = new List<Key>();
            foreach (List<string> key in world["key"])
            {
                Key newKey = new Key();
                newKey.type = int.Parse(key[0]);
                newKey.roomID = int.Parse(key[1]);
                keys.Add(newKey);
            }
            graph.keys = Utility.GetArray(keys);

            List<Gate> gates = new List<Gate>();
            foreach(List<string> gate in world["gate"])
            {
                Gate newGate = new Gate();
                newGate.type = int.Parse(gate[0]);
                newGate.source = int.Parse(gate[1]);
                newGate.destination = int.Parse(gate[2]);
                gates.Add(newGate);
            }
            graph.gates = Utility.GetArray(gates);
            return graph;
        }
        public static void DisplayGraph(Graph world, GameObject nodePrefab, GameObject edgePrefab, Transform miniMap)
        {
            int worldWidth = world.width;
            int worldHeight = world.height;
            node[,] worldGrid = new node[worldWidth, worldHeight];
            for (int roomID = 1; roomID <= worldWidth * worldHeight; roomID += 1)
            {
                Vector2Int index = Utility.roomID_to_index(roomID, worldWidth, worldHeight);
                node node = GameObject.Instantiate(nodePrefab, new Vector3(index.x + 1, worldHeight - index.y, 0), Quaternion.identity).GetComponent<node>();
                node.transform.parent = miniMap;
                node.transform.localPosition = new Vector3(index.x + 1, worldHeight - index.y, 0);
                node.SetText(roomID);
                worldGrid[index.x, index.y] = node;
            }

            foreach (Door door in world.doors)
            {

                Vector2Int sourceIndex = Utility.roomID_to_index(door.source, worldWidth, worldHeight);
                Vector2Int destinationIndex = Utility.roomID_to_index(door.destination, worldWidth, worldHeight);

                if (destinationIndex.x > sourceIndex.x)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1.5f, worldHeight - sourceIndex.y), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(0);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(sourceIndex.x + 1.5f, worldHeight - sourceIndex.y);
                    worldGrid[sourceIndex.x, sourceIndex.y].rightExit = edge;
                }
                else if (destinationIndex.x < sourceIndex.x)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(destinationIndex.x + 1.5f, worldHeight - sourceIndex.y), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(180);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(destinationIndex.x + 1.5f, worldHeight - sourceIndex.y);
                    worldGrid[sourceIndex.x, sourceIndex.y].leftExit = edge;
                }
                else if (destinationIndex.y < sourceIndex.y)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1f, worldHeight - destinationIndex.y - 0.5f), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(90);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(sourceIndex.x + 1f, worldHeight - destinationIndex.y - 0.5f);
                    worldGrid[sourceIndex.x, sourceIndex.y].upExit = edge;
                }
                else if (destinationIndex.y > sourceIndex.y)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1f, worldHeight - sourceIndex.y - 0.5f), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(270);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(sourceIndex.x + 1f, worldHeight - sourceIndex.y - 0.5f);
                    worldGrid[sourceIndex.x, sourceIndex.y].downExit = edge;
                }
            }
            foreach (Gate gate in world.gates)
            {
                int key = gate.type;
                int source = gate.source;
                int destination = gate.destination;
                Color gateColor = Color.magenta;
                if (key == 1) gateColor = Color.blue;
                else if (key == 2) gateColor = Color.red;
                else if (key == 3) gateColor = Color.yellow;
                else if (key == 4) gateColor = Color.cyan;
                else if (key == 5) gateColor = Color.gray;
                Vector2Int sourceIndex = Utility.roomID_to_index(source, worldWidth, worldHeight);
                worldGrid[sourceIndex.x, sourceIndex.y].SetColor(gateColor);
                worldGrid[sourceIndex.x, sourceIndex.y].SetType("gate");

                if (destination == source + 1)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].rightExit.SetColor(gateColor);
                }
                else if (destination == source - 1)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].leftExit.SetColor(gateColor);
                }
                else if (destination > source)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].downExit.SetColor(gateColor);
                }
                else if (destination < source)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].upExit.SetColor(gateColor);
                }

            }

            foreach(Key key in world.keys)
            {
                Debug.Log(key.roomID + " " + key.type);
                int keyType = key.type;
                int roomID = key.roomID;
                Color gateColor = Color.magenta;
                if (keyType == 1) gateColor = Color.blue;
                else if (keyType == 2) gateColor = Color.red;
                else if (keyType == 3) gateColor = Color.yellow;
                else if (keyType == 4) gateColor = Color.cyan;
                else if (keyType == 5) gateColor = Color.gray;
                Vector2Int sourceIndex = Utility.roomID_to_index(roomID, worldWidth, worldHeight);
                worldGrid[sourceIndex.x, sourceIndex.y].SetColor(gateColor);
                worldGrid[sourceIndex.x, sourceIndex.y].SetType("key");
            }

            int startRoomID = world.startRoomID;
            Vector2Int startIndex = Utility.roomID_to_index(startRoomID, worldWidth, worldHeight);
            worldGrid[startIndex.x, startIndex.y].SetColor(Color.green);

            
        }
        public static void DisplayGraph(Dictionary<string, List<List<string>>> world, GameObject nodePrefab, GameObject edgePrefab, Transform miniMap)
        {
            int worldWidth = Utility.Max(world["width"]);
            int worldHeight = Utility.Max(world["height"]);
            node[,] worldGrid = new node[worldWidth, worldHeight];
            foreach (List<string> room in world["room"])
            {
                int roomID = int.Parse(room[0]);
                Vector2Int index = Utility.roomID_to_index(roomID, worldWidth, worldHeight);
                node node = GameObject.Instantiate(nodePrefab, new Vector3(index.x + 1, worldHeight - index.y, 0), Quaternion.identity).GetComponent<node>();
                node.transform.parent = miniMap;
                node.transform.localPosition = new Vector3(index.x + 1, worldHeight - index.y, 0);
                node.SetText(roomID);
                worldGrid[index.x, index.y] = node;
            }

            foreach (List<string> door in world["door"])
            {
                Vector2Int sourceIndex = Utility.roomID_to_index(int.Parse(door[0]), worldWidth, worldHeight);
                Vector2Int destinationIndex = Utility.roomID_to_index(int.Parse(door[1]), worldWidth, worldHeight);

                if (destinationIndex.x > sourceIndex.x)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1.5f, worldHeight - sourceIndex.y), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(0);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(sourceIndex.x + 1.5f, worldHeight - sourceIndex.y);
                    worldGrid[sourceIndex.x, sourceIndex.y].rightExit = edge;
                }
                else if (destinationIndex.x < sourceIndex.x)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(destinationIndex.x + 1.5f, worldHeight - sourceIndex.y), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(180);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(destinationIndex.x + 1.5f, worldHeight - sourceIndex.y);
                    worldGrid[sourceIndex.x, sourceIndex.y].leftExit = edge;
                }
                else if (destinationIndex.y < sourceIndex.y)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1f, worldHeight - destinationIndex.y - 0.5f), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(90);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(sourceIndex.x + 1f, worldHeight - destinationIndex.y - 0.5f);
                    worldGrid[sourceIndex.x, sourceIndex.y].upExit = edge;
                }
                else if (destinationIndex.y > sourceIndex.y)
                {
                    edge edge = GameObject.Instantiate(edgePrefab, new Vector3(sourceIndex.x + 1f, worldHeight - sourceIndex.y - 0.5f), Quaternion.identity).GetComponent<edge>();
                    edge.SetDirection(270);
                    edge.transform.parent = miniMap;
                    edge.transform.localPosition = new Vector3(sourceIndex.x + 1f, worldHeight - sourceIndex.y - 0.5f);
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
                else if (key == 4) gateColor = Color.cyan;
                else if (key == 5) gateColor = Color.gray;
                Vector2Int sourceIndex = Utility.roomID_to_index(source, worldWidth, worldHeight);
                worldGrid[sourceIndex.x, sourceIndex.y].SetColor(gateColor);
                if (destination == source + 1)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].rightExit.SetColor(gateColor);
                }
                else if (destination == source - 1)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].leftExit.SetColor(gateColor);
                }
                else if (destination > source)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].downExit.SetColor(gateColor);
                }
                else if (destination < source)
                {
                    worldGrid[sourceIndex.x, sourceIndex.y].upExit.SetColor(gateColor);
                }
            }

        }
        public static bool[] GetConnections(RoomConnections connection)
        {
            bool[] boolConnection = new bool[8];
            boolConnection[0] = connection.upEgress;
            boolConnection[1] = connection.upIngress;
            boolConnection[2] = connection.rightEgress;
            boolConnection[3] = connection.rightIngress;
            boolConnection[4] = connection.downEgress;
            boolConnection[5] = connection.downIngress;
            boolConnection[6] = connection.leftEgress;
            boolConnection[7] = connection.leftIngress;
            return boolConnection;
        }

        public static RoomConnections[] get_room_connections(Dictionary<string, List<List<string>>> world_graph)
        {
            int height = Utility.Max(world_graph["height"]);
            int width = Utility.Max(world_graph["width"]);
            RoomConnections[] connections = RoomConnections.GetRoomConnectionsArray(height * width + 1);
            foreach (List<string> door in world_graph["door"])
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
            foreach (List<string> node in map)
            {
                if (node.Count == 4 && node[3] == path)
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
            %start(start_room).

            1{start(RoomID): room_grid(_,YY,RoomID), YY == 1}1.
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


            door_soft_locked(Source, Destination) :- door(Source, Destination), not door(Destination, Source).
    
            %:- door_count(RoomID, Count), roomID(RoomID), Count > 3.
    
            %#show door_count/2.

        %if room has a directional door, it can only have two doors
            :- door_soft_lock_count(RoomID, Count), Count > 0, door_count(RoomID, Count2), Count2 > 2.

        %world must have at least one directional door
            %:- {door_soft_lock_count(RoomID, Count) : roomID(RoomID), Count > 0} < 5.

        %neighboring door to a room with directional door cannot have a directional door
            :- door_soft_lock_count(RoomID, Count), Count > 1.

            %:- not door_east_soft_lock(_).
            %:- not door_west_soft_lock(_).
            %:- not door_north_soft_lock(_).
            %:- not door_south_soft_lock(_).

        ";

        public static string gate_key_rules = @"
            #const key_count = 3.
            #const max_gate_type_count = 2.

            keys_types(1..key_count).

            1{key(KeyID, RoomID) : roomID(RoomID)}1 :- keys_types(KeyID).
            min_gate_type_count {gate(KeyID, RoomID, RoomIDExit) : door(RoomID, RoomIDExit)} max_gate_type_count :- keys_types(KeyID).

      
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


        %% gated area --------------------------

            :- room(RoomID), {gated_order(RoomID,_); non_gated(RoomID); gated_gate(RoomID,_); gated_key(RoomID,_)} != 1.

        %% gated_order
            %gated_order(RoomID, GateRoomID) :- gate(KeyID,_,RoomID), GateRoomID = RoomID, not gated_key(RoomID,KeyRoomID), key(KeyID,KeyRoomID).
            gated_order(RoomID, GateRoomID) :- gate(KeyID,GateRoomID,RoomID), not gated_key(GateRoomID,KeyRoomID), key(KeyID,KeyRoomID).
            %gated_order(RoomID, GateRoomID) :- gate(KeyID,GateRoomID,RoomID).

            gated_order(RoomID, GateRoomID) :- gated_order(Source,GateRoomID), door(Source, RoomID), not door_soft_locked(Source, RoomID), not gate(_,RoomID,_).
            %gated_order(RoomID, GateRoomID) :- door(Source, RoomID), gated_order(Source,GateRoomID), gate(_,RoomID,GatedID), GateRoomID != RoomID, GatedID != Source.

        %% gated gate
            gated_gate(RoomID, GateRoomID) :- door(Source, RoomID), gated_order(Source,GateRoomID), gate(_,RoomID,GatedRoom), GateRoomID != RoomID, Source != GatedRoom.
            :- gate(_,GateRoomID,RoomID), not gated_order(RoomID, GateRoomID), not non_gated(RoomID).

        %% gated key
            gated_key(RoomID, KeyRoomID) :- key(_,RoomID), KeyRoomID = RoomID.
            gated_key(RoomID, KeyRoomID) :- gated_key(Source, KeyRoomID), door(Source,RoomID), not gate(KeyID,Source,RoomID), key(KeyID, KeyRoomID).

            gated_key_check(RoomID, KeyRoomID) :- gated_key(RoomID, KeyRoomID), gate(KeyID,RoomID,_), key(KeyID, KeyRoomID).
            gated_key_check(RoomID, KeyRoomID) :- gated_key_check(Source,KeyRoomID), door(Source, RoomID), not gate(_,Source, RoomID).
            %gated_key_check(RoomID, KeyRoomID) :- gated_key_check(Source,KeyRoomID), door(Source, RoomID), not gate(_,Source, RoomID), key(KeyID, KeyRoomID), not gate_order(_,KeyID).
            %gated_key_check(RoomID, KeyRoomID) :- gated_key_check(Source,KeyRoomID), door(Source, RoomID), gate(_,Source, RoomID), key(KeyID, KeyRoomID), gate_order(_,KeyID).
            :- gated_key(RoomID, KeyRoomID), not gated_key_check(RoomID,KeyRoomID).


            %gated_area(RoomID) :- gate(_,Source,RoomID), gated_gate(Source, _).
            %gated_area(RoomID) :- gate(_,Source,RoomID), gated_order(RoomID,_).
            %gated_area(RoomID) :- door_soft_locked(RoomID,_).
            %gated_area(RoomID) :- door_soft_locked(_,RoomID).
            %gated_area(RoomID) :- gate(_,RoomID,_), gated_key(RoomID,_).

            gated_area(RoomID) :- gated_order(RoomID,_).
            gated_area(RoomID) :- gated_gate(RoomID,_).
            gated_area(RoomID) :- gated_key(RoomID,_).

            :- door_soft_locked(Source, Dest), non_gated(Source), non_gated(Dest).
            :- door_soft_locked(Source, Dest), gated_order(Source, GateRoomID), gated_order(Dest, GateRoomID).
            :- door_soft_locked(Source, Dest), gated_key(Source, KeyRoomID), gated_key(Dest, KeyRoomID).
            
        %% not gated
            non_gated(RoomID) :- start(RoomID).
            %non_gated(RoomID) :- door(Source, RoomID), non_gated(Source), not gate(_,Source, RoomID), not door_soft_locked(Source, RoomID).
            non_gated(RoomID) :- door(Source, RoomID), non_gated(Source), not gated_area(RoomID).
           

        %% gate ordering 
            gate_order(0,0).
            %gate_order(2,1).
            
            
            gated_start(RoomID, KeyID ) :- gated_key(RoomID, KeyRoomID), key(KeyID,KeyRoomID), door_soft_locked(_,RoomID).
            %:- gate_order(K1,K2), gated_start(RoomID, K2), door_soft_locked(Source, RoomID), not gated_order(Source, GateRoomID), gate(K1,RoomID).
            %:- gate_order(K1,K2), gated_start(RoomID, K2), door_soft_locked(Source, RoomID), not gated_order(Source, GateRoomID), gate(KeyID, GateRoomID,_), K1 == KeyID.
            
            
            :- gate_order(K1,K2), gated_start(RoomID, K2), door_soft_locked(Source, RoomID), not gated_order(Source, _).
            :- gate_order(K1,K2), gated_start(RoomID, K2), door_soft_locked(Source, RoomID), gated_order(Source, GateRoomID), gate(KeyID, GateRoomID,_), K1 != KeyID.

        %% no gated area can have a directional connection into it unless it is same GateRoomID
            :- gated_order(RoomID, GateRoomID), door(Source, RoomID), not door(RoomID, Source), not gated_order(Source, GateRoomID), Source != GateRoomID.

        %% end gated area ---------------------------------


        % no softlocks on gated rooms
            :- gate(_, Source, Destination), door_soft_lock_count(Source, Count), Count > 0.
            :- gate(_, Source, Destination), door_soft_lock_count(Destination, Count), Count > 0.

            :- gate(_, Source, Destination), door_count(Source, Count), Count > 2.
            %:- gate(_, Source, Destination), door_count(Source, Count), Count > 4.

        % gated horizontally only
            gated_horizontally(0).
            %gated_horizontally(1).
            :- gate(KeyID,Source,_), door_north(Source), gated_horizontally(KeyID).
            :- gate(KeyID,Source,_), door_south(Source), gated_horizontally(KeyID).

        %% gated vertically only
            gated_vertically(0).
            %gated_vertically(2).
            :- gate(KeyID,Source,_), door_east(Source), gated_vertically(KeyID).
            :- gate(KeyID,Source,_), door_west(Source), gated_vertically(KeyID).

        %% boss room
            % #const boss_room_size = 4.
            % #const boss_gate_type = 2.
            % boss_room_size {boss_room(RoomID): room(RoomID)} boss_room_size.

            % :- boss_room(RoomID), {boss_room(Source):door(Source,RoomID)} == 0.
            % boss_room_entrance(RoomID) :- boss_room(RoomID), door(Source, RoomID), not boss_room(Source).
            % boss_room_exit(RoomID) :- boss_room(RoomID), door(RoomID, Source), not boss_room(Source).
            % :- {boss_room_entrance(_)} > 1.
            % :- {boss_room_exit(_)} > 1.
            % :- boss_room_exit(R1), boss_room_entrance(R2), R1 != R2.
            % :- boss_room_entrance(RoomID), {door(Source,RoomID): not boss_room(Source)} > 1.

            % :- boss_room(RoomID), {boss_room(Neighbor): door(Neighbor, RoomID)} < 2.

            % :- boss_room(RoomID), key(_,RoomID).
            % :- boss_room(RoomID), gate(_,RoomID,_).

            % gate(boss_gate_type, Source, RoomID) :- boss_room_entrance(RoomID), door(Source,RoomID), not boss_room(Source).
            %:- path_order(BossKeyRoom, min, D1), path_order(KeyRoom, min, D2), D1 < D2, key(boss_gate_type, BossKeyRoom), key(KeyID, KeyRoom), keys_types(KeyID).
            %gate_order(1,boss_gate_type).
        ";
    }
}