using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChunkHandler;
using System.Threading;

namespace LocomotionGraph
{
    public class LocomotionGraph : MonoBehaviour
    {
        RoomChunk roomChunk;
        public RoomChunk RoomChunk { get { return roomChunk; } }
        public int jumpHeight = 6;
        public bool platformSetupComplete;
        private int seed;
        
        public void SetRoomChunk(List<Chunk> roomChunks, int seed)
        {
            this.seed = seed;
            platformSetupComplete = false;
            //setupComplete = false;
            //active = true;
            this.roomChunks = roomChunks;
            Thread thread = new Thread(SetRoomChunkThread);
            thread.Start();
        }
        List<Chunk> roomChunks;
        public void SetRoomChunkThread()
        {
            roomChunk = new RoomChunk(roomChunks, jumpHeight);
            platformSetupComplete = true;
        }

        public List<PlatformChunk> GetPlatforms()
        {
            List<PlatformChunk> platforms = new List<PlatformChunk>();
            foreach (FilledChunk filledChunk in roomChunk.filledChunks)
            {
                foreach (PlatformChunk platform in filledChunk.platforms)
                {
                    platforms.Add(platform);
                }
            }
            return platforms;
        }
        public PlatformChunk GetPlatform(int platformID)
        {
            return roomChunk.GetPlatform(platformID);
        }

        public LocomotionSolver ls;
        public bool generatingLocomotionGraph = false;
        public IEnumerator GenerateLocomotionGraph()
        {
            generatingLocomotionGraph = true;
            generateLocomotionGraphThreadCompleted = false;
            Thread thread = new Thread(GenerateLocomotionGraphThread);
            thread.Start();
            while (!generateLocomotionGraphThreadCompleted)
            {
                yield return null;
            }

            ls.Solve(nodeChunks, seed);

            while (!ls.ready)
            {
                yield return null;
            }

            generatingLocomotionGraph = false;
        }

        List<NodeChunk> nodeChunks;
        bool generateLocomotionGraphThreadCompleted = false;
        private void GenerateLocomotionGraphThread()
        {
            List<int> platformIDs = roomChunk.GetPlatformIDs();
            nodeChunks = new List<NodeChunk>();
            foreach (int platformID in platformIDs)
            {
                NodeChunk nodeChunk = roomChunk.GetPlatform(platformID);
                nodeChunks.Add(nodeChunk);
            }
            roomChunk.SetPlatformEdges(platformIDs, jumpHeight, checkConnection);
            generateLocomotionGraphThreadCompleted = true;
        }

        public bool checkConnection = false;
    }
}

