using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASPGenerator
{
    public class WorldGraphGenerator : ASPFileGenerator
    {
        [SerializeField] protected int width = 4, height = 4, keyCount = 1, minGateTypeCount = 1, maxGateTypeCount = 2, bossRoomGateType = 1;
        

        protected override string getASPCode()
        {
            return WorldBuilder.WorldMap.test_text + WorldBuilder.WorldMap.bidirectional_rules + WorldBuilder.WorldMap.gate_key_rules;
        }

        protected override string getAdditionalParameters()
        {
            return $" -c max_width={width} -c max_height={height} -c key_count={keyCount} -c max_gate_type_count={maxGateTypeCount} -c min_gate_type_count={minGateTypeCount} -c boss_gate_type={bossRoomGateType}  " + base.getAdditionalParameters();
        }

        protected override void SATISFIABLE()
        {

            base.SATISFIABLE();
            
            
        }

        
    }
}

