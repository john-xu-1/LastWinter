using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseTerrain;

public class LocomotionSolver : MonoBehaviour
{
    public bool ready { get { return GetReady(); } }
    protected virtual bool GetReady() { return true; }

    public virtual void Solve(List<NodeChunk> nodeChunks, int seed) { }
}
