using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DLC_Transition
{
    public DLC_Decision decision;
    public DLC_State trueState;
    public DLC_State falseState;
}