

using UnityEngine;

public abstract class ASPMemory : ScriptableObject
{
    
    public string ASPCode { get { return getASPCode(); } }
    protected abstract string getASPCode();

}
