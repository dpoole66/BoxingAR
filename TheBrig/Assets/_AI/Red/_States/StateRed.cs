using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boxer_AI/State")]

public class StateRed : ScriptableObject {

    public Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(StateControllerRed controller) {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateControllerRed controller) {
        for (int i = 0; i < actions.Length; i++) {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(StateControllerRed controller) {
        for (int i = 0; i < transitions.Length; i++) 
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if (decisionSucceeded) {
                controller.TransitionToState(transitions[i].trueState);
            } else {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }


}