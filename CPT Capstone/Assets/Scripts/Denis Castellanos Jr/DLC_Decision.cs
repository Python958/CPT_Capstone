using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DLC_Decision : ScriptableObject
{
    public abstract bool Decide(DLC_StateController controller);
    
}
