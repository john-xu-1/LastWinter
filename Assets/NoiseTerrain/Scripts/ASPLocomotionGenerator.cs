using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseTerrain;

public class ASPLocomotionGenerator : ASPGenerator
{
    public Clingo_02.AnswerSet GetAnswerSet() { return solver.answerSet; }
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

            #const enemy_max = 10.
            #const enemy_min = 5.
            
            piece_types(player;enemy;item).
            piece_enemy_types(bounce_boi;missile_launcher;rolla_boi;shotgun_boi).
            piece_weapon_types(drone_controller;old_shotgun;rainy_day;magnetized_shifter).

            %% place pieces on nodes %%
            1{{piece(player,NodeID): node(NodeID)}}1.
            enemy_min{{piece(Enemy,NodeID): node(NodeID),piece_enemy_types(Enemy)}}enemy_max.
            :- piece_enemy_types(Type), not piece(Type,_).

            %% only one type of each weapon %%
            4{{piece(Weapon,NodeID): node(NodeID), piece_weapon_types(Weapon)}}4.
            :- piece_weapon_types(Type), not piece(Type,_).

            %% only one piece per room %%
            :- Count = {{piece(_,NodeID)}}, node(NodeID), Count > 1.


            %% start and end nodes %%
            start(NodeID) :- piece(player, NodeID).
            1{{end(NodeID): node(NodeID)}}1.


            %% flood sinks not on end path %%
            path(NodeID, 0, PathID) :- node(NodeID), PathID = NodeID.
            path(NodeID, Step + 1, PathID) :- edge(Source, NodeID), path(Source, Step, PathID), Step < 100.
            sink(PathID) :- node(PathID), end(NodeID), not path(NodeID,_,PathID), path(PathID,_).
            sink_source(NodeID, SinkID) :- edge(NodeID, SinkID), sink(sinkID), not sink (nodeID).


            %% player reach every piece %%
            path(NodeID, 0) :- piece(player,NodeID).
            path(NodeID, Step + 1) :- edge(Source, NodeID), path(Source, Step), Step < 100.
            :- piece(_,NodeID), not path(NodeID,_), node(NodeID).
            :- end(NodeID), not path(NodeID,_).


            %% every piece must be on a ndoe that has a path to the end %%
            :- piece(_,PathID), end(NodeID), not path(NodeID,_,PathID).

            
            #show piece/2.
            #show sink/1.
            #show start/1.
            #show end/1.
            #show sink_source/2.


        ";

        return aspCode + GetNodeChunksMemory();
    }
}
