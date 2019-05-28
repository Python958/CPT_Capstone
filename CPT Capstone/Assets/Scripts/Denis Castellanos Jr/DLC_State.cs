using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/State")] 
public class DLC_State : ScriptableObject
{
    public DLC_Action[] actions; //Contains potential actions
    public DLC_Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    //these are to slow down how often the states are checked for transitions
    private float rolex;


  public void UpdateState(DLC_StateController controller) //Function the state controller calls
    {
        DoActions (controller);
        CheckTransition(controller); //being checked every frame
    }
     
 private void DoActions(DLC_StateController controller) //Decision to complete the action
    {
        for (int i = 0; i < actions.Length; i++) //For Loop for actions array
        {
            actions[i].Act(controller);
        }
    }

private void CheckTransition(DLC_StateController controller)
    {
        rolex += Time.deltaTime;
        float periods = 0f;
        if (controller.navMeshAgent.speed == controller.enemyStats.walkSpeed) periods = controller.enemyStats.patrolUpdateSpeed;
        else { periods = controller.enemyStats.chaseUpdateSpeed; }
        if (rolex >= periods)
        {
            rolex = 0f;

            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].decision.Decide(controller);

                if (decisionSucceeded)
                {
                    controller.TransitionToState(transitions[i].trueState);
                }
                else
                {
                    controller.TransitionToState(transitions[i].falseState);
                }
            }
        }
    }
}
