using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseTerrain;

public class ASPLocomotionGenerator : ASPGenerator
{
    public bool done { get { return solver.SolverStatus == Clingo_02.ClingoSolver.Status.SATISFIABLE; } }
    protected List<NodeChunk> nodeChunks;
    public void SetNodeChunkMemory(List<NodeChunk> nodeChunks)
    {
        this.nodeChunks = nodeChunks;
    }
    protected string GetNodeChunksMemory()
    {
        string aspCode = "\n";
        foreach(NodeChunk nodeChunk in nodeChunks)
        {
            aspCode += $"node({nodeChunk.nodeID}).\n";
            foreach(int connectionID in nodeChunk.connectedPlatforms)
            {
                aspCode += $"edge({nodeChunk.nodeID},{connectionID}).\n";
            }
        }
        return aspCode;
    }
    protected override string getASPCode()
    {
        string aspCode = $@"

        ";

        return aspCode + GetNodeChunksMemory();
    }
}
