﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    [System.Serializable]
    public class Graph
    {
        public Door[] doors;
        public Key[] keys;
        public Gate[] gates;
        public Gated[] gatedRooms;
        public int startRoomID;
        public int width;
        public int height;
        public BossRoom bossRoom;
    }

    [System.Serializable]
    public class BossRoom
    {
        public int[] bossRooms;
        public int bossEntranceRoom;
        public int bossStartRoom;
    }

    [System.Serializable]
    public class Door
    {
        public string name; //{ get { return source + "->" + destination; } }
        public int source;
        public int destination;
    }

    [System.Serializable]
    public class Key
    {
        public int type;
        public int roomID;
    }

    [System.Serializable]
    public class Gate
    {
        
        public int type;
        public int source;
        public int destination;
    }

    [System.Serializable]
    public class Gated
    {
        public int roomID;
        public int type;
    }
 }