using System.Collections;
using System.Collections.Generic;
using Clingo;
using UnityEngine;

namespace ASPGenerator
{
    public class RoomGenerator : ASPFileGenerator
    {
        [SerializeField] int width = 20, height = 20, headroom = 3, shoulderroom = 2, minCeilingHeight = 2;
        [SerializeField] TileBaseTileRules tileRules;
        [SerializeField] ASPMap.MapKeyTileRule mapKeyTileRule;

        [SerializeField] WorldGraphMemory worldGraphMemory;
        protected override string getASPCode()
        {
            string aspCode = "";
            aspCode += world_gen;
            aspCode += tile_rules;


            aspCode += WorldBuilder.WorldStructure.get_floor_rules(headroom, shoulderroom);
            aspCode += WorldBuilder.WorldStructure.get_chamber_rule(minCeilingHeight);

            aspCode += WorldBuilder.Pathfinding.movement_rules;
            aspCode += WorldBuilder.Pathfinding.platform_rules;
            aspCode += WorldBuilder.Pathfinding.path_rules;


            //------ASPMemory--------
            aspCode += worldGraphMemory.ASPCode;
            //aspCode += WorldStructure.GetDoorRules(neighbors);

            return aspCode;
        }

        protected string world_gen = $@"

        #const max_width = 20.
        #const max_height = 20.

        width(1..max_width). 
        height(1..max_height).

        tile_type(empty; filled).
        state_zero(filled).
        state_one(empty).

        states(zero; one).
        
        %check_tile(XX, YY, COUNT) :- COUNT = {{XX < 1; XX > max_width; YY < 1; YY > max_height; TYPE != filled}}, tile(XX,YY,TYPE).
        check_tile(XX,YY,1) :- tile(XX,YY,Type), state_one(Type).
        check_tile(XX,YY,0) :- tile(XX,YY,Type), state_zero(Type).
        state(XX,YY,STATE) :- COUNT > 0, STATE == zero, states(STATE), check_tile(XX,YY,COUNT).
        state(XX,YY,STATE) :- COUNT < 1, STATE == one, states(STATE), check_tile(XX,YY,COUNT).

        1{{tile(XX, YY, TYPE): tile_type(TYPE)}}1 :- width(XX), height(YY).
        %room_tile(XX,YY) :- width(XX), height(YY).
        %1{{tile(XX, YY, TYPE): tile_type(TYPE)}}1 :- room_tile(XX,YY).

        headroom_offset(1..min_ceiling_height).
        floor(XX,YY) :- state(XX,YY, one), state(XX, YY-1, zero).

        #show width/1.
        #show height/1.
        #show tile/3.   
            
        ";

        protected string tile_rules { get { return generateTileRules(); } }

        string generateTileRules()
        {
            return WorldBuilder.WorldStructure.tile_rules;
            //return tileRules.GetTileRules();
        }

        protected override string getAdditionalParameters()
        {
            return $" -c max_width={width} -c max_height={height} -c headroom={headroom} -c shoulderroom={shoulderroom} -c min_ceiling_height={minCeilingHeight} " + base.getAdditionalParameters();
        }

        public virtual void InitializeGenerator(int width, int height, int headroom, int shoulderroom, int minCeilingHeight)
        {
            this.width = width;
            this.height = height;
            this.headroom = headroom;
            this.shoulderroom = shoulderroom;
            this.minCeilingHeight = minCeilingHeight;
        }

        protected override void SATISFIABLE(AnswerSet answerSet, string jobID)
        {
            FindObjectOfType<ASPMap.MapTilemap>().DisplayMap(answerSet, mapKeyTileRule);
            FindObjectOfType<ASPMap.MapTilemap>().AdjustCamera();
            //base.finalizeGenerator();
        }
    }
}

