﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public enum GateTypes
    {
        none,
        water,
        lava,
        door,
        enemy,
        jump
    }
    public class Gates
    {
        public static string water_rules_new = @"
            #const max_water_depth = 10.
            #const min_water_depth = 1.
            #const max_water_surface = 1.

        %adding blue_gate to tile_type
            tile_fluid_type(blue_gate).

            {tile_fluid_surface(XX,YY,blue_gate): state(XX,YY,zero)}max_water_surface.
            tile_fluid(XX,YY,Type) :- tile_fluid_surface(XX,YY,Type).
            tile_fluid(XX,YY,Type) :- tile_fluid(XX,YY-1, Type), state(XX,YY,zero).
            tile_fluid(XX,YY,Type) :- tile_fluid(XX-1,YY, Type), state(XX,YY,zero).
            tile_fluid(XX,YY,Type) :- tile_fluid(XX+1,YY, Type), state(XX,YY,zero).

            fluid(XX,YY) :- tile_fluid(XX,YY,blue_gate).
            water(XX,YY, 0) :- tile_fluid(XX,YY,blue_gate).

            #show tile_fluid/3.
            %#show water/3.
            %#show water_surface/3.
            %#show water_depth/3.
        ";

        public static string water_rules = @"
            #const max_water_depth = 10.
            #const min_water_depth = 1.

        %adding blue_gate to tile_type
            tile_type(blue_gate).

            fluid(XX,YY) :- water(XX,YY,_).

        %% prevent water on bottom
           :- water(_,YY,_), YY == max_height.

            water(XX, YY, Height) :- tile(XX, YY, blue_gate), water(XX, YY+1, Depth), Height = Depth + 1.
            water(XX, YY, 1) :- tile(XX, YY, blue_gate), floor(XX, YY+1).
            water(XX, YY, 1) :- tile(XX, YY, blue_gate), YY == max_height.

            :- water(XX, YY, _), {water(XX, YY+1, _); floor(XX, YY+1); YY == max_height} < 1.
            :- tile(XX, YY, blue_gate), not water(XX, YY, _).

        %control water levels
            :- water(XX, YY, Height), Height > max_water_depth.
            :- water_surface(XX, YY, Height), Height<min_water_depth.
 
        %water must have water, wall or edge neighbors
            :- water(XX, YY, Height), {water(XX-1, YY, _); water(XX+1, YY, _); state(XX-1, YY, one); state(XX+1, YY, one); XX ==0; XX == max_width} != 2.

            %:- {water(XX, YY, _)} == 0.
            water_surface(XX, YY, Height) :- Height = #max{H1:water(XX,YY,H1); H2:water(XX-1,YY,H2); H3:water(XX+1,YY,H3)}, water(XX,YY,_), not water(XX,YY-1,_).
            water_depth(XX, YY, Depth) :- water_surface(XX, YY, _), Depth = 0.
            water_depth(XX, YY, Depth) :- water(XX, YY, _), water_depth(XX, YY-1, D1), Depth = D1 + 1.

            %#show water/3.
            %#show water_surface/3.
            %#show water_depth/3.
        ";
        public static string lava_rules = @"
            #const max_lava_depth = 10.
            #const min_lava_depth = 1.

        %adding orange_gate to tile_type
            tile_type(orange_gate).

            fluid(XX,YY) :- lava(XX,YY,_).

        %% prevent lava on bottom
        :- lava(_,YY,_), YY == max_height.

        %define orange_gates as lava
            lava(XX, YY, Height) :- tile(XX, YY, orange_gate), lava(XX, YY+1, Depth), Height = Depth + 1.
            lava(XX, YY, 1) :- tile(XX, YY, orange_gate), floor(XX, YY+1).
            lava(XX, YY, 1) :- tile(XX, YY, orange_gate), YY == max_height.

            :- lava(XX, YY, _), {lava(XX, YY+1, _); floor(XX, YY+1); YY == max_height} < 1.
            :- tile(XX, YY, orange_gate), not lava(XX, YY, _).

        %control lava levels
            :- lava(XX, YY, Height), Height > max_lava_depth.
            :- lava_surface(XX, YY, Height), Height<min_lava_depth.
 
        %lava must have water, wall or edge neighbors
            %:- lava(XX, YY, Height), {lava(XX-1, YY, _); lava(XX+1, YY, _); state(XX-1, YY, one); state(XX+1, YY, one); XX ==0; XX == max_width} != 2.
            :- lava(XX, YY, Height), {lava(XX-1, YY, _); lava(XX+1, YY, _); state(XX-1, YY, one); state(XX+1, YY, one); XX ==0; XX == max_width} != 2.

            :- {lava(XX, YY, _)} == 0.
            lava_surface(XX, YY, Height) :- Height = #max{H1:lava(XX,YY,H1); H2:lava(XX-1,YY,H2); H3:lava(XX+1,YY,H3)}, lava(XX,YY,_), not lava(XX,YY-1,_).
            lava_depth(XX, YY, Depth) :- lava_surface(XX, YY, _), Depth = 0.
            lava_depth(XX, YY, Depth) :- lava(XX, YY, _), lava_depth(XX, YY-1, D1), Depth = D1 + 1.

            %#show lava/3.
            %#show lava_surface/3.
            %#show lava_depth/3.
        ";

        public static string door_rules = @"

            #const max_door_width = 1.
            #const max_door_height = 5.

            tile_type(brown_gate).

            obstacle(XX,YY) :- door(XX,YY).

            door_count(Count) :- Count = {door_top(_,_)}.
            :- door_count(Count), Count > 1.

            door_top(XX, YY) :- tile(XX, YY, brown_gate), tile(XX, YY -1, filled).
            door_bottom(XX, YY) :- tile(XX, YY, brown_gate), tile(XX, YY +1, filled).
  
            door(XX, YY) :- door_top(XX, YY).
            door(XX, YY) :- door_bottom(XX, YY).
            door(XX, YY) :- door(XX, YY-1), tile(XX, YY, brown_gate).
            door(XX, YY) :- door(XX, YY+1), tile(XX, YY, brown_gate).

        
  
            :- tile(XX, YY, brown_gate), not door(XX, YY).
            :- door_top(XX, YY), not door(XX, YY+1).
            :- door_bottom(XX, YY), not door(XX, YY-1).
            :- door(XX, YY), not door(XX, YY-1), not door_top(XX, YY).
            :- door(XX, YY), not door(XX, YY+1), not door_bottom(XX, YY).

            door_height(XX, YY,1) :- door_bottom(XX, YY).
            door_height(XX, YY, H1) :- door(XX, YY), door_height(XX, YY+1, H2), H1 = H2 + 1.

            :- door_height(XX, YY, Height), door_top(XX, YY), Height > max_door_height.
            :- {door(XX, YY)} == 0.

            %#show door_top/2.
            %#show door/2.
            %#show door_bottom/2.
        ";

        

        public static string gating_rules = @"
            gated_max(0..20).
            gated_path(XX,YY,0) :- path(XX,YY,middle,middle).
            gated_path(XX,YY,Count) :- path(XX,YY,_), not fluid(XX,YY), not obstacle(XX,YY), path(XX + LMR, YY+TMB, _), gated_path(XX+LMR,YY+TMB, Count), lmr_offset(TMB), lmr_offset(LMR).
            gated_path(XX,YY,Count + 1) :- path(XX,YY,_), fluid(XX,YY), not obstacle(XX,YY), path(XX + LMR, YY+TMB, _), gated_path(XX+LMR,YY+TMB, Count), lmr_offset(TMB), lmr_offset(LMR), gated_max(Count + 1).
            
            gated_path(XX,YY,Count) :- path(XX,YY,_), not obstacle(XX,YY), not fluid(XX,YY), path(XX + LMR, YY+TMB, _), gated_path(XX+LMR,YY+TMB, Count), lmr_offset(TMB), lmr_offset(LMR).
            gated_path(XX,YY,Count + 1) :- path(XX,YY,_), obstacle(XX,YY), not fluid(XX,YY), path(XX + LMR, YY+TMB, _), gated_path(XX+LMR,YY+TMB, Count), lmr_offset(TMB), lmr_offset(LMR), gated_max(Count + 1).

            %min_gated_path(XX,YY,Min) :- path(XX,YY,Type,Type), path_types(Type), Min = #min{Count: gated_path(XX,YY,Count)}.
            
        ";

        public static string GetGateASP(World world, int roomID, GateTypes[] gates, RoomConnections connections)
        {
            string code = "";
            foreach(Gate gate in world.worldGraph.gates)
            {
                if(gate.source == roomID)
                {

                    //code += GetGateASP(gates[gate.type - 1], GetGatedPath(gate, connections));
                    code += GetGateASP(gate, gates, connections);
                }
            }

            foreach(Gated gated in world.worldGraph.gatedRooms)
            {
                if(gated.roomID == roomID)
                {
                    //code += GetGateASP(gates[gated.type - 1]);
                    code += GetGateASP(gated, gates);
                }
            }
            return code;
        }
        public static string GetGateASP(Gate gate, Gated gated, GateTypes[] gates, RoomConnections connections)
        {
            return GetGateASP(gate, gates, connections) + GetGateASP(gated, gates);
        }
        public static string GetGateASP(Gate gate, GateTypes[] gates, RoomConnections connections)
        {
            return GetGateASP(gates[gate.type - 1], GetGatedPath(gate, connections));
        }
        public static string GetGateASP(Gated gated, GateTypes[] gates)
        {
            return GetGateASP(gates[gated.type - 1]);
        }
        static List<string> GetGatedPath(Gate gate, RoomConnections connections)
        {
            //get neighbor ids
            int gatedID = gate.destination;
            int gateID = gate.source;
            List<string> paths = new List<string>();
            bool upGated = false;
            bool downGated = false;
            bool rightGated = false;
            bool leftGated = false;
            if (gatedID == gateID - 1) leftGated = true;
            else if (gatedID == gateID + 1) rightGated = true;
            else if (gatedID < gateID) upGated = true;
            else if (gatedID > gateID) downGated = true;
            else Debug.LogWarning("Gated Room Error");

            if(connections.upEgress || connections.upIngress)
            {
                if (upGated) paths.Insert(0, "top");
                else paths.Add("top");
            }
            if (connections.downEgress || connections.downIngress)
            {
                if (downGated) paths.Insert(0, "bottom");
                else paths.Add("bottom");
            }
            if (connections.rightEgress || connections.rightIngress)
            {
                if (rightGated) paths.Insert(0, "right");
                else paths.Add("right");
            }
            if (connections.leftEgress || connections.leftIngress)
            {
                if (leftGated) paths.Insert(0, "left");
                else paths.Add("left");
            }

            //find gated path with ids

            //a

            return paths;
        }
        static string GetGateASP(GateTypes gate)
        {
            switch (gate)
            {
                case GateTypes.water:
                    return water_rules;
                case GateTypes.lava:
                    return lava_rules;
                case GateTypes.door:
                    return door_rules;
                default:
                    return "";

            }
        }

        static string GetGateASP(GateTypes gate, List<string> paths)
        {
            switch (gate)
            {
                case GateTypes.water:
                    return GetWaterASP(paths);
                case GateTypes.lava:
                    return GetLavaASP(paths);
                case GateTypes.door:
                    return GetDoorASP(paths);
                default:
                    return "";

            }
        }

        public static string GetWaterASP(List<string> paths)
        {
            string gatedPath = paths[0];
            string code = $@"
                :-{{path(XX,YY,_):water(XX,YY,_)}} == 0.
                :- path(XX,YY,middle, middle), fluid(XX,YY-1).
                :- path(XX,YY,{gatedPath}, {gatedPath}), fluid(XX,YY-1).

            % no water touching another room
                :- water(XX,YY,_), YY == max_height.
                :- water(XX,YY,_), XX = 1.
                :- water(XX,YY,_), XX = max_width.
            ";
            
            Debug.Log($"water gatedPath: {gatedPath}");
            code += $":- gated_path(XX,YY,Count), path(XX,YY,{gatedPath},{gatedPath}), Count < 6.\n";
            for(int i = 1; i < paths.Count; i += 1)
            {
                Debug.Log($"non gatedPath: {paths[i]}");
                code += $":- not gated_path(XX,YY,0), path(XX,YY,{paths[i]},{paths[i]}).\n";
            }
            return code + water_rules + gating_rules;
        }

        public static string GetLavaASP(List<string> paths)
        {
            string gatedPath = paths[0];
            string code = $@"
                :-{{path(XX,YY,_):lava(XX,YY,_)}} == 0.
                :- path(XX,YY,middle, middle), fluid(XX,YY-1).
                :- path(XX,YY,{gatedPath}, {gatedPath}), fluid(XX,YY-1).

            % no lava touching another room
                :- lava(XX,YY,_), YY == max_height.
                :- lava(XX,YY,_), XX = 1.
                :- lava(XX,YY,_), XX = max_width.
            ";
            
            Debug.Log($"lava gatedPath: {gatedPath}");
            code += $":- gated_path(XX,YY,Count), path(XX,YY,{gatedPath},{gatedPath}), Count < 1.\n";
            for (int i = 1; i < paths.Count; i += 1)
            {
                Debug.Log($"non gatedPath: {paths[i]}");
                code += $":- not gated_path(XX,YY,0), path(XX,YY,{paths[i]},{paths[i]}).\n";
            }
            return code + lava_rules + gating_rules;
        }

        public static string GetDoorASP(List<string> paths)
        {
            string gatedPath = paths[0];
            string code = $@"
                :-{{path(XX,YY,_):door(XX,YY)}} == 0.
                :- path(XX,YY,middle, middle), obstacle(XX,YY-1).
                :- path(XX,YY,{gatedPath}, {gatedPath}), obstacle(XX,YY-1).

            % no door touching another room
                %:- door(XX,YY), YY == max_height.
                %:- door(XX,YY), XX = 1.
                %:- door(XX,YY), XX = max_width.
            ";

            Debug.Log($"door gatedPath: {gatedPath}");
            code += $":- gated_path(XX,YY,Count), path(XX,YY,{gatedPath},{gatedPath}), Count < 1.\n";
            for (int i = 1; i < paths.Count; i += 1)
            {
                Debug.Log($"non gatedPath: {paths[i]}");
                code += $":- not gated_path(XX,YY,0), path(XX,YY,{paths[i]},{paths[i]}).\n";
            }
            return code + door_rules + gating_rules;
        }
    }

    
}