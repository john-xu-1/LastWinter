using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASPMap
{
    public abstract class ASPMap : MonoBehaviour
    {
        public abstract void DisplayMap(Clingo.AnswerSet answerset, ASPMapKey mapKey);
        public abstract void AdjustCamera();
    }
}

