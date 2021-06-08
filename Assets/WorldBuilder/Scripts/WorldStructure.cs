using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStructure 
{
    public static string world_gen = @"
        #const max_width = 20.
        #const max_height = 20.
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
        
    ";

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

    public static string floor_rules = @"
        #const headroom = 3.
        #const shoulderroom = 3.

        %headroom_offset(1..headroom).
        floor(XX,YY) :- state(XX,YY, one), state(XX, YY-1, zero).

    ";

    public static string chamber_rule = @"
        %#const headroom = 3.
          %#const shoulderroom = 3.
          #const min_ceiling_height = 3.

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

    ";

}
