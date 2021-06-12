﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates 
{
    public static string water_rules = @"
      #const max_water_depth = 3.
      #const min_water_depth = 3.

      %adding blue_gate to tile_type
      tile_type(blue_gate).
      water(XX, YY, Height) :- tile(XX, YY, blue_gate), water(XX, YY+1, Depth), Height = Depth + 1.
      water(XX, YY, 1) :- tile(XX, YY, blue_gate), floor(XX, YY+1).
      water(XX, YY, 1) :- tile(XX, YY, blue_gate), YY == max_height.

      :- water(XX, YY, _), {water(XX, YY+1, _); floor(XX, YY+1); YY == max_height} < 1.
      :- tile(XX, YY, blue_gate), not water(XX, YY, _).

    %control water levels
      %:- water(XX, YY, Height), Height > max_water_depth.
      %:- water_surface(XX, YY, Height), Height<min_water_depth.
 
    %water must have water, wall or edge neighbors
      :- water(XX, YY, Height), {water(XX-1, YY, _); water(XX+1, YY, _); state(XX-1, YY, one); state(XX+1, YY, one); XX ==0; XX == max_width} != 2.

      :- {water(XX, YY, _)} == 0.
      water_surface(XX, YY, Height) :- Height = #max{H1:water(XX,YY,H1); H2:water(XX-1,YY,H2); H3:water(XX+1,YY,H3)}, water(XX,YY,_), not water(XX,YY-1,_).
      water_depth(XX, YY, Depth) :- water_surface(XX, YY, _), Depth = 0.
      water_depth(XX, YY, Depth) :- water(XX, YY, _), water_depth(XX, YY-1, D1), Depth = D1 + 1.

      #show water/3.
      #show water_surface/3.
      #show water_depth/3.
    ";

    public static string doors_rules = @"

      #const max_door_width = 1.
      #const max_door_height = 3.

      tile_type(brown_gate).
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

      #show door_top/2.
      #show door/2.
      #show door_bottom/2.
    ";

    public static string lava_rules = @"
      #const max_lava_depth = 3.
      #const min_lava_depth = 3.

      %adding orange_gate to tile_type
      tile_type(orange_gate).
      lava(XX, YY, Height) :- tile(XX, YY, orange_gate), lava(XX, YY+1, Depth), Height = Depth + 1.
      lava(XX, YY, 1) :- tile(XX, YY, orange_gate), floor(XX, YY+1).
      lava(XX, YY, 1) :- tile(XX, YY, orange_gate), YY == max_height.

      :- lava(XX, YY, _), {lava(XX, YY+1, _); floor(XX, YY+1); YY == max_height} < 1.
      :- tile(XX, YY, orange_gate), not lava(XX, YY, _).

    %control lava levels
      %:- lava(XX, YY, Height), Height > max_lava_depth.
      %:- lava_surface(XX, YY, Height), Height<min_lava_depth.
 
    %lava must have water, wall or edge neighbors
      :- lava(XX, YY, Height), {lava(XX-1, YY, _); lava(XX+1, YY, _); state(XX-1, YY, one); state(XX+1, YY, one); XX ==0; XX == max_width} != 2.

      :- {lava(XX, YY, _)} == 0.
      lava_surface(XX, YY, Height) :- Height = #max{H1:lava(XX,YY,H1); H2:lava(XX-1,YY,H2); H3:lava(XX+1,YY,H3)}, lava(XX,YY,_), not lava(XX,YY-1,_).
      lava_depth(XX, YY, Depth) :- lava_surface(XX, YY, _), Depth = 0.
      lava_depth(XX, YY, Depth) :- lava(XX, YY, _), lava_depth(XX, YY-1, D1), Depth = D1 + 1.

      #show lava/3.
      #show lava_surface/3.
      #show lava_depth/3.
    ";
}