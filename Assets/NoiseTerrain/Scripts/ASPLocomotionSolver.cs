using System.Collections;
using System.Collections.Generic;
using NoiseTerrain;
using UnityEngine;

public class ASPLocomotionSolver : LocomotionSolver
{
    public ASPLocomotionGenerator generator;

    protected override bool GetReady()
    {
        return generator.done;
    }
    public override void Solve(List<NodeChunk> nodeChunks)
    {
        generator.SetNodeChunkMemory(nodeChunks);
        generator.StartGenerator("0");
    }
}
