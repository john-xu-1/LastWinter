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
        protected override string getASPCode()
        {
            return world_gen + tile_rules;
        }

        protected string world_gen = $@"

        #const max_width = 20.
        #const max_height = 20.

        width(1..max_width). 
        height(1..max_height).

        tile_type(empty; filled).

        states(zero; one).
        %check_tile(XX, YY, COUNT) :- COUNT = {{XX < 1; XX > max_width; YY < 1; YY > max_height; TYPE == empty}}, tile(XX,YY,TYPE).
        check_tile(XX, YY, COUNT) :- COUNT = {{XX < 1; XX > max_width; YY < 1; YY > max_height; TYPE != filled}}, tile(XX,YY,TYPE).
        state(XX,YY,STATE) :- COUNT > 0, STATE == zero, states(STATE), check_tile(XX,YY,COUNT).
        state(XX,YY,STATE) :- COUNT < 1, STATE == one, states(STATE), check_tile(XX,YY,COUNT).

        1{{tile(XX, YY, TYPE): tile_type(TYPE)}}1 :- width(XX), height(YY).
        

        #show width/1.
        #show height/1.
        #show tile/3.   
            
        ";

        protected string tile_rules { get { return generateTileRules(); } }

        string generateTileRules()
        {
            return tileRules.GetTileRules();
        }

        protected override string getAdditionalParameters()
        {
            return $" -c max_width={width} -c max_height={height} -c headroom={headroom} -c shoulderroom={shoulderroom} -c min_ceiling_height={minCeilingHeight} " + base.getAdditionalParameters();
        }

        protected override void SATISFIABLE(AnswerSet answerSet, string jobID)
        {
            FindObjectOfType<ASPMap.MapTilemap>().DisplayMap(answerSet, mapKeyTileRule);
            FindObjectOfType<ASPMap.MapTilemap>().AdjustCamera();
            base.finalizeGenerator();
        }
    }
}

