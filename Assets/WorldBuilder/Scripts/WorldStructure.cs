using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldBuilder
{
    public class WorldStructure
    {
        public static int max_width = 15;
        public static int max_height = 25;

        public static string world_gen { get { return @"
        
        width(1..max_width). % -> width(1), width(2),..., width(max_width)
        height(1..max_height).

        tile_type(empty; filled).
  
        %border_type(filled; red_gate).

        states(zero; one).
        %check_tile(XX, YY, COUNT) :- COUNT = {XX < 1; XX > max_width; YY < 1; YY > max_height; TYPE == empty}, tile(XX,YY,TYPE).
        check_tile(XX, YY, COUNT) :- COUNT = {XX < 1; XX > max_width; YY < 1; YY > max_height; TYPE != filled}, tile(XX,YY,TYPE).
        state(XX,YY,STATE) :- COUNT > 0, STATE == zero, states(STATE), check_tile(XX,YY,COUNT).
        state(XX,YY,STATE) :- COUNT < 1, STATE == one, states(STATE), check_tile(XX,YY,COUNT).

        1{tile(XX,YY, TYPE): tile_type(TYPE)}1 :- width(XX), height(YY).
        %1{enemy(XX,YY,Enemy): enemies(Enemy)}5 :- width(XX), height(YY).

  

        %:- tile(XX,YY,TYPE) , TYPE == red_gate.

        #show width/1.
        #show height/1.
        #show tile/3.
        
    " + $@"
        #const max_width = {max_width}.
        #const max_height = {max_height}.

    "; } }
        public static string get_world_gen(int roomWidth, int roomHeight)
        {
            max_height = roomHeight;
            max_width = roomWidth;
            return world_gen;
        }

        public static string tile_rules = @"
      #show state/3.

      %tiles must have a neighbor
      :- state(XX,YY, one), state(XX-1, YY, zero), state(XX+1, YY, zero).
      :- state(XX,YY, one), state(XX, YY-1, zero), state(XX, YY+1, zero).

      %:- state(XX,YY, one), state(XX-1, YY, zero), XX == max_width.
      %:- state(XX,YY, one), XX == 1, state(XX+1, YY, zero).
      %:- state(XX,YY, one), state(XX, YY-1, zero), YY == max_height.
      %:- state(XX,YY, one), YY == 1, state(XX, YY+1, zero).


      %no empty diagonals with adjencent filled diagonals (no checkers pattern)
      :- state(XX, YY, zero), state(XX+1, YY + 1, zero), state(XX, YY +1, one), state(XX+1, YY, one).
      :- state(XX, YY, zero), state(XX+1, YY - 1, zero), state(XX+1, YY, one), state(XX, YY-1, one).

      %no knight move from empty to empty
      :- state(XX,YY,one), state(XX-1, YY, zero), state(XX, YY+1, one), state(XX +1, YY+1, zero).
      :- state(XX,YY,one), state(XX-1, YY, one), state(XX, YY -1, zero), state(XX-1,YY +1, zero).
      :- state(XX,YY,one), state(XX,YY-1, one), state(XX+1, YY, zero), state(XX-1, YY-1, zero).
      :- state(XX,YY,one), state(XX+1,YY, one), state(XX, YY+1, zero), state(XX+1, YY-1, zero).

      :- state(XX,YY,one), state(XX-1, YY, zero), state(XX, YY-1, one), state(XX +1, YY-1, zero).
      :- state(XX,YY,one), state(XX+1, YY, one), state(XX, YY -1, zero), state(XX+1,YY +1, zero).
      :- state(XX,YY,one), state(XX,YY+1, one), state(XX+1, YY, zero), state(XX-1, YY+1, zero).
      :- state(XX,YY,one), state(XX-1,YY, one), state(XX, YY+1, zero), state(XX-1, YY-1, zero).

      % no 2 empty diagonals on a filled tile
      %:- state(XX,YY, one), state(XX+1, YY-1, zero), state(XX-1, YY+1, zero).
      %:- state(XX,YY, one), state(XX-1, YY-1, zero), state(XX+1, YY+1, zero).
      :- state(XX,YY, one), state(XX+1, YY-1, zero), state(XX-1, YY+1, zero), state(XX, YY-1, one), state(XX,YY+1,one), state(XX+1,YY,one), state(XX-1,YY,one).
      :- state(XX,YY, one), state(XX-1, YY-1, zero), state(XX+1, YY+1, zero), state(XX, YY-1, one), state(XX,YY+1,one), state(XX+1,YY,one), state(XX-1,YY,one).


    ";
        public static int headroom = 3;
        public static int shoulderroom = 3;
        public static string floor_rules { get { return @"


        %headroom_offset(1..headroom).
        floor(XX,YY) :- state(XX,YY, one), state(XX, YY-1, zero).

    " + $@"
        #const headroom = {headroom}.
        #const shoulderroom = {shoulderroom}.
    "; } }
        public static string get_floor_rules(int headroom, int shoulderroom)
        {
            WorldStructure.headroom = headroom;
            WorldStructure.shoulderroom = shoulderroom;
            return floor_rules;
        }
        public static int min_ceiling_height = 3;
        public static string chamber_rule { get { return @"
        

          headroom_offset(1..min_ceiling_height).
          floor(XX,YY) :- state(XX,YY, one), state(XX, YY-1, zero).
          :- floor(XX,YY), state(XX, YY - H, one), headroom_offset(H).

          shoulderroom_offset(1..shoulderroom).
          left_wall(XX,YY) :- state(XX,YY,one), state(XX+1,YY, zero).
          :- left_wall(XX,YY), state(XX+S,YY,one), shoulderroom_offset(S).

          left_step(XX,YY) :- floor(XX,YY), state(XX-1, YY, zero).
          right_step(XX,YY) :- floor(XX,YY), state(XX+1, YY, zero).
          :- left_step(XX,YY), state(XX - S, YY-H, one), headroom_offset(H), shoulderroom_offset(S).
          :- right_step(XX,YY), state(XX + S, YY - H, one), headroom_offset(H), shoulderroom_offset(S).

    " + $@"
        #const min_ceiling_height = {min_ceiling_height}.
        %#const shoulderroom = {shoulderroom}.
    "; } }
        public static string get_chamber_rule(int minCeilingHeight)
        {
            min_ceiling_height = minCeilingHeight;
            return chamber_rule;
        }

        public static List<Tile> get_exits(int width, int height, Dictionary<string, List<List<string>>> map, string side)
        {
            List<Tile> wall = new List<Tile>();
            foreach (List<string> tile in map["tile"])
            {
                if (side == "up" && tile[1] == "1" || side == "down" && tile[1] == height.ToString() || side == "left" && tile[0] == "1" || side == "right" && tile[0] == width.ToString())
                {
                    Tile newTile = new Tile();
                    newTile.x = int.Parse(tile[0]);
                    newTile.y = int.Parse(tile[1]);
                    //Debug.Log("newTile.type: " + tile[2]);
                    newTile.type = tile[2] == "filled" ? 1 : 0;
                    wall.Add(newTile);
                }
            }
            return wall;
        }

        public static List<Tile> get_entrances(int width, int height, Dictionary<string, List<List<string>>> map, string side)
        {
            List<Tile> wall = new List<Tile>();
            if (side == "up")
            {
                List<Tile> exitWall = get_exits(width, height, map, "down");
                foreach (Tile tile in exitWall)
                {
                    Tile newTile = new Tile();
                    newTile.x = tile.x;
                    newTile.y = 1;
                    newTile.type = tile.type;
                    wall.Add(newTile);
                }
            }
            else if (side == "down")
            {
                List<Tile> exitWall = get_exits(width, height, map, "up");
                foreach (Tile tile in exitWall)
                {
                    Tile newTile = new Tile();
                    newTile.x = tile.x;
                    newTile.y = height;
                    newTile.type = tile.type;
                    wall.Add(newTile);
                }
            }
            else if (side == "left")
            {
                List<Tile> exitWall = get_exits(width, height, map, "right");
                foreach (Tile tile in exitWall)
                {
                    Tile newTile = new Tile();
                    newTile.x = 1;
                    newTile.y = tile.y;
                    newTile.type = tile.type;
                    wall.Add(newTile);
                }
            }
            else if (side == "right")
            {
                List<Tile> exitWall = get_exits(width, height, map, "left");
                foreach (Tile tile in exitWall)
                {
                    Tile newTile = new Tile();
                    newTile.x = height;
                    newTile.y = tile.y;
                    newTile.type = tile.type;
                    wall.Add(newTile);
                }
            }
            return wall;
        }

        public static string get_door_rules(List<Tile> edges)
        {
            string door_rules = "";
            foreach (Tile tile in edges)
            {
                int x = tile.x;
                int y = tile.y;
                string type = tile.type == 1 ? "filled" : "empty";
                door_rules += $"tile({x},{y},{type}).\n";
            }
            return door_rules;
        }

        public static string GetDoorRules(Neighbors neighbors)
        {
            List<Tile> walls = new List<Tile>();
            if (neighbors.left != null) walls.AddRange(get_entrances(neighbors.left.map.dimensions.room_width, neighbors.left.map.dimensions.room_width, neighbors.left.rawMap, "left"));
            if (neighbors.right != null) walls.AddRange(get_entrances(neighbors.right.map.dimensions.room_width, neighbors.right.map.dimensions.room_width, neighbors.right.rawMap, "right"));
            if (neighbors.up != null) walls.AddRange(get_entrances(neighbors.up.map.dimensions.room_width, neighbors.up.map.dimensions.room_width, neighbors.up.rawMap, "up"));
            if (neighbors.down != null) walls.AddRange(get_entrances(neighbors.down.map.dimensions.room_width, neighbors.down.map.dimensions.room_width, neighbors.down.rawMap, "down"));

            return get_door_rules(walls);
        }
    }
}