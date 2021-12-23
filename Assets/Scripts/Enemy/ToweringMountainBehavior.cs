using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToweringMountainBehavior : BossBehaviorBase
{
    protected override void ComputeVelocity()
    {
        motion();
        velocity.y = targetVelocity.y;
    }
}
