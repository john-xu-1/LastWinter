using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class Pathfinding
    {
        public static string movement_rules = @"
            path_type(right; left; top; bottom; middle).
      
        %Create destination points (nodes)
            {path(XX,YY, Type, Type): floor(XX,YY), height(YY), width(XX)}1 :- path_type(Type).

        %One node of each type?
            %:- path(XX,YY, Type1, Type1), path(XX,YY, Type2, Type2), Type1 != Type2.
            %:- path(X1, Y1, Type,Type), path(X2,Y2, Type, Type), X1 != X2.
            %:- path(X1, Y1, Type,Type), path(X2,Y2, Type, Type), Y1 != Y2.


        %add each node type to its path type
            path(XX,YY, Type) :- path(XX,YY,Type, Type), path_type(Type).



        %ensures every point on path has headroom
            :- path(XX,YY, Type), floor(XX,YY), state(XX, YY-H, one), headroom_offset(H), path_type(Type).



        %sets neighboring floor tiles on path
            %path(XX,YY, Type) :- floor(XX,YY), path(XX-1, YY, Type), floor(XX-1,YY), path_type(Type).
            %path(XX,YY, Type) :- floor(XX,YY), path(XX+1, YY, Type), floor(XX+1,YY), path_type(Type).
            %path(XX,YY, Type) :- state(XX,YY, zero), path(XX-1, YY, Type), floor(XX-1,YY), path_type(Type).
            %path(XX,YY, Type) :- state(XX,YY, zero), path(XX+1, YY, Type), floor(XX+1,YY), path_type(Type).

        % an empty tile is on a path if its neighbor to the right or left is floor and on path
            path(XX,YY, Type) :- state(XX,YY, zero), path(XX+H, YY, Type), floor(XX+H,YY), path_type(Type), horizontal(H).

        % a floor tile is on a path if it is within the jump distance of H width and V high
            path(XX,YY,Type) :- floor(XX,YY), path(XX+H, YY+V,Type), floor(XX+H, YY+V), horizontal(H), vertical(V), path_type(Type).
  

        %jumping
            lmr_offset(-1..1).

            jump_headroom_offset(1..headroom).
            has_headroom(XX,YY) :- { state(XX,YY-H, one): jump_headroom_offset(H)} == 0, width(XX), height(YY).

            fluid(-1000,-1000).
            obstacle(-1000,-1000).
            path(XX, YY, Type) :- floor(XX+L,YY+1), path(XX+L, YY+1, Type), state(XX,YY, zero), path_type(Type), lmr_offset(L), has_headroom(XX,YY), not fluid(XX,YY).
            path(XX, YY, Type) :- floor(XX +L*2,YY+2), path(XX +L*2, YY+2, Type), state(XX +L,YY+1, zero), state(XX,YY, zero), path_type(Type), lmr_offset(L), has_headroom(XX,YY), not fluid(XX,YY).
            path(XX, YY, Type) :- floor(XX +L*3,YY+3), path(XX +L*3, YY+3, Type), state(XX +L,YY+1, zero), state(XX +L*2,YY+2, zero), state(XX,YY, zero), path_type(Type), lmr_offset(L), has_headroom(XX,YY), not fluid(XX,YY).
  
            %path(XX,YY-1,Type) :- floor(XX,YY), path(XX, YY-2, Type), state(XX,YY-1,zero), path_type(Type), lmr_offset(L), width(XX), height(YY-1).
            %path(XX,YY-2,Type) :- floor(XX,YY), path(XX, YY-3, Type), state(XX,YY-2,zero), path_type(Type), lmr_offset(L), width(XX), height(YY-2).

        %swimming
            path(XX,YY, Type) :- path(XX + LMR,YY+TMB,Type), state(XX,YY,zero), path_type(Type), lmr_offset(LMR), lmr_offset(TMB), has_headroom(XX,YY), width(XX), height(YY), fluid(XX,YY).

        %falling 
            path(XX,YY,Type) :- path(XX + LMR,YY-1,Type), state(XX,YY,zero), path_type(Type), lmr_offset(LMR), has_headroom(XX,YY), width(XX), height(YY).
            path(XX,YY,Type) :- path(XX + LMR,YY-1,Type), floor(XX,YY), path_type(Type), lmr_offset(LMR), width(XX), height(YY).



            #show path/4.
            #show path/3.
    ";

        public static string platform_rules = @"
            #show platform/3.

            horizontal(-1; 1).
            vertical(-3..3).
            platform(XX,YY,Type) :- platform(XX+H, YY+V,Type), floor(XX,YY), horizontal(H), vertical(V), path_type(Type).
      
  
            :- floor(XX,YY), not platform(XX,YY,_).
            platform(XX,YY, Type) :- path(XX,YY,Type, Type), path_type(Type).

        % no platforms can overlap
            :- platform(XX,YY, Type1), platform(XX, YY, Type2), Type1 != Type2.
    ";

        public static string path_rules = @"

        %each corner has a tile, makes sure opening occur on the correct side
          :- not state(1,1,one).
          :- not state(1,max_height,one).
          :- not state(max_width,1,one).
          :- not state(max_width,max_height,one).

          exit_up(XX,YY, Type) :- YY == 1, path(XX,YY,Type), path_type(Type).
          exit_down(XX,YY, Type) :- YY == max_height, path(XX,YY,Type), path_type(Type).
          exit_right(XX,YY, Type) :- XX == max_width, path(XX,YY,Type), path_type(Type).
          exit_left(XX,YY, Type) :- XX == 1, path(XX,YY,Type), path_type(Type).

          has_exit_down(Type) :- {exit_down(XX,YY, Type): width(XX), height(YY), path(XX,YY,bottom)} > 0, path_type(Type).
          has_exit_up(Type) :- {exit_up(XX,YY, Type): width(XX), height(YY), path(XX,YY,top)} > 0, path_type(Type).
          has_exit_right(Type) :- {exit_right(XX,YY, Type): path(XX,YY,right)} > 0, path_type(Type).
          has_exit_left(Type) :- {exit_left(XX,YY, Type): width(XX), height(YY), path(XX,YY,left)} > 0, path_type(Type).

          invalid_exit_right(StartType, EndType) :- {exit_right(XX,YY, StartType): not path(XX,YY,EndType)} > 0, path_type(StartType), path_type(EndType).
          invalid_exit_left(StartType, EndType) :- {exit_left(XX,YY, StartType): not path(XX,YY,EndType)} > 0, path_type(StartType), path_type(EndType).
          invalid_exit_up(StartType, EndType) :- {exit_up(XX,YY, StartType): not path(XX,YY,EndType)} > 0, path_type(StartType), path_type(EndType).
          invalid_exit_down(StartType, EndType) :- {exit_down(XX,YY, StartType): not path(XX,YY,EndType)} > 0, path_type(StartType), path_type(EndType).

          :- invalid_exit_right(Type, right), path_type(Type).
          :- invalid_exit_left(Type, left), path_type(Type).
          :- invalid_exit_up(Type, top), path_type(Type).
          :- invalid_exit_down(Type, bottom), path_type(Type).

            door_start_top(XX) :- state(XX - 1, YY, one), state(XX,YY,zero), YY == 1.
            door_start_bottom(XX) :- state(XX - 1, YY, one), state(XX,YY,zero), YY == max_height.
            door_start_left(YY) :- state(XX, YY - 1, one), state(XX,YY,zero), XX == 1.
            door_start_right(YY) :- state(XX, YY - 1, one), state(XX,YY,zero), XX == max_width.

            :- {door_start_top(_)} > 1.
            :- {door_start_bottom(_)} > 1.
            :- {door_start_left(_)} > 1.
            :- {door_start_right(_)} > 1.

        %Set node's location
          :- path(XX, YY, left, left), not XX == 1.
          :- path(XX, YY, right, right), not XX == max_width.

          :- path(XX, YY, left, left), not YY > 1.
          :- path(XX, YY, right, right), not YY > 1.

          :- path(XX, YY, top, top), not YY == 3.
            top_step :- path(XX,YY,top,top), state(XX-1,YY-2,one).
            top_step :- path(XX,YY,top,top), state(XX+1, YY-2, one).
            :- not top_step, path(_,_,top,top).

          :- path(XX,YY, top, top), XX > max_width - 2.
          :- path(XX,YY, top, top), XX <  3.

          % bottom
          :- path(XX,YY, bottom, bottom), {state(XX+H,YY, zero): horizontal(H)} < 1.
          :- path(XX, YY, bottom, bottom), not YY == max_height.

          :- path(XX,YY, bottom, bottom), XX > max_width - 1.
          :- path(XX,YY, bottom, bottom), XX <  2.

          :- path(XX,YY,middle,middle), YY < 3.


            :- floor(XX,YY), YY < 2.
        

          

        %connected chambers
          %:- floor(XX,YY), path(XX,YY,Type1), path(XX,YY,Type2), Type1 != Type2.

        ";

        public static string set_openings(bool[] connections)
        {
            string connection_rules = @"
            %:- {path(XX,YY,middle,middle): width(XX), height(YY)} == 0.
        ";
            connection_rules += set_opening(connections[0], connections[1], "top");
            connection_rules += set_opening(connections[2], connections[3], "right");
            connection_rules += set_opening(connections[4], connections[5], "bottom");
            connection_rules += set_opening(connections[6], connections[7], "left");


            return connection_rules;
        }

        private static string set_opening(bool egress, bool ingress, string side)
        {
            string connection_rules = "";
            if (egress || ingress)
            {
                connection_rules += $" :- {{path(XX,YY,{side}): width(XX), height(YY)}} == 0. \n";
                if (egress && !ingress)
                {
                    connection_rules += $@"
                    :-path(XX, YY, middle, middle), path(XX, YY, {side}).
                    :-path(XX, YY, {side}, {side}), not path(XX, YY, middle).
                ";
                }
                else
                {
                    connection_rules += $@"
                    :- path(XX,YY,middle,middle), not path(XX,YY,{side}).
                    :- path(XX,YY,{side},{side}), not path(XX,YY,middle).
                ";
                }
            }
            else
            {
                connection_rules += $" :- path(XX,YY,{side}), width(XX), height(YY).\n";
            }
            return connection_rules;
        }

        public static string get_path_start_rules(string path, Dictionary<string, List<List<string>>> map)
        {
            string rule = "";
            return rule;
        }
        
        public static string getPathStartRules(string path, Room room)
        {
            string rule = "";
            if(path == "top")
            {
                Vector2Int node = room.downExit;
                rule += $@"
                    bottom_node :- path({node.x} -1,_,top,top).
                    bottom_node :- path({node.x} +1,_,top,top).
                    :- not bottom_node.
                ";
            }else if(path == "bottom")
            {
                Vector2Int node = room.upExit;
                rule += $@"
                    top_node :- path({node.x} -1, _, bottom,bottom).
                    top_node :- path({node.x} +1, _, bottom,bottom).
                    :- not top_node.
                ";
            }
            else if (path == "right")
            {
                Vector2Int node = room.leftExit;
                rule += $@"
                    :- not path(_, {node.y}, right,right).
                ";
            }
            else if (path == "left")
            {
                Vector2Int node = room.rightExit;
                rule += $@"
                    :- not path(_, {node.y}, left,left).
                ";
            }
            return rule;
        }
        
        
    }
}