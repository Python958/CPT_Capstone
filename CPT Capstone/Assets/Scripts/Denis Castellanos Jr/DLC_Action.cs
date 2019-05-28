using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DLC_Action :ScriptableObject //Base action class for other actions
{
    public abstract void Act(DLC_StateController controller);
}
